import React, { Fragment, useState } from "react";
import { PanelBody, PanelFooter } from "../../atoms/Panels";
import { ButtonPrimary } from "../../atoms/Buttons/Buttons";
import { PrimaryTextbox } from "../../atoms/Textboxes";
import styled from "styled-components";

const FormRow = styled.div`
  margin-bottom: ${(p) => p.theme.size.tiny};
  .cate-selection {
    > div {
      border: 1px solid ${(p) => p.theme.color.primaryDivide};
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

  const {
    displayOrder,
    pricePercentageAdjustment,
    priceAdjustment,
    name,
  } = formData;

  const canSubmit = !!name;
  return (
    <Fragment>
      <PanelBody>
        <FormRow>
          <PrimaryTextbox
            name="name"
            value={name}
            placeholder="Name"
            onChange={handleInputChange}
          />
        </FormRow>
        <FormRow>
          <PrimaryTextbox
            name="priceAdjustment"
            placeholder="Price adjustment"
            onChange={(e) => handleInputChange(e, parseFloat)}
            value={priceAdjustment ? priceAdjustment : ""}
          />
        </FormRow>
        <FormRow>
          <PrimaryTextbox
            name="pricePercentageAdjustment"
            placeholder="Price percentage adjustment"
            onChange={(e) => handleInputChange(e, parseFloat)}
            value={pricePercentageAdjustment ? pricePercentageAdjustment : ""}
          />
        </FormRow>
        <FormRow>
          <PrimaryTextbox
            name="displayOrder"
            placeholder="Display Order"
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
          {attributeValueIndex || attributeValueIndex === 0 ? "Update" : "Add"}
        </ButtonPrimary>
      </PanelFooter>
    </Fragment>
  );
}
