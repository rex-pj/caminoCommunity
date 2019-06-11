import React from "react";
import { Link, withRouter } from "react-router-dom";

const AnchorLink = withRouter(function ({ ...props }) {
  const { match, children, target, to } = props;
  let { className } = props;
  let { path } = match;
  className = className ? className : "";

  path = path ? (path.split(":") ? path.split(":")[0] : path) : null;

  const activedClass =
    path === to || path === `${to}/` ? `${className}${" actived"}` : className;
  return (
    <Link to={to} className={`${activedClass}`} target={target}>
      {children}
    </Link>
  );
})

export { AnchorLink };
