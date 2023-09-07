import React, { useState, useContext, useEffect } from "react";
import { useQuery } from "@apollo/client";
import ProfileUpdateFrom from "../../components/organisms/Profile/ProfileUpdateForm";
import { userQueries } from "../../graphql/fetching/queries";
import UserService from "../../services/userService";
import { ErrorBar, LoadingBar } from "../../components/molecules/NotificationBars";
import { SessionContext } from "../../store/context/session-context";
import { Helmet } from "react-helmet-async";

const UserUpdate = (props) => {
  const { userId } = props;
  const [isFormEnabled] = useState(true);
  const userService = new UserService();
  const { relogin } = useContext(SessionContext);
  const { loading, error, data, refetch } = useQuery(userQueries.GET_USER_IDENTIFY, {
    variables: {
      criterias: {
        userId,
      },
    },
  });

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

    await userService.updateIdentifiers(data).then(async (response) => {
      await refetch();

      timeoutId = setTimeout(() => {
        if (!relogin) {
          return;
        }
        relogin();
      }, 300);
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
    <>
      <Helmet>
        <meta name="robots" content="noindex,nofollow" />
      </Helmet>
      <ProfileUpdateFrom onUpdate={(e) => onUpdate(e)} isFormEnabled={isFormEnabled} userInfo={userIdentityInfo} />
    </>
  );
};

export default UserUpdate;
