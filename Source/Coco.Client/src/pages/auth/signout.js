import React, { Component } from "react";
import { logOut } from "../../services/AuthService";
import UserContext from "../../utils/Context/UserContext";

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
  }

  componentWillUnmount() {
    this._isMounted = false;
    clearTimeout(this._timeOut);
  }
  // #endregion Life Cycle

  render() {
    return <div>Logout Page</div>;
  }
}

SingnOutPage.contextType = UserContext;

export default SingnOutPage;
