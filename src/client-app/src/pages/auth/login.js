import React, { useState, useContext } from "react";
import LoginForm from "../../components/organisms/Auth/LoginForm";
import { useNavigate } from "react-router-dom";
import { SessionContext } from "../../store/context/session-context";
import { setLogin } from "../../services/AuthLogic";
import { useStore } from "../../store/hook-store";
import { getError } from "../../utils/Helper";
import { AuthLayout } from "../../components/templates/Layout";
import AuthService from "../../services/AuthService";

const Login = (props) => {
  const navigate = useNavigate();
  const [isFormEnabled, setFormEnabled] = useState(true);
  const dispatch = useStore(false)[1];
  const { lang, relogin } = useContext(SessionContext);
  const authService = new AuthService();

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

  const onLogin = async (criterias, isRemember) => {
    setFormEnabled(false);

    await authService
      .login(criterias)
      .then(async (response) => {
        const { data } = response;
        const { authenticationToken } = data;
        if (!authenticationToken) {
          notifyError("An error occurd when login");
          setFormEnabled(true);
        } else {
          setLogin(data, isRemember);

          await relogin();
          navigate("/");
        }
      })
      .catch((error) => {
        setFormEnabled(true);
        showError(
          "An error occurd when login",
          getError("ErrorOccurredTryRefeshInputAgain", lang)
        );
      });
  };

  return (
    <AuthLayout>
      <LoginForm
        onlogin={onLogin}
        showValidationError={showError}
        isFormEnabled={isFormEnabled}
      />
    </AuthLayout>
  );
};

export default Login;
