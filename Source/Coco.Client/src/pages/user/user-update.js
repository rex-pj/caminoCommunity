import React, { useState, useContext, useEffect } from "react";
import { useQuery, useMutation } from "@apollo/react-hooks";
import ProfileUpdateFrom from "../../components/organisms/User/ProfileUpdateForm";
import { GET_USER_IDENTIFY } from "../../utils/GraphQlQueries/queries";
import { UPDATE_USER_IDENTIFIER } from "../../utils/GraphQlQueries/mutations";
import Loading from "../../components/atoms/Loading";
import ErrorBlock from "../../components/atoms/ErrorBlock";
import { useStore } from "../../store/hook-store";
import { SessionContext } from "../../store/context/SessionContext";

export default (props) => {
  const { userId } = props;
  const [isFormEnabled] = useState(true);
  const [updateUserIdentifier] = useMutation(UPDATE_USER_IDENTIFIER);
  const sessionContext = useContext(SessionContext);
  const { loading, error, data, refetch } = useQuery(GET_USER_IDENTIFY, {
    variables: {
      criterias: {
        userId,
      },
    },
  });

  useEffect(() => {
    return () => {
      clearTimeout();
    };
  });

  const onUpdate = async (data) => {
    if (!canEdit) {
      return;
    }

    await updateUserIdentifier({
      variables: {
        criterias: data,
      },
    })
      .then((response) => {
        const { errors } = response;
        if (errors) {
          showNotification(
            "Có lỗi xảy ra trong quá trình cập nhật",
            "Kiểm tra lại thông tin và thử lại",
            "error"
          );
        } else {
          showNotification(
            "Thay đổi thành công",
            "Bạn đã cập nhật thông tin cá nhân thành công",
            "info"
          );
          refetch();

          setTimeout(() => {
            sessionContext.relogin();
          }, 300);
        }
      })
      .catch(() => {
        showNotification(
          "Có lỗi xảy ra trong quá trình cập nhật",
          "Kiểm tra lại thông tin và thử lại",
          "error"
        );
      });
  };

  const dispatch = useStore(false)[1];
  const showNotification = (title, message, type) => {
    dispatch("NOTIFY", {
      title,
      message,
      type: type,
    });
  };

  if (loading) {
    return <Loading>Loading</Loading>;
  }
  if (error) {
    return <ErrorBlock>Error</ErrorBlock>;
  }

  const { fullUserInfo } = data;
  const { canEdit } = fullUserInfo;
  return (
    <ProfileUpdateFrom
      onUpdate={(e) => onUpdate(e)}
      isFormEnabled={isFormEnabled}
      userInfo={fullUserInfo}
      canEdit={canEdit}
      showValidationError={showNotification}
    />
  );
};
