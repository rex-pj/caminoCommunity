import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import SignUpForm from "../../components/organisms/Auth/SignUpForm";
import { useStore } from "../../store/hook-store";
import { AuthLayout } from "../../components/templates/Layout";
import UserSerVice from "../../services/UserService";

const SignupPage = (props) => {
  const navigate = useNavigate();
  const [isFormEnabled, setFormEnabled] = useState(true);
  const dispatch = useStore(false)[1];
  const userService = new UserSerVice();

  const showError = (title, message) => {
    dispatch("NOTIFY", {
      title,
      message,
      type: "error",
    });
  };

  const signUp = async (data) => {
    setFormEnabled(true);

    await userService
      .register(data)
      .then((response) => {
        navigate("/auth/login");
      })
      .catch((error) => {
        setFormEnabled(true);
        showError(
          "Đăng ký KHÔNG thành công",
          "Có lỗi xảy ra trong quá trình đăng ký"
        );
      });
  };

  return (
    <AuthLayout>
      <SignUpForm
        signUp={(data) => signUp(data)}
        showValidationError={showError}
        isFormEnabled={isFormEnabled}
      />
    </AuthLayout>
  );
};

export default SignupPage;
