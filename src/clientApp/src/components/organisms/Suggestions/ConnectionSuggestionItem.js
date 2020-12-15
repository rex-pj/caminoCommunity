import React from "react";
import { PeopleSuggestionPanel } from "../SuggestionPanels";

export default (props) => {
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
