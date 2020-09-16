import React from "react";
import { SuggestionPanel } from "../../molecules/SuggestionPanels";

export default (props) => {
  const { className, index } = props;
  let { association } = props;

  association = {
    ...association,
    actionIcon: "handshake",
    infoIcon: "users",
    actionText: "Join",
  };
  return (
    <SuggestionPanel data={association} className={className} index={index} />
  );
};
