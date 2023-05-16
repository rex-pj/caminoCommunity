import * as React from "react";
import { PrimaryTitle } from "../../components/atoms/Titles";
import styled from "styled-components";
import { LightTextbox } from "../../components/atoms/Textboxes";
import { PanelFooter } from "../../components/molecules/Panels";
import {} from "@fortawesome/react-fontawesome";
import CartTable from "../../components/organisms/ShoppingCart/CartTable";
import CartSummary from "../../components/organisms/ShoppingCart/CartSummary";
import { BodyLayout } from "../../components/templates/Layout";

const Wrapper = styled.div`
  ${LightTextbox} {
    max-width: 50px;
  }

  .attributes {
    padding-left: ${(p) => p.theme.size.distance};
    font-size: ${(p) => p.theme.fontSize.small};

    li {
      color: ${(p) => p.theme.color.secondaryText};
    }
  }

  .cart-table ${PanelFooter} {
    text-align: right;
    margin: 0 ${(p) => p.theme.size.distance};
  }

  .cart-total td {
    background-color: transparent;
  }
`;

const Title = styled(PrimaryTitle)`
  color: ${(p) => p.theme.color.darkText};
`;
type Props = {};

const Index = (props: Props) => {
  return (
    <BodyLayout>
      <Wrapper className="container">
        <Title className="mb-3">Giỏ hàng</Title>
        <CartTable />
        <div className="row">
          <div className="col-sm-12 col-md-6">
            <CartSummary />
          </div>
          <div className="col-sm-12 col-md-6"></div>
        </div>
      </Wrapper>
    </BodyLayout>
  );
};

export default Index;
