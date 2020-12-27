import React from "react";
import { NavLink, withRouter } from "react-router-dom";

export default withRouter((props) => {
  const { location, children, userId, baseUrl } = props;
  let { pageNav } = props;
  let { pathname } = location;
  pathname = pathname ? pathname.replace(/\/$/, "") : pathname;

  pageNav = pageNav ? `/${pageNav}` : "";

  let activedClass = "";
  if (pageNav) {
    activedClass =
      pathname.indexOf(`${baseUrl}/${userId}${pageNav}`) >= 0 ? "actived" : "";
  } else {
    activedClass =
      pathname === `${baseUrl}/${userId}${pageNav}` ? "actived" : "";
  }
  return (
    <NavLink to={`${baseUrl}/${userId}${pageNav}`} className={activedClass}>
      {children}
    </NavLink>
  );
});
