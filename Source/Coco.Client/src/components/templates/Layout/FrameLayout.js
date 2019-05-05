import React, { Fragment } from "react";
import MasterLayout from "./MasterLayout";
import { Header } from "../../organisms/Containers";

export default ({ component: Component, ...rest }) => {
  return (
    <MasterLayout
      {...rest}
      component={matchProps => (
        <Fragment>
          <Header />
          <Component {...matchProps} />
        </Fragment>
      )}
    />
  );
};
