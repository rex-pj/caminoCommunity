import * as React from "react";
import { HorizontalList } from "../../molecules/List";
import { BadgeOutlineSecondary } from "../../atoms/Badges";
import { adjustPrice } from "../../../utils/PriceUtils";
import { IProductAttributeValue } from "../../../models/productAttributesModel";

type Props = {
  attributeRelationValues?: IProductAttributeValue[];
  price?: number;
};

const HorizontalListAttributeValues = (props: Props) => {
  const { attributeRelationValues, price } = props;
  if (!attributeRelationValues) {
    return null;
  }
  return (
    <HorizontalList className="mt-2 mb-2">
      {attributeRelationValues.map((av, cIndex) => {
        return (
          <li className="me-1" key={cIndex}>
            <BadgeOutlineSecondary size="xs">
              {av.name} [{adjustPrice(av, price)}
              {" vnđ"}]
            </BadgeOutlineSecondary>
          </li>
        );
      })}
    </HorizontalList>
  );
};

export default HorizontalListAttributeValues;
