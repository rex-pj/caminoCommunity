import React, { Fragment, useState } from "react";
import { PanelBody, PanelFooter } from "../../molecules/Panels";
import { ButtonPrimary } from "../../atoms/Buttons/Buttons";
import { PrimaryTextbox } from "../../atoms/Textboxes";
import styled from "styled-components";

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

export default function (props) {
  const { data, execution } = props;
  const { onEditAttributeValue } = execution;
  const { attributeValue, attributeIndex, attributeValueIndex } = data;
  const [formData, setFormData] = useState(attributeValue);

  const parseValueToFloat = (value) => {
    if (!value || isNaN(value)) {
      return 0;
    }
    return parseFloat(value);
  };

  const handleInputBlur = (evt, formatFunc) => {
    let attributeValue = { ...formData };
    const { name, value } = evt.target;
    if (formatFunc) {
      attributeValue[name] = formatFunc(value);
    } else {
      attributeValue[name] = value;
    }

    setFormData({ ...attributeValue });
  };

  const handleInputChange = (evt, formatFunc) => {
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
            disabled={pricePercentageAdjustment}
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
            disabled={priceAdjustment}
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
