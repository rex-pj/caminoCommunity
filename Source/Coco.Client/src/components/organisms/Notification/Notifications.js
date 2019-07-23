import React, { Component } from "react";
import styled from "styled-components";
import NotifyItem from "./NotifyItem";
import { connect } from "react-redux";
import * as actionTypes from "../../../store/notifyActions";

const Root = styled.div`
  position: absolute;
  right: ${p => p.theme.size.distance};
  top: ${p => p.theme.size.distance};
  z-index: 100;
`;

class Notifications extends Component {
  constructor(props) {
    super(props);

    this.state = {
      notifications: []
    };
  }

  componentWillReceiveProps(nextProps) {
    if (nextProps.notify) {
      let notifications = [...this.state.notifications];
      notifications.push(nextProps.notify);

      this.setState({
        notifications: notifications
      });

      this.props.dispatch({
        type: actionTypes.UNNOTIFICATION
      });
    }
  }

  closePopup = notify => {
    let notifications = [...this.state.notifications];
    notifications = notifications.filter(item => item !== notify);

    this.setState({
      notifications: notifications
    });

    this.props.dispatch({
      type: actionTypes.UNNOTIFICATION
    });
  };

  closeLatestPopup = () => {
    let notifications = [...this.state.notifications];
    notifications.splice(0, 1);

    this.setState({
      notifications: notifications
    });
  };

  render() {
    const { notifications } = this.state;
    return (
      <Root>
        {notifications
          ? notifications.map((item, index) => {
              return (
                <NotifyItem
                  key={index}
                  closePopup={this.closePopup}
                  closeLatestPopup={this.closeLatestPopup}
                  notify={item}
                />
              );
            })
          : null}
      </Root>
    );
  }
}

const mapStateToProps = state => {
  return {
    notify: state.notifyRdc.notify
  };
};

export default connect(mapStateToProps)(Notifications);
