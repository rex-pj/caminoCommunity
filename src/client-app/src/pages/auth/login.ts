import React, { useState, useContext } from "react";
import LoginForm from "../../components/organisms/Auth/LoginForm";
import { useNavigate } from "react-router-dom";
import { SessionContext } from "../../store/context/session-context";
import { setLogin } from "../../services/AuthLogic";
import { AuthLayout } from "../../components/templates/Layout";
import AuthService from "../../services/authService";

const Login = (props) => {
  const navigate = useNavigate();
  const [isFormEnabled, setFormEnabled] = useState(true);
  const { relogin } = useContext(SessionContext);
  const authService = new AuthService();

  const onLogin = async (criterias, isRemember) => {
    setFormEnabled(false);

    await authService
      .login(criterias)
      .then(async (response) => {
        const { data } = response;
        const { authenticationToken } = data;
        if (!authenticationToken) {
          setFormEnabled(true);
          return Promise.reject();
        }
        setLogin(data, isRemember);

        await relogin();
        navigate("/");
        return Promise.resolve(response);
      })
      .catch((error) => {
        setFormEnabled(true);
        return Promise.reject(error);
      });
  };

  return (
    <AuthLayout>
      <LoginForm onlogin={onLogin} isFormEnabled={isFormEnabled} />
    </AuthLayout>
  );
};

export default Login;
