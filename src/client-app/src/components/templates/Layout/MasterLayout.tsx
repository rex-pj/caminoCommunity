import * as React from "react";
import { Fragment } from "react";
import Notifications from "../../organisms/Notification/Notifications";
import Modal from "../../organisms/Modals/Modal";

interface Props {
  children?: any;
  component?: any;
}

/// the layout with popup and notifications
const MasterLayout = (props: Props) => {
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
