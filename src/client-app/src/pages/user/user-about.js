import React from "react";
import { withRouter } from "react-router-dom";
import { useQuery, useMutation } from "@apollo/client";
import { userQueries } from "../../graphql/fetching/queries";
import { userMutations } from "../../graphql/fetching/mutations";
import About from "../../components/organisms/User/About";
import Loading from "../../components/atoms/Loading";
import ErrorBlock from "../../components/atoms/ErrorBlock";

export default withRouter((props) => {
  const { userId } = props;

  const [updateUserInfoItem] = useMutation(
    userMutations.UPDATE_USER_INFO_PER_ITEM
  );

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
    return <Loading>Loading</Loading>;
  }
  if (error) {
    return <ErrorBlock>Error</ErrorBlock>;
  }

  const { fullUserInfo, countrySelections, genderSelections } = data;
  const { canEdit } = fullUserInfo;

  const onEdited = async (e) => {
    if (updateUserInfoItem) {
      return await updateUserInfoItem({
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
});
