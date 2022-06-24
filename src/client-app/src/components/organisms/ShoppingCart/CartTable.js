import React from "react";
import { TableResponsive, Table } from "../../atoms/Tables.js";
import styled from "styled-components";
import { Image } from "../../atoms/Images";
import { LightTextbox } from "../../atoms/Textboxes";
import {
  ButtonOutlineDanger,
  ButtonOutlineLight,
} from "../../atoms/Buttons/OutlineButtons";
import { ButtonSecondary, ButtonLight } from "../../atoms/Buttons/Buttons";
import { PanelDefault, PanelBody, PanelFooter } from "../../molecules/Panels";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

const CartTable = styled(PanelDefault)`
  ${PanelFooter} {
    text-align: right;
    margin: 0 ${(p) => p.theme.size.distance};
  }
`;

export default (function (props) {
  return (
    <CartTable>
      <PanelBody>
        <TableResponsive>
          <Table className="table table-striped table-hover">
            <thead>
              <tr>
                <th scope="col">#</th>
                <th scope="col" colSpan="2">
                  Sản phẩm
                </th>
                <th scope="col">Giá</th>
                <th scope="col">Số lượng</th>
                <th scope="col">Tổng giá</th>
                <th scope="col"></th>
              </tr>
            </thead>
            <tbody>
              <tr>
                <td width="3%">1</td>
                <td width="12%">
                  <Image
                    src={`${process.env.PUBLIC_URL}/photos/farmstay.jpg`}
                  ></Image>
                </td>
                <td width="28%">
                  <strong>
                    Donec ultricies urna ut nulla aliquam eleifend
                  </strong>
                  <ul className="attributes">
                    <li>Giống: Typica [20.010 vnđ]</li>
                    <li>Xuất xứ: Campuchia [9.900 vnđ]</li>
                  </ul>
                </td>
                <td width="15%">$245.00</td>
                <td width="10%">
                  <LightTextbox defaultValue={1} />
                </td>
                <td width="15%">$245.00</td>
                <td width="17%">
                  <ButtonOutlineDanger
                    size="xs"
                    title="Xóa khỏi giỏ hàng"
                    className="me-1"
                  >
                    <FontAwesomeIcon icon="times" />
                  </ButtonOutlineDanger>
                  <ButtonOutlineLight size="xs" title="Sửa giỏ hàng này">
                    <FontAwesomeIcon icon={["fas", "edit"]} />
                  </ButtonOutlineLight>
                </td>
              </tr>
              <tr>
                <td width="3%">2</td>
                <td width="12%">
                  <Image
                    src={`${process.env.PUBLIC_URL}/photos/farmstay.jpg`}
                  ></Image>
                </td>
                <td width="28%">
                  <strong>
                    Donec ultricies urna ut nulla aliquam eleifend
                  </strong>
                  <ul className="attributes">
                    <li>Giống: Typica [20.010 vnđ]</li>
                    <li>Xuất xứ: Campuchia [9.900 vnđ]</li>
                  </ul>
                </td>
                <td width="15%">$245.00</td>
                <td width="10%">
                  <LightTextbox defaultValue={1} />
                </td>
                <td width="15%">$245.00</td>
                <td width="17%">
                  <ButtonOutlineDanger
                    size="xs"
                    title="Xóa khỏi giỏ hàng"
                    className="me-1"
                  >
                    <FontAwesomeIcon icon="times" />
                  </ButtonOutlineDanger>
                  <ButtonOutlineLight size="xs" title="Sửa giỏ hàng này">
                    <FontAwesomeIcon icon={["fas", "edit"]} />
                  </ButtonOutlineLight>
                </td>
              </tr>
              <tr>
                <td width="3%">2</td>
                <td width="12%">
                  <Image
                    src={`${process.env.PUBLIC_URL}/photos/farmstay.jpg`}
                  ></Image>
                </td>
                <td width="28%">
                  <strong>
                    Donec ultricies urna ut nulla aliquam eleifend
                  </strong>
                  <ul className="attributes">
                    <li>Giống: Typica [20.010 vnđ]</li>
                    <li>Xuất xứ: Campuchia [9.900 vnđ]</li>
                  </ul>
                </td>
                <td width="15%">$245.00</td>
                <td width="10%">
                  <LightTextbox defaultValue={1} />
                </td>
                <td width="15%">$245.00</td>
                <td width="17%">
                  <ButtonOutlineDanger
                    size="xs"
                    title="Xóa khỏi giỏ hàng"
                    className="me-1"
                  >
                    <FontAwesomeIcon icon="times" />
                  </ButtonOutlineDanger>
                  <ButtonOutlineLight size="xs" title="Sửa giỏ hàng này">
                    <FontAwesomeIcon icon={["fas", "edit"]} />
                  </ButtonOutlineLight>
                </td>
              </tr>
            </tbody>
          </Table>
        </TableResponsive>
      </PanelBody>
      <PanelFooter>
        <ButtonSecondary size="sm" className="me-1">
          <FontAwesomeIcon className="me-1" icon="save" />
          Cập nhật giỏ hàng
        </ButtonSecondary>
        <ButtonLight size="sm">
          <FontAwesomeIcon className="me-1" icon="store" />
          Tiếp tục mua hàng
        </ButtonLight>
      </PanelFooter>
    </CartTable>
  );
});
