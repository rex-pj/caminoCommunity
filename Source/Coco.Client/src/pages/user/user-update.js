import React, { useState, useContext, useEffect } from "react";
import { useQuery, useMutation } from "@apollo/react-hooks";
import ProfileUpdateFrom from "../../components/organisms/User/ProfileUpdateForm";
import {
  GET_FULL_USER_INFO,
  UPDATE_USER_IDENTIFIER
} from "../../utils/GraphQLQueries";
import Loading from "../../components/atoms/Loading";
import ErrorBlock from "../../components/atoms/ErrorBlock";
import { publicClient } from "../../utils/GraphQLClient";
import { useStore } from "../../store/hook-store";
import { SessionContext } from "../../store/context/SessionContext";

export default props => {
  const { userId } = props;
  const [isFormEnabled] = useState(true);
  const [updateUserIdentifier] = useMutation(UPDATE_USER_IDENTIFIER);
  const sessionContext = useContext(SessionContext);
  const { loading, error, data, refetch } = useQuery(GET_FULL_USER_INFO, {
    client: publicClient,
    variables: {
      criterias: {
        userId
      }
    }
  });

  useEffect(() => {
    return () => {
      clearTimeout();
    };
  });

  const onUpdate = async data => {
    if (!canEdit) {
      return;
    }

    await updateUserIdentifier({
      variables: {
        user: data
      }
    })
      .then(() => {
        showNotification(
          "Thay đổi thành công",
          "Bạn đã cập nhật thông tin cá nhân thành công",
          "info"
        );
        refetch();

        setTimeout(() => {
          sessionContext.relogin();
        }, 300);
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
      type: type
    });
  };

  if (loading) {
    return <Loading>Loading</Loading>;
  }
  if (error) {
    return <ErrorBlock>Error</ErrorBlock>;
  }

  const { fullUserInfo } = data;
  const { result, accessMode } = fullUserInfo;
  const canEdit = accessMode === "CAN_EDIT";
  return (
    <ProfileUpdateFrom
      onUpdate={e => onUpdate(e)}
      isFormEnabled={isFormEnabled}
      userInfo={result}
      canEdit={canEdit}
      showValidationError={showNotification}
    />
  );
};
