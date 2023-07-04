import * as React from "react";
import ForgotPasswordForm from "../../components/organisms/Auth/ForgotPasswordForm";
import { AuthLayout } from "../../components/templates/Layout";
import AuthService from "../../services/authService";

type Props = {};

const ForgotPassword = (props: Props) => {
  const authService = new AuthService();

  const onForgotPassword = async (data: any) => {
    return await authService
      .forgotPassword(data)
      .then((response) => {
        if (response.status === 202) {
          return Promise.reject();
        }
        return Promise.resolve(response);
      })
      .catch((error) => {
        return Promise.reject(error);
      });
  };

  return (
    <AuthLayout>
      <ForgotPasswordForm onForgotPassword={(data) => onForgotPassword(data)} />
    </AuthLayout>
  );
};

export default ForgotPassword;
