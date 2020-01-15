import React, { useState, useEffect, useContext } from "react";
import SignInForm from "../../components/organisms/Auth/SignInForm";
import { useMutation } from "@apollo/react-hooks";
import { publicClient } from "../../utils/GraphQLClient";
import { SIGNIN } from "../../utils/GraphQLQueries";
import { withRouter } from "react-router-dom";
import { SessionContext } from "../../store/context/SessionContext";
import AuthService from "../../services/AuthService";
import { useStore } from "../../store/hook-store";
import { getError } from "../../utils/Helper";

export default withRouter(props => {
  const [isFormEnabled, setFormEnabled] = useState(true);
  const dispatch = useStore(false)[1];
  const sessionContext = useContext(SessionContext);
  const [signin] = useMutation(SIGNIN, {
    client: publicClient
  });

  useEffect(() => {
    return () => {
      clearTimeout();
    };
  });

  const notifyError = (error, lang) => {
    if (
      error &&
      error.networkError &&
      error.networkError.result &&
      error.networkError.result.errors
    ) {
      const errors = error.networkError.result.errors;

      errors.forEach(item => {
        dispatch("NOTIFY", {
          title: "Đăng nhập KHÔNG thành công",
          message:
            item.extensions && item.extensions.code
              ? getError(item.extensions.code, lang)
              : null,
          type: "error"
        });
      });
    } else {
      dispatch("NOTIFY", {
        title: "Đăng nhập KHÔNG thành công",
        message: getError("ErrorOccurredTryRefeshInputAgain", lang),
        type: "error"
      });
    }
  };

  const showValidationError = (title, message) => {
    dispatch("NOTIFY", {
      title,
      message,
      type: "error"
    });
  };

  const signIn = async data => {
    setFormEnabled(false);

    if (signin) {
      await signin({
        variables: {
          args: data
        }
      })
        .then(async response => {
          const { data } = response;
          const { signin } = data;
          const { result } = signin;

          if (!signin || !signin.isSucceed) {
            notifyError(data.signin.errors, sessionContext.lang);
            setFormEnabled(true);
            return;
          }

          AuthService.setLogin(result.userInfo, result.authenticationToken);

          await sessionContext.relogin();
          props.history.push("/");
        })
        .catch(error => {
          setFormEnabled(true);
          notifyError(error, sessionContext.lang);
        });
    }
  };

  return (
    <SignInForm
      onSignin={data => signIn(data)}
      showValidationError={showValidationError}
      isFormEnabled={isFormEnabled}
    />
  );
});
