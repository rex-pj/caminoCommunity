import * as React from "react";
import { LinkProps, NavLink, useLocation } from "react-router-dom";

interface ProfileNavLinkProps extends LinkProps {
  userId?: string;
  pageNav?: string;
  baseUrl?: string;
}

const ProfileNavLink: React.FC<ProfileNavLinkProps> = (props) => {
  const { children, userId, baseUrl } = props;
  let { pageNav } = props;
  let { pathname } = useLocation();
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
};

export default ProfileNavLink;
