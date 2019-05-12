import React, { Fragment } from "react";
import { Route } from "react-router-dom";
import { ThemeProvider } from "styled-components";
import * as theme from "../../../utils/Theme";
import { connect } from "react-redux";
import Notifications from "../../organisms/Notification/Notifications";
import UserContext from "../../../utils/Context/UserContext";

const MasterLayout = ({ component: Component, ...rest }) => {
  return (
    <ThemeProvider theme={theme}>
      <Route
        {...rest}
        render={matchProps => (
          <Fragment>
            <UserContext.Provider
              value={{
                lang: "vn"
              }}
            >
              <Component {...matchProps} />
            </UserContext.Provider>

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
