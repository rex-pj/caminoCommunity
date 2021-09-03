import React, { useState, useContext } from "react";
import LoginForm from "../../components/organisms/Auth/LoginForm";
import { useMutation } from "@apollo/client";
import { unauthClient } from "../../graphql/client";
import { userMutations } from "../../graphql/fetching/mutations";
import { withRouter } from "react-router-dom";
import { SessionContext } from "../../store/context/session-context";
import { setLogin } from "../../services/authService";
import { useStore } from "../../store/hook-store";
import { getError } from "../../utils/Helper";

export default withRouter((props) => {
  const [isFormEnabled, setFormEnabled] = useState(true);
  const dispatch = useStore(false)[1];
  const { lang, relogin } = useContext(SessionContext);
  const [login] = useMutation(userMutations.LOGIN, {
    client: unauthClient,
  });

  const notifyError = (errors) => {
    if (errors) {
      errors.forEach((item) => {
        const errorCode =
          item.extensions && item.extensions.code
            ? getError(item.extensions.code, lang)
            : null;
        showError("An error occurd when login", errorCode);
      });
    }
  };

  const showError = (title, message) => {
    dispatch("NOTIFY", {
      title,
      message,
      type: "error",
    });
  };

  const onLogin = async (data) => {
    setFormEnabled(false);

    if (login) {
      await login({
        variables: {
          criterias: data,
        },
      })
        .then(async (response) => {
          const { data, errors } = response;
          const { login } = data;

          if (errors || !login || !login.authenticationToken) {
            notifyError(errors);
            setFormEnabled(true);
          } else {
            setLogin(login);

            await relogin();
            props.history.push("/");
          }
        })
        .catch((error) => {
          setFormEnabled(true);
          showError(
            "An error occurd when login",
            getError("ErrorOccurredTryRefeshInputAgain", lang)
          );
        });
    }
  };

  return (
    <LoginForm
      onlogin={(data) => onLogin(data)}
      showValidationError={showError}
      isFormEnabled={isFormEnabled}
    />
  );
});
