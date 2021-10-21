import React from "react";
import { NoDataBox } from "./NotificationBoxes";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

export default function (props) {
  return (
    <NoDataBox>
      <FontAwesomeIcon icon="ghost" className="me-2"></FontAwesomeIcon>
      <span>{props.children}</span>
    </NoDataBox>
  );
}
