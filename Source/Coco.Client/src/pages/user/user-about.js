import React from "react";
import { withRouter } from "react-router-dom";
import { useQuery, useMutation } from "@apollo/react-hooks";
import { GET_FULL_USER_INFO } from "../../utils/GraphQlQueries/queries";
import { UPDATE_USER_INFO_PER_ITEM } from "../../utils/GraphQlQueries/mutations";
import About from "../../components/organisms/User/About";
import Loading from "../../components/atoms/Loading";
import ErrorBlock from "../../components/atoms/ErrorBlock";

export default withRouter((props) => {
  const { userId } = props;

  const [updateUserInfoItem] = useMutation(UPDATE_USER_INFO_PER_ITEM);

  const { loading, error, data, refetch } = useQuery(GET_FULL_USER_INFO, {
    variables: {
      criterias: {
        userId,
      },
    },
  });

  if (loading) {
    return <Loading>Loading</Loading>;
  }
  if (error) {
    return <ErrorBlock>Error</ErrorBlock>;
  }

  const { fullUserInfo } = data;
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
      canEdit={canEdit}
    />
  );
});
