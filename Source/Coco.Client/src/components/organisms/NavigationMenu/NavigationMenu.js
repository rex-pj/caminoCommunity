import React, { useContext } from "react";
import styled from "styled-components";
import { RouterLinkButton } from "../../atoms/RouterLinkButtons";
import { SessionContext } from "../../../store/context/SessionContext";
import ProfileDropdown from "./ProfileDropdown";

const List = styled.ul`
  list-style: none;
  padding-left: 0;
  margin-bottom: 0;
  height: calc(${p => p.theme.size.normal} - 2px);
  margin: 2px 0;

  > li {
    display: inline-block;
  }
`;

const ListItem = styled.li`
  display: inline-block;
  height: calc(${p => p.theme.size.normal} - 2px);
`;

const Devided = styled.li`
  width: 1px;
  height: 15px;
  margin-left: 0;
  background: ${p => p.theme.rgbaColor.darkLight};
  vertical-align: middle;
  margin: 0 2px;
`;

const AuthButton = styled(RouterLinkButton)`
  color: ${p => p.theme.color.neutral};
  font-weight: 500;
  height: 100%;
  padding-top: 5px;
  padding-bottom: 5px;
  font-size: ${p => p.theme.fontSize.small};

  border: 1px solid ${p => p.theme.color.primaryLight};

  :hover {
    color: ${p => p.theme.color.light};
  }
`;

export default props => {
  const { user, isLoading } = useContext(SessionContext);

  if (isLoading) {
    return null;
  }

  if (user && user.isLogin) {
    return <ProfileDropdown userInfo={user} />;
  }

  return (
    <List className={props.className}>
      <ListItem>
        <AuthButton to="/auth/signup">Đăng Ký</AuthButton>
      </ListItem>

      <Devided />
      <ListItem>
        <AuthButton to="/auth/signin">Đăng Nhập</AuthButton>
      </ListItem>
    </List>
  );
};
