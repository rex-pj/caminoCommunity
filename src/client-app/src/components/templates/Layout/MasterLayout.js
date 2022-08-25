import React, { Fragment } from "react";

import Notifications from "../../organisms/Notification/Notifications";
import Modal from "../../organisms/Modals/Modal";

/// the layout with popup and notifications
export default ({ children }) => {
  return (
    <Fragment>
      {children}
      <Notifications />
      <Modal />
    </Fragment>
  );
};
