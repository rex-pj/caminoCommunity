import React from "react";
import { AdsSuggestionPanel } from "../SuggestionPanels";

export default (props) => {
  const { className, index } = props;
  let { ads } = props;

  ads = {
    ...ads,
  };
  return <AdsSuggestionPanel data={ads} className={className} index={index} />;
};
