import React from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { LabelNormal, LabelDark } from "../../atoms/Labels";
import {
  ButtonOutlinePrimary,
  ButtonOutlineDanger,
  ButtonOutlineLight,
} from "../../atoms/Buttons/OutlineButtons";
import styled from "styled-components";
import ProductAttributeValueRow from "./ProductAttributeValueRow";

const FormRow = styled.div`
  margin-bottom: ${(p) => p.theme.size.tiny};
  background-color: ${(p) => p.theme.color.neutralBg};
  border-radius: ${(p) => p.theme.borderRadius.normal};

  label {
    vertical-align: middle;
  }
`;

const AttributeValuePanel = styled.div`
  background-color: ${(p) => p.theme.color.neutralBg};
  border-bottom-left-radius: ${(p) => p.theme.borderRadius.normal};
  border-bottom-right-radius: ${(p) => p.theme.borderRadius.normal};
`;

const ProductAttributeRow = (props) => {
  const { attribute, onEditAttributeValue, price } = props;
  const { attributeRelationValues } = attribute;

  const onRemoveAttributeValue = (currentAttributeValue) => {
    let { attribute } = { ...props };
    let {
      attributeRelationValues: [...attributeRelationValues],
    } = attribute;
    const index = attributeRelationValues.indexOf(currentAttributeValue);
    attributeRelationValues.splice(index, 1);

    props.onAttributeChange({ ...attribute, attributeRelationValues });
  };

  return (
    <FormRow className="mb-2">
      <div className="row p-2">
        <div className="col-2 col-xl-1">
          <ButtonOutlinePrimary
            type="button"
            size="xs"
            title="Thêm giá trị cho thuộc tính"
            onClick={() => props.onAddAttributeValue()}
          >
            <FontAwesomeIcon icon="plus" />
          </ButtonOutlinePrimary>
        </div>
        <div className="col-3 col-xl-3 ps-0 pt-1">
          <LabelNormal className="me-1">Tên thuộc tính:</LabelNormal>
          <LabelDark>{attribute.name}</LabelDark>
        </div>
        <div className="col-3 col-xl-3 pt-1">
          <LabelNormal className="me-1">Kiểu hiển thị:</LabelNormal>
          <LabelDark>{attribute.controlTypeName}</LabelDark>
        </div>
        <div className="col-2 col-xl-3 pt-1">
          <LabelNormal className="me-1">Thứ tự:</LabelNormal>
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
              return (
                <ProductAttributeValueRow
                  className="p-2 row mb-2"
                  key={index}
                  price={price}
                  attributeValue={attrVal}
                  onRemoveAttributeValue={onRemoveAttributeValue}
                  onEditAttributeValue={(e) => onEditAttributeValue(e, index)}
                ></ProductAttributeValueRow>
              );
            })
          : null}
      </AttributeValuePanel>
    </FormRow>
  );
};

export default ProductAttributeRow;
