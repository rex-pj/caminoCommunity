import React from "react";
import { SuggestionPanel } from "../../molecules/SuggestionPanels";

export default (props) => {
  const { className, index } = props;
  let { farm } = props;

  farm = {
    ...farm,
    actionIcon: "user-plus",
    infoIcon: "map-marker-alt",
    actionText: "Follow",
  };
  return <SuggestionPanel data={farm} className={className} index={index} />;
};
