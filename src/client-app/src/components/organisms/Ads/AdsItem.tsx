import * as React from "react";
import { AdsSuggestionData, AdsSuggestionPanel } from "../SuggestionPanels";
import { HTMLAttributes } from "react";

interface AdsItemProps extends HTMLAttributes<HTMLLIElement> {
  index: number;
  ads?: AdsSuggestionData;
}

const AdsItem: React.FC<AdsItemProps> = (props) => {
  const { className, index } = props;
  const { ads } = props;
  return <AdsSuggestionPanel data={ads} className={className} index={index} />;
};

export default AdsItem;
