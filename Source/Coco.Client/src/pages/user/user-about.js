import React, { Component } from "react";
import { withRouter } from "react-router-dom";

export default withRouter(
  class extends Component {
    componentDidMount() {}

    render() {
      console.log(this.props);
      return <div>Đẹp trai 6 múi</div>;
    }
  }
);
