import React, { Component, Fragment } from "react";
import { withRouter } from "react-router-dom";

export default withRouter(
  class extends Component {
    render() {
      return (
        <Fragment>
          <div>404 NOT FOUND</div>
        </Fragment>
      );
    }
  }
);
