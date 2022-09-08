import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useMutation } from "@apollo/client";
import { unauthClient } from "../../graphql/client";
import SignUpForm from "../../components/organisms/Auth/SignUpForm";
import { userMutations } from "../../graphql/fetching/mutations";
import { useStore } from "../../store/hook-store";
import { AuthLayout } from "../../components/templates/Layout";

export default (props) => {
  const navigate = useNavigate();
  const [isFormEnabled, setFormEnabled] = useState(true);
  const dispatch = useStore(false)[1];
  const [signup] = useMutation(userMutations.SIGNUP, {
    client: unauthClient,
  });

  const showError = (title, message) => {
    dispatch("NOTIFY", {
      title,
      message,
      type: "error",
    });
  };

  const signUp = async (data) => {
    setFormEnabled(true);

    if (signup) {
      await signup({
        variables: {
          criterias: data,
        },
      })
        .then((response) => {
          const { errors } = response;
          if (errors) {
            showError(
              "Đăng ký KHÔNG thành công",
              "Có lỗi xảy ra trong quá trình đăng ký"
            );
          } else {
            navigate("/auth/login");
          }
        })
        .catch((error) => {
          setFormEnabled(true);
          showError(
            "Đăng ký KHÔNG thành công",
            "Có lỗi xảy ra trong quá trình đăng ký"
          );
        });
    }
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
