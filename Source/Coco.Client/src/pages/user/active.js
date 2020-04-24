import React from "react";
import Active from "../../components/organisms/User/Active";
import { withRouter } from "react-router-dom";
import { useQuery } from "@apollo/react-hooks";
import { ACTIVE } from "../../utils/GraphQlQueries/queries";
import { unauthClient } from "../../utils/GraphQLClient";

export default withRouter((props) => {
  const { match } = props;
  const { params } = match;
  const { email, key } = params;

  const { data, loading, error } = useQuery(ACTIVE, {
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
      <Active
        icon="check"
        title="Đang chờ kích hoạt"
        instruction="Sau khi kích hoạt thành công bạn sẽ được chuyển đến trang đăng nhập"
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
      history.push("/auth/signin");
    }
  }

  return (
    <Active
      icon="check"
      title="Kích hoạt thành công"
      instruction="Bạn đã kích hoạt tài khoản thành công"
      actionUrl="/"
      actionText="Vào trang chủ"
    />
  );
});
