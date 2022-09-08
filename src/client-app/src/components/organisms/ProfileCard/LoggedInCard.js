import React, { useContext } from "react";
import styled from "styled-components";
import { faUserCheck, faComments } from "@fortawesome/free-solid-svg-icons";
import { PanelDefault } from "../../molecules/Panels";
import { SessionContext } from "../../../store/context/session-context";
const UserCoverCard = React.lazy(() => import("./UserCoverCard"));

const Root = styled(PanelDefault)`
  position: relative;
`;

const Card = styled(UserCoverCard)`
  box-shadow: none;
  border-radius: 0;
`;

export default (props) => {
  var { currentUser, isLogin } = useContext(SessionContext);
  const menu = {
    menuList: [
      {
        icon: faUserCheck,
        text: "800",
        description: "Following",
      },
      {
        icon: faComments,
        text: "350",
        description: "Topics",
      },
    ],
  };

  const { menuList } = menu;
  return (
    <Root>
      {currentUser && isLogin ? (
        <Card userInfo={currentUser} menuList={menuList} />
      ) : null}
    </Root>
  );
};
