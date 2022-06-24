import React from "react";
import { useMatch } from "react-router-dom";
import styled from "styled-components";
import { RouterLinkButtonPrimary } from "../../atoms/Buttons/RouterLinkButtons";
import { HorizontalList } from "../../molecules/List";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

const Root = styled.div`
  background-color: ${(p) => p.theme.color.neutralBg};
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
    color: ${(p) => p.theme.color.neutralText};
    background-color: transparent;
  }
`;

const ListItem = styled.li`
  display: inline-block;

  &.actived ${NavButton} {
    background-color: ${(p) => p.theme.color.primaryBg};
  }

  :first-child ${NavButton} {
    border-top-left-radius: 0;
  }
`;

export default (function (props) {
  const { className } = props;
  const { pathname } = useMatch();
  return (
    <Root>
      <HorizontalList className={className}>
        <ListItem className={pathname === "/auth/login" ? "actived" : ""}>
          <NavButton to="/auth/login">Login</NavButton>
        </ListItem>
        <ListItem
          className={pathname === "/auth/forgot-password" ? "actived" : ""}
        >
          <NavButton to="/auth/forgot-password">Forgot Password</NavButton>
        </ListItem>
        <ListItem>
          <NavButton to="/">
            <FontAwesomeIcon icon="home" />
          </NavButton>
        </ListItem>
      </HorizontalList>
    </Root>
  );
});
