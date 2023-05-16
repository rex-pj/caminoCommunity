import * as React from "react";
import { useContext } from "react";
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

interface Props {}

const LoggedInCard = (props: Props) => {
  const { currentUser, isLogin } = useContext(SessionContext);
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
    <Root className="mt-4 mt-md-0">
      {currentUser && isLogin ? (
        <Card userInfo={currentUser} menuList={menuList} />
      ) : null}
    </Root>
  );
};

export default LoggedInCard;
