import * as React from "react";
import { LoadingBox } from "./NotificationBoxes";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { HTMLAttributes } from "react";

const LoadingBar: React.FC<HTMLAttributes<HTMLSpanElement>> = (props) => {
  const { children } = props;
  return (
    <LoadingBox>
      <FontAwesomeIcon
        icon="spinner"
        spin={true}
        className="me-2"
      ></FontAwesomeIcon>
      {children ? <span>{children}</span> : null}
    </LoadingBox>
  );
};

export default LoadingBar;
