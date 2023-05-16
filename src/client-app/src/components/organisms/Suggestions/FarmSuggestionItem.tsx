import * as React from "react";
import { SuggestionPanel } from "../SuggestionPanels";

type Props = {
  className?: string;
  index: number;
  farm?: any;
};

const FarmSuggestionItem = (props: Props) => {
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

export default FarmSuggestionItem;
