import React from "react";
import { withRouter, NavLink } from "react-router-dom";

const ProfileNavLink = props => {
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

export default withRouter(ProfileNavLink);
