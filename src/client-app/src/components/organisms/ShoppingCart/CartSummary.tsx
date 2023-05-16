import * as React from "react";
import { Table } from "../../atoms/Tables";
import { ButtonSecondary } from "../../atoms/Buttons/Buttons";
import { PanelDefault, PanelBody, PanelFooter } from "../../molecules/Panels";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

type Props = {};

const CartSummary = (props: Props) => {
  return (
    <PanelDefault className="mt-3">
      <PanelBody>
        <Table className="cart-total">
          <tbody>
            <tr>
              <td>
                <label>Giá ban đầu:</label>
              </td>
              <td>
                <span className="value-summary">$1,315.00</span>
              </td>
            </tr>
            <tr>
              <td>
                <label>Phí vận chuyển:</label>
              </td>
              <td>
                <span className="value-summary">$0.00</span>
              </td>
            </tr>
            <tr>
              <td>
                <label>Thuế:</label>
              </td>
              <td>
                <span className="value-summary">$0.00</span>
              </td>
            </tr>
            <tr>
              <td>
                <label>Tổng giá:</label>
              </td>
              <td>
                <span className="value-summary">
                  <strong>$1,315.00</strong>
                </span>
              </td>
            </tr>
          </tbody>
        </Table>
      </PanelBody>
      <PanelFooter className="text-start">
        <ButtonSecondary size="sm">
          <FontAwesomeIcon className="me-1" icon="money-bill" />
          Thanh toán
        </ButtonSecondary>
      </PanelFooter>
    </PanelDefault>
  );
};

export default CartSummary;
