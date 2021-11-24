import React from "react";
import { FontButtonItem } from "../ActionIcons";

export default (props) => {
  const { reactionNumber } = props;
  return (
    <FontButtonItem
      icon="smile-beam"
      className={props.className}
      title={reactionNumber}
    />
  );
};
