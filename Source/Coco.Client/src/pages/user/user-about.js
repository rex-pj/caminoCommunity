import React from "react";
import { withRouter } from "react-router-dom";
import { useQuery, useMutation } from "@apollo/react-hooks";
import {
  GET_FULL_USER_INFO,
  UPDATE_USER_INFO_PER_ITEM
} from "../../utils/GraphQLQueries";
import About from "../../components/organisms/User/About";
import Loading from "../../components/atoms/Loading";
import ErrorBlock from "../../components/atoms/ErrorBlock";
import { publicClient } from "../../utils/GraphQLClient";

export default withRouter(props => {
  const { userId } = props;

  const [updateUserInfoItem] = useMutation(UPDATE_USER_INFO_PER_ITEM);

  const { loading, error, data, refetch } = useQuery(GET_FULL_USER_INFO, {
    client: publicClient,
    variables: {
      criterias: {
        userId
      }
    }
  });

  if (loading) {
    return <Loading>Loading</Loading>;
  }
  if (error) {
    return <ErrorBlock>Error</ErrorBlock>;
  }

  const { fullUserInfo } = data;
  const { accessMode, result } = fullUserInfo;
  const canEdit = accessMode === "CAN_EDIT";

  const onEdited = async e => {
    if (updateUserInfoItem) {
      return await updateUserInfoItem({
        variables: {
          criterias: {
            key: e.primaryKey,
            value: e.value,
            propertyName: e.propertyName,
            canEdit
          }
        }
      }).then(() => {
        refetch();
      });
    }
  };

  return (
    <About onEdited={e => onEdited(e)} userInfo={result} canEdit={canEdit} />
  );
});
