import React from "react";
import { LoadingBox } from "./NotificationBoxes";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

export default function (props) {
  return (
    <LoadingBox>
      <FontAwesomeIcon
        icon="spinner"
        spin={true}
        className="me-2"
      ></FontAwesomeIcon>
      <span>{props.children}</span>
    </LoadingBox>
  );
}
