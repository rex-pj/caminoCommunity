import * as React from "react";
import { VerticalList } from "../../molecules/List";
import { LabelSecondary } from "../../atoms/Labels";
import { adjustPrice } from "../../../utils/PriceUtils";

const CheckBoxesAttributeValues = (props) => {
  const { attributeRelationValues, attribute, price } = props;
  if (!attributeRelationValues) {
    return null;
  }
  return (
    <VerticalList className="mt-2 mb-2">
      {attributeRelationValues.map((av, cIndex) => {
        return (
          <li className="mb-1" key={cIndex}>
            <input
              type="checkbox"
              name={`attribute-${attribute.id}`}
              id={`chkAttributeValue-${av.id}`}
              value={av.id}
              className="me-1"
            ></input>
            <LabelSecondary htmlFor={`chkAttributeValue-${av.id}`}>
              {av.name} [{adjustPrice(av, price)}
              {" vnđ"}]
            </LabelSecondary>
          </li>
        );
      })}
    </VerticalList>
  );
};

export default CheckBoxesAttributeValues;
