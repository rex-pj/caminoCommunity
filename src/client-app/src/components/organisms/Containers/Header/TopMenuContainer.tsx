import * as React from "react";
import { useContext } from "react";
import styled from "styled-components";
import { RouterLinkButtonPrimary } from "../../../atoms/Buttons/RouterLinkButtons";
import { SessionContext } from "../../../../store/context/session-context";
import TopProfileContainer from "./TopProfileContainer";
import { useTranslation } from "react-i18next";

const Root = styled.div`
  text-align: right;
`;

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

interface Props extends React.HTMLAttributes<HTMLDivElement> {}

const TopMenuContainer = (props: Props) => {
  const { currentUser, isLogin, isLoading } = useContext(SessionContext);
  const { t } = useTranslation();

  if (isLoading) {
    return <></>;
  }

  if (currentUser && isLogin) {
    return (
      <Root>
        <TopProfileContainer className="top-profile-container" userInfo={currentUser} />
      </Root>
    );
  }

  return (
    <Root>
      <List className={props.className}>
        <ListItem className="d-none d-sm-inline-block">
          <AuthButton to="/auth/signup">{t("sign_up")}</AuthButton>
        </ListItem>

        <Devided />
        <ListItem>
          <AuthButton to="/auth/login">{t("login")}</AuthButton>
        </ListItem>
      </List>
    </Root>
  );
};

export default TopMenuContainer;
