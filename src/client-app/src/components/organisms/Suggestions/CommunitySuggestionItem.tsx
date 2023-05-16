import * as React from "react";
import { SuggestionPanel } from "../SuggestionPanels";

type Props = {
  className?: string;
  index: number;
  community?: any;
};

const CommunitySuggestionItem = (props: Props) => {
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

export default CommunitySuggestionItem;
