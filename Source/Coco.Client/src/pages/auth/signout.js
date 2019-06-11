import React, { Component } from "react";
import loadable from "@loadable/component";
import AuthService from "../../services/AuthService";
import { withRouter } from "react-router-dom";
import UserContext from "../../utils/Context/UserContext";
const SignOutPanel = loadable(() =>
  import("../../components/organisms/Auth/SignOutPanel")
);

class SingnOutPage extends Component {
  constructor(props) {
    super(props);
    this._isMounted = false;

    this.state = {
      isFormEnabled: false
    };
  }

  // #region Life Cycle
  componentDidMount() {
    this._isMounted = true;

    AuthService.logOut();
    this.context.logout();

    const { history } = this.props;
    setTimeout(function() {
      history.push("/");
    }, 1000);
  }

  componentWillUnmount() {
    this._isMounted = false;
    clearTimeout();
  }
  // #endregion Life Cycle

  render() {
    return <SignOutPanel />;
  }
}

SingnOutPage.contextType = UserContext;

export default withRouter(SingnOutPage);
