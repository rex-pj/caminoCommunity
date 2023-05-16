import * as React from "react";
import { Fragment, useState } from "react";
import { PanelBody, PanelFooter } from "../../molecules/Panels";
import { ButtonPrimary } from "../../atoms/Buttons/Buttons";
import { PrimaryTextbox } from "../../atoms/Textboxes";
import styled from "styled-components";
import { IProductAttributeValue } from "../../../models/productAttributesModel";

const FormRow = styled.div`
  margin-bottom: ${(p) => p.theme.size.tiny};
  .cate-selection {
    > div {
      border: 1px solid ${(p) => p.theme.color.primaryBg};
    }
  }

  ${PrimaryTextbox} {
    width: 100%;
  }
`;

type Props = {
  data: {
    attributeValue: IProductAttributeValue;
    attributeIndex: number;
    attributeValueIndex: number;
  };
  execution: {
    onEditAttributeValue: (
      attributeValue: IProductAttributeValue,
      attributeIndex: number,
      attributeValueIndex: number
    ) => void;
    loadAttributeSelections: () => void;
    loadAttributeControlTypeSelections: () => void;
  };
  closeModal: () => void;
};

export default function (props: Props) {
  const { data, execution } = props;
  const { onEditAttributeValue } = execution;
  const { attributeValue, attributeIndex, attributeValueIndex } = data;
  const [formData, setFormData] = useState(attributeValue);

  const parseValueToFloat = (value: string) => {
    if (!value || Number.isNaN(value)) {
      return 0;
    }
    return parseFloat(value);
  };

  const handleInputBlur = (
    evt: React.FocusEvent<HTMLInputElement, Element>,
    formatFunc?: any
  ) => {
    let attributeValue = { ...formData };
    const { name, value } = evt.target;
    if (formatFunc) {
      attributeValue[name] = formatFunc(value);
    } else {
      attributeValue[name] = value;
    }

    setFormData({ ...attributeValue });
  };

  const handleInputChange = (
    evt: React.ChangeEvent<HTMLInputElement>,
    formatFunc?: any
  ) => {
    let attributeValue = { ...formData };
    const { name, value } = evt.target;
    if (formatFunc) {
      attributeValue[name] = formatFunc(value);
    } else {
      attributeValue[name] = value;
    }

    setFormData({ ...attributeValue });
  };

  const editAttributeValue = () => {
    onEditAttributeValue(formData, attributeIndex, attributeValueIndex);
    props.closeModal();
  };

  const { displayOrder, pricePercentageAdjustment, priceAdjustment, name } =
    formData;

  const canSubmit = !!name;
  return (
    <Fragment>
      <PanelBody>
        <FormRow>
          <PrimaryTextbox
            name="name"
            value={name}
            placeholder="Tên"
            onChange={handleInputChange}
          />
        </FormRow>
        <FormRow>
          <PrimaryTextbox
            name="priceAdjustment"
            disabled={
              !!pricePercentageAdjustment && pricePercentageAdjustment > 0
            }
            placeholder="Giá điều chỉnh"
            onChange={(e) => handleInputChange(e)}
            onBlur={(e) => handleInputBlur(e, parseValueToFloat)}
            value={priceAdjustment ? priceAdjustment : ""}
          />
        </FormRow>
        <FormRow>
          <PrimaryTextbox
            name="pricePercentageAdjustment"
            placeholder="Giá điều chỉnh theo phần trăm"
            disabled={!!priceAdjustment && priceAdjustment > 0}
            onChange={(e) => handleInputChange(e)}
            onBlur={(e) => handleInputBlur(e, parseValueToFloat)}
            value={pricePercentageAdjustment ? pricePercentageAdjustment : ""}
          />
        </FormRow>
        <FormRow>
          <PrimaryTextbox
            name="displayOrder"
            placeholder="Thứ tự"
            onChange={(e) => handleInputChange(e, parseInt)}
            value={displayOrder ? displayOrder : ""}
          />
        </FormRow>
      </PanelBody>
      <PanelFooter>
        <ButtonPrimary
          disabled={!canSubmit}
          onClick={() => editAttributeValue()}
          size="xs"
        >
          {attributeValueIndex || attributeValueIndex === 0
            ? "Cập nhật"
            : "Thêm giá trị thuộc tính"}
        </ButtonPrimary>
      </PanelFooter>
    </Fragment>
  );
}
