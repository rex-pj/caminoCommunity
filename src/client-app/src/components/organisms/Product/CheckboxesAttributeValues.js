import React from "react";
import { VerticalList } from "../../molecules/List";
import { LabelPrimary } from "../../atoms/Labels";
import { adjustPrice } from "../../../utils/PriceUtils";

export default (props) => {
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
            <LabelPrimary htmlFor={`chkAttributeValue-${av.id}`}>
              {av.name} [{adjustPrice(av, price)}
              {" vnđ"}]
            </LabelPrimary>
          </li>
        );
      })}
    </VerticalList>
  );
};
