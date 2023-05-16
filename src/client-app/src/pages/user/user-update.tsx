import React, { useState, useContext, useEffect } from "react";
import { useQuery } from "@apollo/client";
import ProfileUpdateFrom from "../../components/organisms/Profile/ProfileUpdateForm";
import { userQueries } from "../../graphql/fetching/queries";
import UserService from "../../services/userService";
import {
  ErrorBar,
  LoadingBar,
} from "../../components/molecules/NotificationBars";
import { useStore } from "../../store/hook-store";
import { SessionContext } from "../../store/context/session-context";

const UserUpdate = (props) => {
  const { userId } = props;
  const [isFormEnabled] = useState(true);
  const dispatch = useStore(false)[1];
  const userService = new UserService();
  const { relogin } = useContext(SessionContext);
  const { loading, error, data, refetch } = useQuery(
    userQueries.GET_USER_IDENTIFY,
    {
      variables: {
        criterias: {
          userId,
        },
      },
    }
  );

  let timeoutId: any = null;

  useEffect(() => {
    return () => {
      clearTimeout(timeoutId);
    };
  });

  const onUpdate = async (data) => {
    if (!canEdit) {
      return;
    }

    await userService
      .updateIdentifiers(data)
      .then((response) => {
        showNotification(
          "The information is changed successfully",
          "The information is changed successfully",
          "info"
        );
        refetch();

        timeoutId = setTimeout(() => {
          if (!relogin) {
            return;
          }
          relogin();
        }, 300);
      })
      .catch(() => {
        showNotification(
          "An error occured when updating your information",
          "Please check your input and try again",
          "error"
        );
      });
  };

  const showNotification = (title: string, message: string, type?: string) => {
    dispatch("NOTIFY", {
      title,
      message,
      type: type,
    });
  };

  if (loading) {
    return <LoadingBar />;
  }
  if (error) {
    return <ErrorBar />;
  }

  const { userIdentityInfo } = data;
  const { canEdit } = userIdentityInfo;
  return (
    <ProfileUpdateFrom
      onUpdate={(e) => onUpdate(e)}
      isFormEnabled={isFormEnabled}
      userInfo={userIdentityInfo}
      showValidationError={showNotification}
    />
  );
};

export default UserUpdate;
