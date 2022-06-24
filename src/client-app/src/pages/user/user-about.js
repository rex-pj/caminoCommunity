import React from "react";
import { useLocation, useNavigate } from "react-router-dom";
import { useQuery, useMutation } from "@apollo/client";
import { userQueries } from "../../graphql/fetching/queries";
import { userMutations } from "../../graphql/fetching/mutations";
import About from "../../components/organisms/Profile/About";
import {
  ErrorBar,
  LoadingBar,
} from "../../components/molecules/NotificationBars";

export default (props) => {
  const { userId } = props;

  const [partialUserUpdate] = useMutation(userMutations.PARTIAL_USER_UPDATE);

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
    return <LoadingBar>Loading</LoadingBar>;
  }
  if (error) {
    return <ErrorBar>Error</ErrorBar>;
  }

  const { fullUserInfo, countrySelections, genderSelections } = data;
  const { canEdit } = fullUserInfo;

  const onEdited = async (e) => {
    if (partialUserUpdate) {
      return await partialUserUpdate({
        variables: {
          criterias: {
            key: e.primaryKey,
            value: e.value,
            propertyName: e.propertyName,
            canEdit,
          },
        },
      }).then((response) => {
        const { errors } = response;
        if (!errors) {
          refetch();
        }
      });
    }
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
