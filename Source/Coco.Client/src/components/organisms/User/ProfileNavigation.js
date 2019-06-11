import React from "react";
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
  const { match, location, children } = props;
  let { pageNav } = props;
  let { pathname, search } = location;
  pathname = pathname ? pathname.replace(/\/$/, "") : pathname;
  let { url } = match;

  pageNav = pageNav ? `/${pageNav}` : "";

  return (
    <NavLink
      to={`${url}${pageNav}${search}`}
      className={pathname === `${url}${pageNav}` ? "actived" : ""}
    >
      {children}
    </NavLink>
  );
};

export default withRouter(function (props) {
  const { className } = props;
  return (
    <Root>
      <div className="row">
        <div className="col-auto mr-auto">
          <HorizontalList className={className}>
            <ListItem>
              <NavLinkActived {...this.props}>Tất cả</NavLinkActived>
            </ListItem>
            <ListItem>
              <NavLinkActived pageNav="posts" {...this.props}>
                Bài Viết
              </NavLinkActived>
            </ListItem>
            <ListItem>
              <NavLinkActived pageNav="products" {...this.props}>
                Sản Phẩm
              </NavLinkActived>
            </ListItem>
            <ListItem>
              <NavLinkActived pageNav="farms" {...this.props}>
                Nông Trại
              </NavLinkActived>
            </ListItem>
            <ListItem>
              <NavLinkActived pageNav="followings" {...this.props}>
                Được Theo Dõi
              </NavLinkActived>
            </ListItem>
            <ListItem>
              <NavLinkActived pageNav="about" {...this.props}>
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
);
