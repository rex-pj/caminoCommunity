import * as React from "react";
import styled from "styled-components";
import { formatPrice } from "../../utils/PriceUtils";
import { HTMLAttributes } from "react";

const Root = styled.p`
  margin-bottom: 0;
`;

const PriceNumber = styled.span`
  color: ${(p) => p.theme.color.secondaryText};
  font-weight: 600;
  font-size: ${(p) => p.theme.fontSize.medium};
  margin-bottom: ${(p) => p.theme.size.tiny};
  margin-right: ${(p) => p.theme.size.exTiny};
`;

const CurrencyText = styled.span`
  color: ${(p) => p.theme.color.secondaryText};
  font-size: 14px;
  vertical-align: super;
`;

interface PriceLabelProps extends HTMLAttributes<HTMLDivElement> {
  currency?: string;
  price: number | undefined | null;
}

function PriceLabel(props: PriceLabelProps) {
  const { currency, className } = props;
  let { price } = props;
  if (!price) {
    price = 0;
  }

  const priceFormatted = formatPrice(price);

  return (
    <Root className={className}>
      <PriceNumber>{priceFormatted}</PriceNumber>
      <CurrencyText>{currency}</CurrencyText>
    </Root>
  );
}

export { PriceLabel };
