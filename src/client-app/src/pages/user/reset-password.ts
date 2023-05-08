import * as React from "react";
import ResetPasswordForm from "../../components/organisms/Auth/ResetPasswordForm";
import { useParams, useNavigate } from "react-router-dom";
import { PromptLayout } from "../../components/templates/Layout";
import AuthService from "../../services/authService";

const ResetPassword = (props) => {
  const authService = new AuthService();
  const navigate = useNavigate();
  const params = useParams();
  const { email } = params;
  let { key } = params;
  if (!key && params["*"]) {
    key = params["*"];
  }

  const onResetPassword = async (resetPasswordData) => {
    await authService
      .resetPassword(resetPasswordData)
      .then((response) => {
        navigate("/auth/login");
        return Promise.resolve();
      })
      .catch(() => {
        return Promise.reject();
      });
  };

  return (
    <PromptLayout>
      <ResetPasswordForm
        resetPassword={onResetPassword}
        args={{ email, key }}
      />
    </PromptLayout>
  );
};

export default ResetPassword;
