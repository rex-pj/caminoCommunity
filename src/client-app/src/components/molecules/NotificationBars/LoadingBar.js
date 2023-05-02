import React from "react";
import { LoadingBox } from "./NotificationBoxes";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

const LoadingBar = (props) => {
  return (
    <LoadingBox>
      <FontAwesomeIcon
        icon="spinner"
        spin={true}
        className="me-2"
      ></FontAwesomeIcon>
      {props.children ? <span>{props.children}</span> : null}
    </LoadingBox>
  );
};

export default LoadingBar;
