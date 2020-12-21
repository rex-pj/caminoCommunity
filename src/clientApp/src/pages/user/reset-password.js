import React from "react";
import ResetPasswordForm from "../../components/organisms/Auth/ResetPasswordForm";
import { withRouter } from "react-router-dom";
import { useMutation } from "@apollo/client";
import { RESET_PASSWORD } from "../../utils/GraphQLQueries/mutations";
import { useStore } from "../../store/hook-store";
import { unauthClient } from "../../utils/GraphQLClient";

export default withRouter((props) => {
  const { match, history } = props;
  const { params } = match;
  const { email } = params;
  let { key } = params;
  if (!key && params[0]) {
    key = params[0];
  }

  const [resetPassword] = useMutation(RESET_PASSWORD, {
    client: unauthClient,
  });
  const dispatch = useStore(false)[1];

  const showValidationError = (title, message) => {
    dispatch("NOTIFY", {
      title,
      message,
      type: "error",
    });
  };

  const notifyError = (error, lang) => {
    dispatch("NOTIFY", {
      title: "Something went wrong when update your password",
      message:
        "Something went wrong when update your password, please check your input and try again or turn back after",
      type: "error",
    });
  };

  const notifyInfo = (error, lang) => {
    dispatch("NOTIFY", {
      title: "Update your password successfully",
      message: "Your password has changed successfully, please login again",
      type: "info",
    });
  };

  const onResetPassword = async (resetPasswordData) => {
    await resetPassword({
      variables: {
        criterias: resetPasswordData,
      },
    })
      .then((response) => {
        const { data } = response;
        const { resetPassword: rs } = data;

        if (!rs || !rs.isSucceed) {
          notifyError(rs.errors, "vn");
        } else {
          notifyInfo();
          history.push("/auth/login");
        }
      })
      .catch(() => {
        notifyError();
      });
  };

  const model = { email, key };

  return (
    <ResetPasswordForm
      resetPassword={onResetPassword}
      args={model}
      showValidationError={showValidationError}
    />
  );
});
