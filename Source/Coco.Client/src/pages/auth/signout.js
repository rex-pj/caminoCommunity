import React, { Component } from "react";
import { logOut } from "../../services/AuthService";
import { withRouter } from "react-router-dom";
import UserContext from "../../utils/Context/UserContext";
import SignOutPanel from "../../components/organisms/Auth/SignOutPanel";

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

    logOut();
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
