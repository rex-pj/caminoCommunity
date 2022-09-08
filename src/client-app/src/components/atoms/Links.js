import React from "react";
import { Link, useLocation } from "react-router-dom";

const AnchorLink = ({ ...props }) => {
  const { children, target, to, className } = props;
  const { pathname } = useLocation();
  const currentPath = pathname
    ? pathname.split(":")
      ? pathname.split(":")[0]
      : pathname
    : null;

  let toNormalized = "";
  if (to && typeof to === "string") {
    toNormalized = to ? to.split("/")[1] : to;
  } else if (to && to.pathname) {
    toNormalized = to.pathname ? to.pathname.split("/")[1] : to.pathname;
  }

  let activedClass = className;
  const isCurrentPath = currentPath.split("/")[1] === toNormalized;
  if (className && isCurrentPath) {
    activedClass = `${className}${" actived"}`;
  } else if (isCurrentPath) {
    activedClass = "actived";
  } else if (!className) {
    activedClass = "";
  }

  return (
    <Link to={to} className={activedClass} target={target}>
      {children}
    </Link>
  );
};

export { AnchorLink };
