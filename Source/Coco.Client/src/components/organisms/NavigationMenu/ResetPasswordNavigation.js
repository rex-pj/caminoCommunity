import React, { Component } from "react";
import { withRouter } from "react-router-dom";
import styled from "styled-components";
import { RouterLinkButton } from "../../atoms/RouterLinkButtons";
import { HorizontalList } from "../../atoms/List";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

const Root = styled.div`
  background-color: ${p => p.theme.color.primaryLight};
  position: relative;
`;

const NavButton = styled(RouterLinkButton)`
  color: ${p => p.theme.color.neutral};
  font-weight: 500;
  font-size: ${p => p.theme.fontSize.small};
  border: 0;
  border-top-right-radius: ${p => p.theme.borderRadius.medium};
  border-top-left-radius: ${p => p.theme.borderRadius.medium};
  border-bottom-left-radius: 0;
  border-bottom-right-radius: 0;
  background-color: transparent;

  :hover {
    color: ${p => p.theme.color.light};
    background-color: transparent;
  }
`;

const ListItem = styled.li`
  display: inline-block;

  &.actived ${NavButton} {
    background-color: ${p => p.theme.color.primary};
  }

  &.actived span {
    display: block;
    color: ${p => p.theme.color.lighter};
    background-color: ${p => p.theme.color.primary};
    font-weight: 500;
    font-size: ${p => p.theme.fontSize.small};
    border: 0;
    border-top-right-radius: ${p => p.theme.borderRadius.medium};
    border-top-left-radius: ${p => p.theme.borderRadius.medium};
    border-bottom-left-radius: 0;
    border-bottom-right-radius: 0;
    padding: ${p => (p.size === "sm" ? ".5rem .75rem" : "10px 15px")};
  }

  :first-child ${NavButton} {
    border-top-left-radius: 0;
  }
`;

export default withRouter(
  class extends Component {
    render() {
      const { className } = this.props;
      return (
        <Root>
          <HorizontalList className={className}>
            <ListItem>
              <NavButton to="/">
                <FontAwesomeIcon icon="home" />
              </NavButton>
            </ListItem>
            <ListItem className="actived">
              <span>Quên mật khẩu</span>
            </ListItem>
          </HorizontalList>
        </Root>
      );
    }
  }
);
