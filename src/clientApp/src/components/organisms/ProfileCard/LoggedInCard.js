import React, { useContext } from "react";
import styled from "styled-components";
import loadable from "@loadable/component";
import { faUserCheck, faComments } from "@fortawesome/free-solid-svg-icons";
import { PanelDefault } from "../../atoms/Panels";
import { SessionContext } from "../../../store/context/SessionContext";
const UserCard = loadable(() => import("./UserCard"));

const Root = styled(PanelDefault)`
  position: relative;
`;

const Card = styled(UserCard)`
  box-shadow: none;
  border-radius: 0;
`;

export default (props) => {
  var sessionContext = useContext(SessionContext);
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
  const { user } = sessionContext;
  return (
    <Root>
      {user && user.isLogin ? (
        <Card userInfo={user} menuList={menuList} />
      ) : null}
    </Root>
  );
};
