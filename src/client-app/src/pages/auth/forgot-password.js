import React from "react";
import ForgotPasswordForm from "../../components/organisms/Auth/ForgotPasswordForm";
import { AuthLayout } from "../../components/templates/Layout";
import AuthService from "../../services/authService";

const ForgotPassword = (props) => {
  const authService = new AuthService();

  const onForgotPassword = async (data) => {
    return await authService
      .forgotPassword(data)
      .then((response) => {
        Promise.resolve(response);
      })
      .catch((error) => {
        Promise.reject(error);
      });
  };

  return (
    <AuthLayout>
      <ForgotPasswordForm onForgotPassword={(data) => onForgotPassword(data)} />
    </AuthLayout>
  );
};

export default ForgotPassword;
