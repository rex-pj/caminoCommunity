import * as React from "react";
import { useState, useContext } from "react";
import LoginForm from "../../components/organisms/Auth/LoginForm";
import { useNavigate } from "react-router-dom";
import { SessionContext } from "../../store/context/session-context";
import { setLogin } from "../../services/AuthLogic";
import { AuthLayout } from "../../components/templates/Layout";
import AuthService from "../../services/authService";
import { Helmet } from "react-helmet-async";

type Props = {};

const Login = (props: Props) => {
  const navigate = useNavigate();
  const [isFormEnabled, setFormEnabled] = useState(true);
  const { relogin } = useContext(SessionContext);
  const authService = new AuthService();

  const onLogin = async (criterias: any, isRemember: boolean) => {
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

        await relogin(data);
        navigate("/");
        return Promise.resolve(response);
      })
      .catch((error) => {
        setFormEnabled(true);
        return Promise.reject(error);
      });
  };

  return (
    <>
      <Helmet>
        <meta name="robots" content="noindex,nofollow" />
      </Helmet>
      <AuthLayout>
        <LoginForm onlogin={onLogin} isFormEnabled={isFormEnabled} />
      </AuthLayout>
    </>
  );
};

export default Login;
