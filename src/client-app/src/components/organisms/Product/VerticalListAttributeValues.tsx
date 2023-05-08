import * as React from "react";
import { VerticalList } from "../../molecules/List";
import { BadgeOutlineSecondary } from "../../atoms/Badges";
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
            <BadgeOutlineSecondary size="xs">
              {av.name} [{adjustPrice(av, price)}
              {" vnđ"}]
            </BadgeOutlineSecondary>
          </li>
        );
      })}
    </VerticalList>
  );
};
