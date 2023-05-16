import * as React from "react";
import { SelectionSecondary } from "../../atoms/Selections";
import { adjustPrice } from "../../../utils/PriceUtils";
import { IProductAttributeValue } from "../../../models/productAttributesModel";

type Props = {
  attributeRelationValues?: IProductAttributeValue[];
  price?: number;
};

const DropdownListAttributeValues = (props: Props) => {
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

export default DropdownListAttributeValues;
