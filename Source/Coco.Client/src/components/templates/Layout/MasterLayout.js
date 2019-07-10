import React, { Fragment } from "react";
import { Route } from "react-router-dom";
import { ThemeProvider } from "styled-components";
import * as theme from "../../../utils/Theme";
import { connect } from "react-redux";
import Notifications from "../../organisms/Notification/Notifications";
import Modal from "../../molecules/Modals/Modal";
import { modalPushData } from "../../../store/commands";

function MasterLayout({ component: Component, ...rest }) {
  return (
    <ThemeProvider theme={theme}>
      <Route
        {...rest}
        render={matchProps => (
          <Fragment>
            <Component {...matchProps} />
            <Notifications notify={rest.notify} />
            <Modal options={rest.modal} pushData={rest.openUploadModal} />
          </Fragment>
        )}
      />
    </ThemeProvider>
  );
}

const mapDispatchToProps = dispatch => {
  return {
    openUploadModal: data => {
      modalPushData(dispatch, data);
    }
  };
};

const mapStateToProps = state => {
  return {
    notify: state.notifyRdc.notify,
    modal: state.modalReducer.options
  };
};

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(MasterLayout);
