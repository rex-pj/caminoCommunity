import React from "react";
import { SuggestionPanel } from "../SuggestionPanels";

export default (props) => {
  const { className, index } = props;
  let { community } = props;

  community = {
    ...community,
    actionIcon: "handshake",
    infoIcon: "users",
    actionText: "Join",
  };
  return (
    <SuggestionPanel data={community} className={className} index={index} />
  );
};
