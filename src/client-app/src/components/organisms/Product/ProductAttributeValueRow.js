import React from "react";
import {
  ButtonOutlineDanger,
  ButtonOutlineLight,
} from "../../atoms/Buttons/OutlineButtons";
import styled from "styled-components";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { LabelNormal, LabelDark } from "../../atoms/Labels";
import { adjustPrice } from "../../../utils/PriceUtils";

const SecondaryLabel = styled(LabelNormal)`
  font-size: ${(p) => p.theme.fontSize.tiny};
`;

const ProductAttributeValueRow = (props) => {
  const {
    attributeValue,
    className,
    price,
    onEditAttributeValue,
    onRemoveAttributeValue,
  } = props;

  return (
    <div className={className}>
      <div className="col-12 col-md-12 col-lg-3">
        <div className="mb-1">
          <LabelNormal className="me-1">Tên:</LabelNormal>
          <LabelDark>{attributeValue.name}</LabelDark>
        </div>
        <div className="mb-1">
          <SecondaryLabel className="me-1">Giá sau cùng:</SecondaryLabel>
          <LabelDark>
            {adjustPrice(attributeValue, price)}
            {" vnđ"}
          </LabelDark>
        </div>
      </div>
      <div className="col-12 col-md-6 col-lg-5">
        <div className="mb-1">
          <LabelNormal className="me-1">Giá điều chỉnh:</LabelNormal>
          <LabelDark>
            $
            {attributeValue.priceAdjustment
              ? attributeValue.priceAdjustment
              : 0}
          </LabelDark>
        </div>
        <div className="mb-1">
          <SecondaryLabel className="me-1">
            Giá điều chỉnh theo phần trăm:
          </SecondaryLabel>
          <LabelDark>
            {attributeValue.pricePercentageAdjustment
              ? attributeValue.pricePercentageAdjustment
              : 0}
            %
          </LabelDark>
        </div>
      </div>
      <div className="col-12 col-md-3 col-lg-2">
        <LabelNormal className="me-1">Thứ tự:</LabelNormal>
        <LabelDark>{attributeValue.displayOrder}</LabelDark>
      </div>
      <div className="col-auto offset-auto">
        <ButtonOutlineLight
          type="button"
          size="xs"
          className="me-1"
          title="Sửa giá trị thuộc tính"
          onClick={() => onEditAttributeValue(attributeValue)}
        >
          <FontAwesomeIcon icon="pencil-alt" />
        </ButtonOutlineLight>
        <ButtonOutlineDanger
          type="button"
          size="xs"
          title="Xóa giá trị thuộc tính"
          onClick={() => onRemoveAttributeValue(attributeValue)}
        >
          <FontAwesomeIcon icon="times"></FontAwesomeIcon>
        </ButtonOutlineDanger>
      </div>
    </div>
  );
};

export default ProductAttributeValueRow;
