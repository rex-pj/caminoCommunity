import React, { Fragment } from "react";
import { Route } from "react-router-dom";
import { ThemeProvider } from "styled-components";
import * as theme from "../../../utils/Theme";
import Notifications from "../../organisms/Notification/Notifications";
import Modal from "../../molecules/Modals/Modal";

function MasterLayout({ component: Component, ...rest }) {
  return (
    <ThemeProvider theme={theme}>
      <Route
        {...rest}
        render={matchProps => (
          <Fragment>
            <Component {...matchProps} />
            <Notifications />
            <Modal />
          </Fragment>
        )}
      />
    </ThemeProvider>
  );
}

export default MasterLayout;
