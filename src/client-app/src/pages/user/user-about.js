import React from "react";
import { useQuery } from "@apollo/client";
import { userQueries } from "../../graphql/fetching/queries";
import About from "../../components/organisms/Profile/About";
import {
  ErrorBar,
  LoadingBar,
} from "../../components/molecules/NotificationBars";
import UserService from "../../services/userService";

const UserAbout = (props) => {
  const { userId } = props;
  const userService = new UserService();

  const { loading, error, data, refetch } = useQuery(
    userQueries.GET_FULL_USER_INFO,
    {
      variables: {
        criterias: {
          userId,
        },
      },
    }
  );

  if (loading) {
    return <LoadingBar />;
  }
  if (error) {
    return <ErrorBar />;
  }

  const { fullUserInfo, countrySelections, genderSelections } = data;
  const { canEdit } = fullUserInfo;

  const onEdited = async (e) => {
    await userService
      .partialUpdate({
        key: e.primaryKey,
        updates: [
          {
            value: e.value,
            propertyName: e.propertyName,
          },
        ],
      })
      .then((response) => {
        refetch();
      });
  };

  return (
    <About
      onEdited={(e) => onEdited(e)}
      userInfo={fullUserInfo}
      genderSelections={genderSelections}
      countrySelections={countrySelections}
      canEdit={canEdit}
    />
  );
};

export default UserAbout;
