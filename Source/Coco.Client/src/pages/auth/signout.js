import React, { Component } from "react";
import AuthService from "../../services/AuthService";
import { withRouter } from "react-router-dom";
import SignOutPanel from "../../components/organisms/Auth/SignOutPanel";

class SingnOutPage extends Component {
  constructor(props) {
    super(props);
    this._isMounted = false;

    this.state = {
      isFormEnabled: false,
      shoudRedirect: false
    };
  }

  // #region Life Cycle
  async componentDidMount() {
    this._isMounted = true;

    AuthService.logOut();

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

export default withRouter(SingnOutPage);
