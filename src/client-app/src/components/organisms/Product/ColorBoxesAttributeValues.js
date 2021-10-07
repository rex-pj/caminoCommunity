import React from "react";
import styled from "styled-components";
import { HorizontalList } from "../../molecules/List";
import { adjustPrice } from "../../../utils/PriceUtils";

const ColorBox = styled.div`
  width: ${(p) => p.theme.size.normal};
  height: ${(p) => p.theme.size.normal};
  background-color: ${(p) => p.color};
`;

export default (props) => {
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
