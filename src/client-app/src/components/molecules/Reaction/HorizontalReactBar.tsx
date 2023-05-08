import * as React from "react";
import { FontButtonItem, ActionIconProps } from "../ActionIcons";

interface HorizontalReactBarProps extends ActionIconProps {
  reactionNumber?: string;
}

const HorizontalReactBar: React.FC<HorizontalReactBarProps> = (props) => {
  const { reactionNumber, className } = props;
  return (
    <FontButtonItem
      icon="smile-beam"
      className={className}
      title={reactionNumber}
    />
  );
};

export default HorizontalReactBar;
