import React, { useState, useContext, useEffect } from "react";
import { useQuery, useMutation } from "@apollo/client";
import ProfileUpdateFrom from "../../components/organisms/User/ProfileUpdateForm";
import { userQueries } from "../../graphql/fetching/queries";
import { userMutations } from "../../graphql/fetching/mutations";
import Loading from "../../components/atoms/Loading";
import ErrorBlock from "../../components/atoms/ErrorBlock";
import { useStore } from "../../store/hook-store";
import { SessionContext } from "../../store/context/session-context";

export default (props) => {
  const { userId } = props;
  const [isFormEnabled] = useState(true);
  const dispatch = useStore(false)[1];
  const [updateUserIdentifier] = useMutation(
    userMutations.UPDATE_USER_IDENTIFIER
  );
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
            "An error occcured when updating your information",
            "Please check your input and try again",
            "error"
          );
        } else {
          showNotification(
            "The information is changed successfully",
            "The information is changed successfully",
            "info"
          );
          refetch();

          setTimeout(() => {
            relogin();
          }, 300);
        }
      })
      .catch(() => {
        showNotification(
          "An error occured when updating your information",
          "Please check your input and try again",
          "error"
        );
      });
  };

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

  const { userIdentityInfo } = data;
  const { canEdit } = userIdentityInfo;
  return (
    <ProfileUpdateFrom
      onUpdate={(e) => onUpdate(e)}
      isFormEnabled={isFormEnabled}
      userInfo={userIdentityInfo}
      canEdit={canEdit}
      showValidationError={showNotification}
    />
  );
};
