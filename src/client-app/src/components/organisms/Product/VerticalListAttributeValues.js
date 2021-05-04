import React from "react";
import { VerticalList } from "../../atoms/List";
import { BadgeOutlinePrimary } from "../../atoms/badges";
import { adjustPrice } from "../../../utils/PriceUtils";

export default (props) => {
  const { attributeRelationValues, price } = props;
  if (!attributeRelationValues) {
    return null;
  }
  return (
    <VerticalList className="mt-2 mb-2">
      {attributeRelationValues.map((av, cIndex) => {
        return (
          <li className="mb-1" key={cIndex}>
            <BadgeOutlinePrimary size="xs">
              {av.name} [{adjustPrice(av, price)}
              {" vnđ"}]
            </BadgeOutlinePrimary>
          </li>
        );
      })}
    </VerticalList>
  );
};
