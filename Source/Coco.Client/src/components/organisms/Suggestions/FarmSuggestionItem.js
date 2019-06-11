import React from "react";
import { SuggestionPanel } from "../../molecules/SuggestionPanels";

export default function (props) {
  const { className, index } = props;
  let { farm } = props;

  farm = {
    ...farm,
    actionIcon: "user-plus",
    infoIcon: "map-marker-alt",
    actionText: "Theo dõi"
  };
  return <SuggestionPanel data={farm} className={className} index={index} />;
};
