import React from "react";
import { FrameLayout } from "../../components/templates/Layout";
import { withRouter } from "react-router-dom";

export default withRouter(function (props) {
  return (
    <FrameLayout>
      <div className="container">Shopping cart</div>
    </FrameLayout>
  );
});
