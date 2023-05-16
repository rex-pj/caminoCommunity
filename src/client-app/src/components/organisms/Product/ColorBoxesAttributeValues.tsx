import * as React from "react";
import styled from "styled-components";
import { HorizontalList } from "../../molecules/List";
import { adjustPrice } from "../../../utils/PriceUtils";
import { IProductAttributeValue } from "../../../models/productAttributesModel";

const ColorBox = styled.div`
  width: ${(p) => p.theme.size.normal};
  height: ${(p) => p.theme.size.normal};
  background-color: ${(p) => p.color};
`;

type Props = {
  attributeRelationValues?: IProductAttributeValue[];
  price?: number;
};

const ColorBoxesAttributeValues = (props: Props) => {
  const { attributeRelationValues, price } = props;
  if (!attributeRelationValues) {
    return null;
  }
  return (
    <HorizontalList className="mt-2 mb-2">
      {attributeRelationValues.map((av, cIndex) => {
        return (
          <li className="me-2" key={cIndex}>
            <ColorBox color={av.name}>
              {adjustPrice(av, price)}
              {" vnđ"}
            </ColorBox>
          </li>
        );
      })}
    </HorizontalList>
  );
};

export default ColorBoxesAttributeValues;
