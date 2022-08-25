import React from "react";
import { AdsSuggestionPanel } from "../SuggestionPanels";

const AdsItem = (props) => {
  const { className, index } = props;
  let { ads } = props;

  ads = {
    ...ads,
  };
  return <AdsSuggestionPanel data={ads} className={className} index={index} />;
};

export default AdsItem;
