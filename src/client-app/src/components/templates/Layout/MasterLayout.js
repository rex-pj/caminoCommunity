import React, { Fragment } from "react";

import Notifications from "../../organisms/Notification/Notifications";
import Modal from "../../organisms/Modals/Modal";

/// the layout with popup and notifications
export default ({ component: Component, ...props }) => {
  return (
    <Fragment>
      <Component {...props} />
      <Notifications />
      <Modal />
    </Fragment>
  );
};
