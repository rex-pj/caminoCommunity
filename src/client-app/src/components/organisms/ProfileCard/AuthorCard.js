import React from "react";
import styled from "styled-components";
import { faUserCheck, faComments } from "@fortawesome/free-solid-svg-icons";
import ProfileCardInfo from "./ProfileCardInfo";
import { PanelDefault } from "../../molecules/Panels";
const UserCoverCard = React.lazy(() => import("./UserCoverCard"));

const Root = styled(PanelDefault)`
  position: relative;
`;

const Card = styled(UserCoverCard)`
  box-shadow: none;
  border-radius: 0;
  border-bottom: 1px solid ${(p) => p.theme.color.neutralBg};
`;

const AuthorCard = (props) => {
  const { author } = props;
  const menuList = [
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
  ];

  return (
    <Root className="mt-4 mt-md-0">
      <Card menuList={menuList} userInfo={author} />
      <ProfileCardInfo author={author} />
    </Root>
  );
};

export default AuthorCard;
