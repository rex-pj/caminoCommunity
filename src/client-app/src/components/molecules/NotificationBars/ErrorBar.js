import React from "react";
import { ErrorBox } from "./NotificationBoxes";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

const ErrorBar = (props) => {
  return (
    <ErrorBox>
      <FontAwesomeIcon
        icon="exclamation-triangle"
        className="me-2"
      ></FontAwesomeIcon>
      <span>{props.children}</span>
    </ErrorBox>
  );
};

export default ErrorBar;
