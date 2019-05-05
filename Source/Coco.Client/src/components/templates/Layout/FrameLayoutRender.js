import React, { Fragment } from "react";
import { Header } from "../../organisms/Containers";
import { Route } from "react-router-dom";
import { ThemeProvider } from "styled-components";
import * as theme from "../../../utils/Theme";

export default ({ component: Component, ...rest }) => {
  return (
    <ThemeProvider theme={theme}>
      <Route
        {...rest}
        render={matchProps => (
          <Fragment>
            <Header />
            <Component {...matchProps} />
          </Fragment>
        )}
      />
    </ThemeProvider>
  );
};
