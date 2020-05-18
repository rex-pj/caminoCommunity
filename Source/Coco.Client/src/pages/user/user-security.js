import React, { useState } from "react";
import { withRouter } from "react-router-dom";
import { useMutation } from "@apollo/react-hooks";
import UpdatePasswordForm from "../../components/organisms/User/UpdatePasswordForm";
import { UPDATE_USER_PASSWORD } from "../../utils/GraphQLQueries/mutations";
import { useStore } from "../../store/hook-store";

export default withRouter((props) => {
  const [isFormEnabled, setFormEnabled] = useState(true);
  const dispatch = useStore(false)[1];
  const [updateUserPassword] = useMutation(UPDATE_USER_PASSWORD);
  const { canEdit } = props;

  const onUpdateConfirmation = () => {
    dispatch("OPEN_MODAL", {
      data: {
        title: "Bạn sẽ cần phải thoát và đăng nhập lại",
        message:
          "Để đảm bảo các chức năng được hoạt động tốt bạn cần thoát ra và đăng nhập lại",
        executeButtonName: "Đồng ý",
        executeUrl: "/auth/signout",
      },
      options: {
        isOpen: true,
        type: "CONFIRM_REDIRECT",
        unableClose: true,
      },
    });
  };

  const showNotification = (title, message, type) => {
    dispatch("NOTIFY", {
      title,
      message,
      type: type,
    });
  };

  const onUpdatePassword = async (data) => {
    if (!canEdit) {
      return;
    }

    setFormEnabled(true);

    if (updateUserPassword) {
      await updateUserPassword({
        variables: {
          criterias: data,
        },
      })
        .then((response) => {
          const { errors } = response;
          if (errors) {
            setFormEnabled(true);
            showNotification(
              "Có lỗi khi cập nhật mật khẩu",
              "Kiểm tra lại thông tin và thử lại",
              "error"
            );
          }

          showNotification(
            "Thay đổi mật khẩu thành công",
            "Bạn đã cập nhật mật khẩu thành công",
            "info"
          );
          onUpdateConfirmation();
          setFormEnabled(true);
        })
        .catch((error) => {
          setFormEnabled(true);
          showNotification(
            "Có lỗi khi cập nhật mật khẩu",
            "Kiểm tra lại thông tin và thử lại",
            "error"
          );
        });
    }
  };

  return (
    <UpdatePasswordForm
      onUpdate={(e) => onUpdatePassword(e, canEdit)}
      isFormEnabled={isFormEnabled}
      canEdit={canEdit}
      showValidationError={props.showValidationError}
    />
  );
});
