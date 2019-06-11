import React, { Fragment } from "react";
import { Route } from "react-router-dom";

export default function({ component: Component, ...rest }) {
  return (
    <Route
      {...rest}
      render={matchProps => (
        <Fragment>
          <Component {...matchProps} />
        </Fragment>
      )}
    />
  );
}
