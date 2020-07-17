import React from "react";
import { SuggestionPanel } from "../../molecules/SuggestionPanels";

export default props => {
  const { className, index } = props;
  let { group } = props;

  group = {
    ...group,
    actionIcon: "handshake",
    infoIcon: "users",
    actionText: "Tham Gia"
  };
  return <SuggestionPanel data={group} className={className} index={index} />;
};
