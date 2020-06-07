import React, { useState, useEffect, useContext } from "react";
import SignInForm from "../../components/organisms/Auth/SignInForm";
import { useMutation } from "@apollo/client";
import { unauthClient } from "../../utils/GraphQLClient";
import { SIGNIN } from "../../utils/GraphQLQueries/mutations";
import { withRouter } from "react-router-dom";
import { SessionContext } from "../../store/context/SessionContext";
import AuthService from "../../services/AuthService";
import { useStore } from "../../store/hook-store";
import { getError } from "../../utils/Helper";

export default withRouter((props) => {
  const [isFormEnabled, setFormEnabled] = useState(true);
  const dispatch = useStore(false)[1];
  const sessionContext = useContext(SessionContext);
  const [signin] = useMutation(SIGNIN, {
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
        showError("Đăng nhập KHÔNG thành công", errorCode);
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

  const signIn = async (data) => {
    setFormEnabled(false);

    if (signin) {
      await signin({
        variables: {
          criterias: data,
        },
      })
        .then(async (response) => {
          const { data, errors } = response;
          const { signin } = data;

          if (errors || !signin || !signin.authenticationToken) {
            notifyError(errors);
            setFormEnabled(true);
          } else {
            const { userInfo, authenticationToken } = signin;
            AuthService.setLogin(userInfo, authenticationToken);

            await sessionContext.relogin();
            props.history.push("/");
          }
        })
        .catch((error) => {
          setFormEnabled(true);
          showError(
            "Đăng nhập KHÔNG thành công",
            getError("ErrorOccurredTryRefeshInputAgain", sessionContext.lang)
          );
        });
    }
  };

  return (
    <SignInForm
      onSignin={(data) => signIn(data)}
      showValidationError={showError}
      isFormEnabled={isFormEnabled}
    />
  );
});
