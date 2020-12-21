import React, { useState, useEffect, useContext } from "react";
import LoginForm from "../../components/organisms/Auth/LoginForm";
import { useMutation } from "@apollo/client";
import { unauthClient } from "../../utils/GraphQLClient";
import { LOGIN } from "../../utils/GraphQLQueries/mutations";
import { withRouter } from "react-router-dom";
import { SessionContext } from "../../store/context/SessionContext";
import AuthService from "../../services/AuthService";
import { useStore } from "../../store/hook-store";
import { getError } from "../../utils/Helper";

export default withRouter((props) => {
  const [isFormEnabled, setFormEnabled] = useState(true);
  const dispatch = useStore(false)[1];
  const sessionContext = useContext(SessionContext);
  const [login] = useMutation(LOGIN, {
    client: unauthClient,
  });

  useEffect(() => {
    return () => {
      clearTimeout();
    };
  });

  const notifyError = (errors) => {
    if (errors) {
      errors.forEach((item) => {
        const errorCode =
          item.extensions && item.extensions.code
            ? getError(item.extensions.code, sessionContext.lang)
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
            const { userInfo, authenticationToken } = login;
            AuthService.setLogin(userInfo, authenticationToken);

            await sessionContext.relogin();
            props.history.push("/");
          }
        })
        .catch((error) => {
          setFormEnabled(true);
          showError(
            "An error occurd when login",
            getError("ErrorOccurredTryRefeshInputAgain", sessionContext.lang)
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
