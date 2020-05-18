import React from "react";
import ResetPasswordForm from "../../components/organisms/Auth/ResetPasswordForm";
import { withRouter } from "react-router-dom";
import { useMutation } from "@apollo/react-hooks";
import { RESET_PASSWORD } from "../../utils/GraphQLQueries/mutations";
import { useStore } from "../../store/hook-store";
import { unauthClient } from "../../utils/GraphQLClient";

export default withRouter((props) => {
  const { match, history } = props;
  const { params } = match;
  const { email, key } = params;
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
      title: "Có lỗi xảy ra khi thay đổi mật khẩu",
      message:
        "Có lỗi xảy ra khi thay đổi mật khẩu, hãy thử lại hoặc quay lại sau",
      type: "error",
    });
  };

  const notifyInfo = (error, lang) => {
    dispatch("NOTIFY", {
      title: "Đổi mật khẩu thành công",
      message: "Bạn đã đổi mật khẩu thành công, thử đăng nhập lại",
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
          history.push("/auth/signin");
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
