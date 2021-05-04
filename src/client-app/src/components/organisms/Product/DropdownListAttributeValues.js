import React from "react";
import { SelectionSecondary } from "../../atoms/Selections";
import { adjustPrice } from "../../../utils/PriceUtils";

export default (props) => {
  const { attributeRelationValues, price } = props;
  if (!attributeRelationValues) {
    return null;
  }
  return (
    <SelectionSecondary className="mt-2 mb-2">
      {attributeRelationValues.map((av, cIndex) => {
        return (
          <option key={cIndex}>
            {av.name} [{adjustPrice(av, price)}
            {" vnđ"}]
          </option>
        );
      })}
    </SelectionSecondary>
  );
};
