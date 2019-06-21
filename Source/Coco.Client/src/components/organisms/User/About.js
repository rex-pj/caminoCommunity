import React, { Component } from "react";
import { PanelBody, PanelFooter } from "../../../components/atoms/Panels";

export default class extends Component {
  constructor(props) {
    super(props);
    this._isMounted = false;
  }

  // #region Life Cycle
  componentDidMount() {
    this._isMounted = true;
  }

  componentWillUnmount() {
    this._isMounted = false;
  }
  // #endregion Life Cycle

  render() {
    const { userInfo } = this.props;

    console.log(userInfo);
    return (
      <form onSubmit={e => this.onUpdate(e)} method="POST">
        <PanelBody />
        <PanelFooter />
      </form>
    );
  }
}
