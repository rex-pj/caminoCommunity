import * as React from "react";
import { Helmet } from "react-helmet-async";

type Props = {};

const UserFollowings = (props: Props) => {
  return (
    <div>
      <Helmet>
        <meta name="robots" content="noindex,nofollow" />
      </Helmet>
      Followings
    </div>
  );
};

export default UserFollowings;
