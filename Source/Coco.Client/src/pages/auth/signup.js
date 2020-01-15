import React, { useState } from "react";
import { withRouter } from "react-router-dom";
import { useMutation } from "@apollo/react-hooks";
import { publicClient } from "../../utils/GraphQLClient";
import SignUpForm from "../../components/organisms/Auth/SignUpForm";
import { SIGN_UP } from "../../utils/GraphQLQueries";
import { useStore } from "../../store/hook-store";

export default withRouter(props => {
  const [isFormEnabled, setFormEnabled] = useState(true);
  const dispatch = useStore(false)[1];
  const [signup] = useMutation(SIGN_UP, {
    client: publicClient
  });

  const showValidationError = (title, message) => {
    dispatch("NOTIFY", {
      title,
      message,
      type: "error"
    });
  };

  const signUp = async data => {
    setFormEnabled(true);

    if (signup) {
      await signup({
        variables: {
          user: data
        }
      })
        .then(result => {
          props.history.push("/auth/signin");
        })
        .catch(error => {
          setFormEnabled(true);
          showValidationError(
            "Đăng ký KHÔNG thành công",
            "Có lỗi xảy ra trong quá trình đăng ký"
          );
        });
    }
  };

  return (
    <SignUpForm
      signUp={data => signUp(data)}
      showValidationError={showValidationError}
      isFormEnabled={isFormEnabled}
    />
  );
});
