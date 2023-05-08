import * as React from "react";
import { useState } from "react";
import { useNavigate } from "react-router-dom";
import SignUpForm from "../../components/organisms/Auth/SignUpForm";
import { AuthLayout } from "../../components/templates/Layout";
import UserSerVice from "../../services/userService";

const SignupPage = (props) => {
  const navigate = useNavigate();
  const [isFormEnabled, setFormEnabled] = useState(true);
  const userService = new UserSerVice();

  const signUp = async (data) => {
    setFormEnabled(true);

    return userService
      .register(data)
      .then((response) => {
        navigate("/auth/login");
        return Promise.resolve(response);
      })
      .catch((error) => {
        setFormEnabled(true);
        return Promise.reject(error);
      });
  };

  return (
    <AuthLayout>
      <SignUpForm
        signUp={(data) => signUp(data)}
        isFormEnabled={isFormEnabled}
      />
    </AuthLayout>
  );
};

export default SignupPage;
