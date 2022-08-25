import React from "react";
import { FontButtonItem } from "../ActionIcons";

const HorizontalReactBar = (props) => {
  const { reactionNumber } = props;
  return (
    <FontButtonItem
      icon="smile-beam"
      className={props.className}
      title={reactionNumber}
    />
  );
};

export default HorizontalReactBar;
