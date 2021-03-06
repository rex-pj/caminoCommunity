import React from "react";
import UserActive from "../../components/organisms/Profile/UserActive";
import { withRouter } from "react-router-dom";
import { useQuery } from "@apollo/client";
import { userQueries } from "../../graphql/fetching/queries";
import { unauthClient } from "../../graphql/client";

export default withRouter((props) => {
  const { match } = props;
  const { params } = match;
  const { email } = params;
  let { key } = params;
  if (!key && params[0]) {
    key = params[0];
  }

  const { data, loading, error } = useQuery(userQueries.ACTIVE_USER, {
    client: unauthClient,
    variables: {
      criterias: {
        email,
        activeKey: key,
      },
    },
  });
  const { history } = props;

  if (loading) {
    return (
      <UserActive
        icon="check"
        title="Waiting for activation"
        instruction="After successful activation you will be redirected to the login page"
        actionUrl="/"
        actionText="Vào trang chủ"
      />
    );
  }

  if (error) {
    history.push("/error");
  }

  if (data) {
    const { active } = data;
    if (active && !!active.isSucceed) {
      history.push("/auth/login");
    }
  }

  return (
    <UserActive
      icon="check"
      title="Successful activation"
      instruction="You have successfully activated your account"
      actionUrl="/"
      actionText="Vào trang chủ"
    />
  );
});
