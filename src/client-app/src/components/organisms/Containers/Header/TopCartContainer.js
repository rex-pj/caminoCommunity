import React from "react";
import styled from "styled-components";
import { RouterLinkButtonTransparent } from "../../../atoms/Buttons/RouterLinkButtons";
import ButtonGroup from "../../../atoms/ButtonGroup";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

const Root = styled.div`
  display: inline-block;
`;

const ShoppingCartButton = styled(RouterLinkButtonTransparent)`
  color: ${(p) => p.theme.color.neutralTitle};
`;

const ShoppingCartButtonGroup = styled(ButtonGroup)`
  ${ShoppingCartButton} {
    font-size: ${(p) => p.theme.fontSize.tiny};
    padding: 6px ${(p) => p.theme.size.exTiny};
    margin: 1px 0;
    font-weight: 600;
    height: ${(p) => p.theme.size.normal};
    vertical-align: middle;
    position: relative;

    :hover {
      color: ${(p) => p.theme.color.neutralText};
    }

    .badge {
      font-size: calc(${(p) => p.theme.fontSize.tiny} - 4px);
      position: absolute;
      padding: 1px;
      width: 18px;
      height: 18px;
      line-height: 15px;
      top: 0px;
      left: 15px;
      text-align: center;
      background: ${(p) => p.theme.color.dangerBg};
      color: ${(p) => p.theme.color.neutralText};
      border-radius: ${(p) => p.theme.borderRadius.medium};
    }
  }

  .cart-icon {
    font-size: ${(p) => p.theme.fontSize.medium};
    vertical-align: middle;
  }
`;

const CartLabel = styled.span`
  vertical-align: middle;
  color: inherit;
  font-size: inherit;
`;

export default function (props) {
  return (
    <Root className={props.className}>
      <ShoppingCartButtonGroup>
        <ShoppingCartButton to={`/shopping-cart`}>
          <span className="badge">10+</span>
          <FontAwesomeIcon className="me-3 cart-icon" icon="shopping-bag" />
          <CartLabel className="d-none d-sm-none d-md-none d-lg-inline">
            Giỏ hàng
          </CartLabel>
        </ShoppingCartButton>
      </ShoppingCartButtonGroup>
    </Root>
  );
}
