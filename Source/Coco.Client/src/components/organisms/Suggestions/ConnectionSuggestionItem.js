import React from "react";
import { PeopleSuggestionPanel } from "../../molecules/SuggestionPanels";

export default props => {
  const { className, index } = props;
  let { connection } = props;

  connection = {
    ...connection,
    actionIcon: "user-plus",
    actionText: "Kết Nối"
  };
  return (
    <PeopleSuggestionPanel
      data={connection}
      className={className}
      index={index}
    />
  );
};
