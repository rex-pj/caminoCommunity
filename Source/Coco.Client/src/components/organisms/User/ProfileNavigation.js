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

  a.actived {
    color: ${p => p.theme.color.warning};
    text-decoration: none;
  }

  :hover a {
    color: ${p => p.theme.color.warning};
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

const NavLinkActived = props => {
  const { location, children, userId } = props;
  let { pageNav } = props;
  let { pathname } = location;
  pathname = pathname ? pathname.replace(/\/$/, "") : pathname;

  pageNav = pageNav ? `/${pageNav}` : "";
  const baseUrl = "/profile";

  return (
    <NavLink
      to={`${baseUrl}/${userId}${pageNav}`}
      className={pathname === `${baseUrl}/${userId}${pageNav}` ? "actived" : ""}
    >
      {children}
    </NavLink>
  );
};

export default withRouter(
  class extends Component {
    render() {
      const { className, userId } = this.props;
      return (
        <Root>
          <div className="row">
            <div className="col-auto mr-auto">
              <HorizontalList className={className}>
                <ListItem>
                  <NavLinkActived {...this.props} userId={userId}>
                    Tất cả
                  </NavLinkActived>
                </ListItem>
                <ListItem>
                  <NavLinkActived
                    pageNav="posts"
                    {...this.props}
                    userId={userId}
                  >
                    Bài Viết
                  </NavLinkActived>
                </ListItem>
                <ListItem>
                  <NavLinkActived
                    pageNav="products"
                    {...this.props}
                    userId={userId}
                  >
                    Sản Phẩm
                  </NavLinkActived>
                </ListItem>
                <ListItem>
                  <NavLinkActived
                    pageNav="farms"
                    {...this.props}
                    userId={userId}
                  >
                    Nông Trại
                  </NavLinkActived>
                </ListItem>
                <ListItem>
                  <NavLinkActived
                    pageNav="followings"
                    {...this.props}
                    userId={userId}
                  >
                    Được Theo Dõi
                  </NavLinkActived>
                </ListItem>
                <ListItem>
                  <NavLinkActived
                    pageNav="about"
                    {...this.props}
                    userId={userId}
                  >
                    Giới thiệu
                  </NavLinkActived>
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
