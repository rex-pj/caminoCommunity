import * as React from "react";
import { LabelPrimary } from "../../atoms/Labels";
import styled from "styled-components";
import { ProductAttributeControlType } from "../../../utils/Enums";
import HorizontalListAttributeValues from "./HorizontalListAttributeValues";
import VerticalListAttributeValues from "./VerticalListAttributeValues";
import DropdownListAttributeValues from "./DropdownListAttributeValues";
import RadioListAttributeValues from "./RadioListAttributeValues";
import CheckboxesAttributeValues from "./CheckboxesAttributeValues";
import ColorBoxesAttributeValues from "./ColorBoxesAttributeValues";
import { IProductAttribute } from "../../../models/productAttributesModel";

const AttributeLabel = styled(LabelPrimary)`
  font-weight: 700;
`;

type Props = {
  productAttributes: IProductAttribute[];
  price: number;
};

const AttributesDetail = (props: Props) => {
  const { productAttributes, price } = props;

  if (!productAttributes) {
    return null;
  }

  const renderAttributeValues = (attribute: IProductAttribute) => {
    const { attributeRelationValues } = attribute;
    if (
      attribute.controlTypeId === ProductAttributeControlType.HorizontalList
    ) {
      return (
        <HorizontalListAttributeValues
          price={price}
          attributeRelationValues={attributeRelationValues}
        ></HorizontalListAttributeValues>
      );
    }

    if (attribute.controlTypeId === ProductAttributeControlType.VerticalList) {
      return (
        <VerticalListAttributeValues
          price={price}
          attributeRelationValues={attributeRelationValues}
        ></VerticalListAttributeValues>
      );
    }

    if (attribute.controlTypeId === ProductAttributeControlType.DropdownList) {
      return (
        <div>
          <DropdownListAttributeValues
            price={price}
            attributeRelationValues={attributeRelationValues}
          ></DropdownListAttributeValues>
        </div>
      );
    }

    if (attribute.controlTypeId === ProductAttributeControlType.RadioList) {
      return (
        <div>
          <RadioListAttributeValues
            attribute={attribute}
            price={price}
            attributeRelationValues={attributeRelationValues}
          ></RadioListAttributeValues>
        </div>
      );
    }

    if (attribute.controlTypeId === ProductAttributeControlType.Checkboxes) {
      return (
        <div>
          <CheckboxesAttributeValues
            attribute={attribute}
            price={price}
            attributeRelationValues={attributeRelationValues}
          ></CheckboxesAttributeValues>
        </div>
      );
    }

    if (attribute.controlTypeId === ProductAttributeControlType.ColorBoxes) {
      return (
        <div>
          <ColorBoxesAttributeValues
            price={price}
            attributeRelationValues={attributeRelationValues}
          ></ColorBoxesAttributeValues>
        </div>
      );
    }
  };

  return (
    <>
      {productAttributes.map((attr, index) => {
        return (
          <div key={index} className="mt-3">
            <AttributeLabel>{attr.name}:</AttributeLabel>

            {renderAttributeValues(attr)}
          </div>
        );
      })}
    </>
  );
};

export default AttributesDetail;
