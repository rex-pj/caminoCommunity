import * as React from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { LabelSecondary, LabelDark } from "../../atoms/Labels";
import {
  ButtonOutlinePrimary,
  ButtonOutlineDanger,
  ButtonOutlineLight,
} from "../../atoms/Buttons/OutlineButtons";
import styled from "styled-components";
import ProductAttributeValueRow from "./ProductAttributeValueRow";
import {
  IProductAttribute,
  IProductAttributeValue,
} from "../../../models/productAttributesModel";
import { useStore } from "../../../store/hook-store";
import ProductAttributeValueEditModal from "./ProductAttributeValueEditModal";

const FormRow = styled.div`
  margin-bottom: ${(p) => p.theme.size.tiny};
  background-color: ${(p) => p.theme.color.neutralBg};
  border-radius: ${(p) => p.theme.borderRadius.normal};

  label {
    vertical-align: middle;
  }
`;

const AttributeValuePanel = styled.div`
  background-color: ${(p) => p.theme.color.neutralBorder};
  border-bottom-left-radius: ${(p) => p.theme.borderRadius.normal};
  border-bottom-right-radius: ${(p) => p.theme.borderRadius.normal};

  .attr-value-row {
    border-bottom: 1px solid ${(p) => p.theme.color.neutralBg};
  }
`;

type Props = {
  attribute: IProductAttribute;
  onChange: (
    attribute: IProductAttribute,
    event: "added" | "updated" | "removed"
  ) => void;
  onEditAttribute: (attribute: IProductAttribute) => void;
  onRemoveAttribute: (attribute: IProductAttribute) => void;
  price: number;
};

const ProductAttributeRow = (props: Props) => {
  const dispatch = useStore(true)[1];
  const { attribute, price } = props;
  const { attributeRelationValues } = attribute;

  /// Attribute value features
  function onOpenAddAttributeValueModal() {
    dispatch("OPEN_MODAL", {
      data: {
        attributeValue: {
          name: "",
          priceAdjustment: 0,
          pricePercentageAdjustment: 0,
          quantity: 0,
          displayOrder: 0,
        },
        title: "Thêm giá trị của thuộc tính sản phẩm",
      },
      execution: {
        onEditAttributeValue: onAddAttributeValue,
      },
      options: {
        isOpen: true,
        innerModal: ProductAttributeValueEditModal,
      },
    });
  }

  const onRemoveAttributeValue = (
    currentAttributeValue: IProductAttributeValue
  ) => {
    const { attribute } = props;
    if (!attribute?.attributeRelationValues) {
      return;
    }

    const attributeRelationValues = attribute.attributeRelationValues.filter(
      (attributeValue) => attributeValue != currentAttributeValue
    );

    props.onChange({ ...attribute, attributeRelationValues }, "removed");
  };

  function onAddAttributeValue(attributeValue: IProductAttributeValue) {
    const { attribute } = props;

    let attributeRelationValues: IProductAttributeValue[] = [];
    const currentAttribute = { ...attribute };
    if (!currentAttribute.attributeRelationValues) {
      attributeRelationValues = [attributeValue];
    } else {
      attributeRelationValues = [...currentAttribute.attributeRelationValues];
      attributeRelationValues.push(attributeValue);
    }

    props.onChange({ ...attribute, attributeRelationValues }, "added");
  }

  function onUpdateAttributeValue(
    attributeValue: IProductAttributeValue,
    attributeValueIndex: number
  ) {
    let { attributeRelationValues } = attribute;
    if (!attributeRelationValues?.length) {
      return;
    }

    attributeRelationValues[attributeValueIndex] = { ...attributeValue };
    props.onChange({ ...attribute, attributeRelationValues }, "updated");
  }

  return (
    <FormRow className="mb-2">
      <div className="row p-2">
        <div className="col-2 col-xl-1">
          <ButtonOutlinePrimary
            type="button"
            size="xs"
            title="Thêm giá trị cho thuộc tính"
            onClick={onOpenAddAttributeValueModal}
          >
            <FontAwesomeIcon icon="plus" />
          </ButtonOutlinePrimary>
        </div>
        <div className="col-3 col-xl-3 ps-0 pt-1">
          <LabelSecondary className="me-1">Tên thuộc tính:</LabelSecondary>
          <LabelDark>{attribute.name}</LabelDark>
        </div>
        <div className="col-3 col-xl-3 pt-1">
          <LabelSecondary className="me-1">Kiểu hiển thị:</LabelSecondary>
          <LabelDark>{attribute.controlTypeName}</LabelDark>
        </div>
        <div className="col-2 col-xl-3 pt-1">
          <LabelSecondary className="me-1">Thứ tự:</LabelSecondary>
          <LabelDark>{attribute.displayOrder}</LabelDark>
        </div>
        <div className="col-auto">
          <ButtonOutlineLight
            type="button"
            size="xs"
            className="me-1"
            title="Edit"
            onClick={() => props.onEditAttribute(attribute)}
          >
            <FontAwesomeIcon icon="pencil-alt" />
          </ButtonOutlineLight>
          <ButtonOutlineDanger
            type="button"
            size="xs"
            title="Xóa thuộc tính"
            onClick={() => props.onRemoveAttribute(attribute)}
          >
            <FontAwesomeIcon icon="times" />
          </ButtonOutlineDanger>
        </div>
      </div>
      <AttributeValuePanel>
        {attributeRelationValues
          ? attributeRelationValues.map((attrVal, index) => {
              const key = `${attrVal.id}${index}`;
              return (
                <ProductAttributeValueRow
                  className="py-2 row mb-2 attr-value-row mx-0"
                  key={key}
                  price={price}
                  attributeValue={attrVal}
                  onRemoveAttributeValue={onRemoveAttributeValue}
                  onChange={(e) => onUpdateAttributeValue(e, index)}
                ></ProductAttributeValueRow>
              );
            })
          : null}
      </AttributeValuePanel>
    </FormRow>
  );
};

export default ProductAttributeRow;
