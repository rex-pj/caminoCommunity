import * as React from "react";
import { useLocation } from "react-router-dom";
import styled from "styled-components";
import { RouterLinkButtonPrimary } from "../../atoms/Buttons/RouterLinkButtons";
import { HorizontalList } from "../../molecules/List";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { useTranslation } from "react-i18next";

const Root = styled.div`
  background-color: ${(p) => p.theme.color.secondaryBg};
  position: relative;
`;

const NavButton = styled(RouterLinkButtonPrimary)`
  color: ${(p) => p.theme.color.neutralText};
  font-weight: 500;
  font-size: ${(p) => p.theme.fontSize.small};
  border: 0;
  border-top-right-radius: ${(p) => p.theme.borderRadius.medium};
  border-top-left-radius: ${(p) => p.theme.borderRadius.medium};
  border-bottom-left-radius: 0;
  border-bottom-right-radius: 0;
  background-color: transparent;

  :hover {
    color: ${(p) => p.theme.color.lightText};
    background-color: transparent;
  }
`;

const ListItem = styled.li`
  display: inline-block;

  &.actived ${NavButton} {
    background-color: ${(p) => p.theme.color.primaryBg};
    color: ${(p) => p.theme.color.whiteText};
    font-weight: 600;
  }

  :first-child ${NavButton} {
    border-top-left-radius: 0;
  }
`;

interface AuthNavigationProps {
  className?: string;
}

const AuthNavigation = (props: AuthNavigationProps) => {
  const { t } = useTranslation();
  const { pathname } = useLocation();
  const { className } = props;
  return (
    <Root>
      <HorizontalList className={className}>
        <ListItem className={pathname === "/auth/signup" ? "actived" : ""}>
          <NavButton to="/auth/signup">{t("sign_up")}</NavButton>
        </ListItem>
        <ListItem className={pathname === "/auth/login" ? "actived" : ""}>
          <NavButton to="/auth/login">{t("login")}</NavButton>
        </ListItem>
        <ListItem>
          <NavButton to="/">
            <FontAwesomeIcon icon="home" />
          </NavButton>
        </ListItem>
      </HorizontalList>
    </Root>
  );
};

export default AuthNavigation;
