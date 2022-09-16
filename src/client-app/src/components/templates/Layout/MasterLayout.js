import React, { Fragment } from "react";
import Notifications from "../../organisms/Notification/Notifications";
import Modal from "../../organisms/Modals/Modal";

/// the layout with popup and notifications
const MasterLayout = (props) => {
  const { children } = props;
  return (
    <Fragment>
      {children}
      <Notifications />
      <Modal />
    </Fragment>
  );
};

export default MasterLayout;
