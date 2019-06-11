import React, { Fragment } from "react";
import { Route } from "react-router-dom";
import { ThemeProvider } from "styled-components";
import * as theme from "../../../utils/Theme";
import { connect } from "react-redux";
import loadable from "@loadable/component";

const Notifications = loadable(() =>
  import("../../organisms/Notification/Notifications")
);

const MasterLayout = ({ component: Component, ...rest }) => {
  return (
    <ThemeProvider theme={theme}>
      <Route
        {...rest}
        render={matchProps => (
          <Fragment>
            <Component {...matchProps} />
            <Notifications notify={rest.notify} />
          </Fragment>
        )}
      />
    </ThemeProvider>
  );
};

const mapStateToProps = state => {
  return {
    notify: state.notifyRdc.notify
  };
};

export default connect(mapStateToProps)(MasterLayout);
