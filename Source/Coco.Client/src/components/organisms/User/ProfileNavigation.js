import React, { Component } from "react";
import { withRouter, NavLink } from "react-router-dom";
import styled from "styled-components";
import { HorizontalList } from "../../atoms/List";

const Root = styled.div`
  position: relative;
  padding-left: 135px;
`;

const ListItem = styled.li`
  display: inline-block;
  margin: 0 ${p => p.theme.size.distance};

  a.active,
  &:hover a.active {
    color: ${p => p.theme.color.warning};
    text-decoration: none;
  }

  :hover a {
    color: ${p => p.theme.color.brown};
    text-decoration: none;
  }

  a {
    color: ${p => p.theme.color.normal};
    font-weight: 500;
    font-size: ${p => p.theme.fontSize.small};
    border: 0;
    height: ${p => p.theme.size.medium};
    line-height: ${p => p.theme.size.medium};
  }
`;

export default withRouter(
  class extends Component {
    render() {
      const { match, className, location } = this.props;
      let { pathname } = location;
      pathname = pathname ? pathname.replace(/\/$/, "") : pathname;
      let { url } = match;
      url = url ? url.replace(/\/$/, "") : url;
      return (
        <Root>
          <HorizontalList className={className}>
            <ListItem>
              <NavLink
                to={`${url}/feeds`}
                className={pathname === url ? "active" : ""}
              >
                Tất cả
              </NavLink>
            </ListItem>
            <ListItem>
              <NavLink to={`${url}/posts`}>Bài Viết</NavLink>
            </ListItem>
            <ListItem>
              <NavLink to={`${url}/products`}>Sản Phẩm</NavLink>
            </ListItem>
            <ListItem>
              <NavLink to={`${url}/farms`}>Nông Trại</NavLink>
            </ListItem>
            <ListItem>
              <NavLink to={`${url}/followings`}>Được Theo Dõi</NavLink>
            </ListItem>
            <ListItem>
              <NavLink to={`${url}/about`}>Giới thiệu</NavLink>
            </ListItem>
          </HorizontalList>
        </Root>
      );
    }
  }
);
