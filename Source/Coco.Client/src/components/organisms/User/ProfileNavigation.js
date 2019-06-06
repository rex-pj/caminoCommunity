import React, { Component } from "react";
import { withRouter, NavLink } from "react-router-dom";
import styled from "styled-components";
import { HorizontalList } from "../../atoms/List";
import DropdownButton from "../../molecules/DropdownButton";

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
      let { pathname, search } = location;
      pathname = pathname ? pathname.replace(/\/$/, "") : pathname;
      let { url } = match;
      url = url ? url.replace(/\/$/, "") : url;
      return (
        <Root>
          <div className="row">
            <div className="col-auto mr-auto">
              <HorizontalList className={className}>
                <ListItem>
                  <NavLink
                    to={`${url}/feeds${search}`}
                    className={
                      pathname === url || pathname === `${url}/feeds`
                        ? "active"
                        : ""
                    }
                  >
                    Tất cả
                  </NavLink>
                </ListItem>
                <ListItem>
                  <NavLink
                    to={`${url}/posts${search}`}
                    className={pathname === `${url}/posts` ? "active" : ""}
                  >
                    Bài Viết
                  </NavLink>
                </ListItem>
                <ListItem>
                  <NavLink
                    to={`${url}/products${search}`}
                    className={pathname === `${url}/products` ? "active" : ""}
                  >
                    Sản Phẩm
                  </NavLink>
                </ListItem>
                <ListItem>
                  <NavLink
                    to={`${url}/farms${search}`}
                    className={pathname === `${url}/farms` ? "active" : ""}
                  >
                    Nông Trại
                  </NavLink>
                </ListItem>
                <ListItem>
                  <NavLink
                    to={`${url}/followings${search}`}
                    className={pathname === `${url}/followings` ? "active" : ""}
                  >
                    Được Theo Dõi
                  </NavLink>
                </ListItem>
                <ListItem>
                  <NavLink
                    to={`${url}/about${search}`}
                    className={pathname === `${url}/about` ? "active" : ""}
                  >
                    Giới thiệu
                  </NavLink>
                </ListItem>
              </HorizontalList>
            </div>
            <div className="col-auto">
              <DropdownButton icon="ellipsis-v" />
            </div>
          </div>
        </Root>
      );
    }
  }
);
