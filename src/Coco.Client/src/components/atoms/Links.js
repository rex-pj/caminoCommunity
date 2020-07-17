import React from "react";
import { Link, withRouter } from "react-router-dom";

const AnchorLink = withRouter(({ ...props }) => {
  const { match, children, target, to } = props;
  let { className } = props;
  let { path } = match;
  className = className ? className : "";

  path = path ? (path.split(":") ? path.split(":")[0] : path) : null;

  const toNormalized = to ? to.split("/")[1] : to;
  const pathNormalized = path.split("/")[1];

  const activedClass =
    pathNormalized === toNormalized ? `${className}${" actived"}` : className;
  return (
    <Link to={to} className={`${activedClass}`} target={target}>
      {children}
    </Link>
  );
});

export { AnchorLink };
