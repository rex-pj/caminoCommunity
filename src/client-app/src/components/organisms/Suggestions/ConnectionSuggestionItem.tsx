import * as React from "react";
import { PeopleSuggestionPanel } from "../SuggestionPanels";

type Props = {
  className?: string;
  index: number;
  connection?: any;
};

const ConnectionSuggestionItem = (props: Props) => {
  const { className, index } = props;
  let { connection } = props;

  connection = {
    ...connection,
    actionIcon: "user-plus",
    actionText: "Connect",
  };
  return (
    <PeopleSuggestionPanel
      data={connection}
      className={className}
      index={index}
    />
  );
};

export default ConnectionSuggestionItem;
