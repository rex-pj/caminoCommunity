import React from "react";
import { ErrorBox } from "./NotificationBoxes";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

export default function (props) {
  return (
    <ErrorBox>
      <FontAwesomeIcon
        icon="exclamation-triangle"
        className="me-2"
      ></FontAwesomeIcon>
      <span>{props.children}</span>
    </ErrorBox>
  );
}
