import * as React from "react";
import {
  ButtonOutlineDanger,
  ButtonOutlineLight,
} from "../../atoms/Buttons/OutlineButtons";
import styled from "styled-components";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { LabelSecondary, LabelDark } from "../../atoms/Labels";
import { adjustPrice } from "../../../utils/PriceUtils";
import { IProductAttributeValue } from "../../../models/productAttributesModel";
import ProductAttributeValueEditModal from "./ProductAttributeValueEditModal";
import { useStore } from "../../../store/hook-store";

const SecondaryLabel = styled(LabelSecondary)`
  font-size: ${(p) => p.theme.fontSize.tiny};
`;

type Props = {
  className?: string;
  attributeValue: IProductAttributeValue;
  onChange: (
    attributeValue: IProductAttributeValue,
    event: "added" | "updated" | "removed"
  ) => void;
  onRemoveAttributeValue: (attributeValue: IProductAttributeValue) => void;
  price: number;
};

const ProductAttributeValueRow = (props: Props) => {
  const dispatch = useStore(true)[1];
  const { attributeValue, className, price, onRemoveAttributeValue } = props;

  function onOpenEditAttributeValueModal(
    currentAttributeValue: IProductAttributeValue
  ) {
    dispatch("OPEN_MODAL", {
      data: {
        attributeValue: currentAttributeValue,
        title: "Sửa giá trị của thuộc tính sản phẩm",
      },
      execution: {
        onEditAttributeValue: onUpdateAttributeValue,
      },
      options: {
        isOpen: true,
        innerModal: ProductAttributeValueEditModal,
      },
    });
  }

  function onUpdateAttributeValue(attributeValue: IProductAttributeValue) {
    props.onChange({ ...attributeValue }, "updated");
  }

  return (
    <div className={className}>
      <div className="col-12 col-md-12 col-lg-3">
        <div className="mb-1">
          <SecondaryLabel className="me-1">Tên:</SecondaryLabel>
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
          <SecondaryLabel className="me-1">Giá điều chỉnh:</SecondaryLabel>
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
        <SecondaryLabel className="me-1">Thứ tự:</SecondaryLabel>
        <LabelDark>{attributeValue.displayOrder}</LabelDark>
      </div>
      <div className="col-auto offset-auto">
        <ButtonOutlineLight
          type="button"
          size="xs"
          className="me-1"
          title="Sửa giá trị thuộc tính"
          onClick={() => onOpenEditAttributeValueModal(attributeValue)}
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
