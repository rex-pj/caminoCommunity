import React, { useContext } from "react";
import styled from "styled-components";
import { RouterLinkButtonPrimary } from "../../atoms/Buttons/RouterLinkButtons";
import { SessionContext } from "../../../store/context/SessionContext";
import TopPersonalDropdown from "./TopPersonalDropdown";

const List = styled.ul`
  list-style: none;
  padding-left: 0;
  margin-bottom: 0;
  height: calc(${(p) => p.theme.size.normal} - 2px);
  margin: 2px 0;

  > li {
    display: inline-block;
  }
`;

const ListItem = styled.li`
  display: inline-block;
  height: calc(${(p) => p.theme.size.normal} - 2px);
`;

const Devided = styled.li`
  width: 1px;
  height: 15px;
  margin-left: 0;
  background: ${(p) => p.theme.rgbaColor.darkLight};
  vertical-align: middle;
  margin: 0 2px;
`;

const AuthButton = styled(RouterLinkButtonPrimary)`
  color: ${(p) => p.theme.color.neutralText};
  background-color: ${(p) => p.theme.rgbaColor.darkLight};
  border: 1px solid ${(p) => p.theme.rgbaColor.light};
  height: 100%;
  padding-top: 5px;
  padding-bottom: 5px;
  font-size: ${(p) => p.theme.fontSize.tiny};
`;

export default (props) => {
  const { user, isLoading } = useContext(SessionContext);

  if (isLoading) {
    return null;
  }

  if (user && user.isLogin) {
    return <TopPersonalDropdown userInfo={user} />;
  }

  return (
    <List className={props.className}>
      <ListItem>
        <AuthButton to="/auth/signup">Sign Up</AuthButton>
      </ListItem>

      <Devided />
      <ListItem>
        <AuthButton to="/auth/login">Login</AuthButton>
      </ListItem>
    </List>
  );
};
