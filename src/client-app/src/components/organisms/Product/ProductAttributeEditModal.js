import React, { Fragment, useState, useRef } from "react";
import { PanelBody, PanelFooter } from "../../atoms/Panels";
import { ButtonPrimary } from "../../atoms/Buttons/Buttons";
import AsyncSelect from "react-select/async";
import { PrimaryTextbox } from "../../atoms/Textboxes";
import styled from "styled-components";

const FormRow = styled.div`
  margin-bottom: ${(p) => p.theme.size.tiny};
  .cate-selection {
    > div {
      border: 1px solid ${(p) => p.theme.color.primaryDivide};
    }
  }
`;

export default function (props) {
  const { data, execution } = props;
  const {
    onEditAttribute,
    loadAttributeSelections,
    loadAttributeControlTypeSelections,
  } = execution;
  const { attribute, index } = data;
  const selectRef = useRef();
  const [formData, setFormData] = useState(attribute);

  const handleInputChange = (evt, formatFunc) => {
    let attribute = { ...formData };
    const { name, value } = evt.target;
    if (formatFunc) {
      attribute[name] = formatFunc(value);
    } else {
      attribute[name] = value;
    }

    setFormData({ ...attribute });
  };
  const editAttribute = () => {
    onEditAttribute(formData, index);
    props.closeModal();
  };

  const loadAttributeSelected = () => {
    const { attributeId, name } = { ...formData };
    if (!attributeId) {
      return null;
    }
    return {
      label: name,
      value: attributeId,
    };
  };

  const handleAttributeSelectChange = (e, method) => {
    const { action } = method;
    let data = { ...formData } || {};
    if (action === "clear" || action === "remove-value") {
      data.attributeId = 0;
      data.name = "";
    } else {
      const { value, label } = e;
      data.attributeId = parseInt(value);
      data.name = label;
    }

    setFormData({
      ...data,
    });
  };

  const loadControlTypeSelected = () => {
    const { controlTypeId, controlTypeName } = { ...formData };
    if (!controlTypeId) {
      return null;
    }
    return {
      label: controlTypeName,
      value: controlTypeId,
    };
  };

  const handleControlTypeSelectChange = (e, method) => {
    const { action } = method;
    let data = { ...formData } || {};
    if (action === "clear" || action === "remove-value") {
      data.controlTypeId = 0;
      data.controlTypeName = "";
    } else {
      const { value, label } = e;
      data.controlTypeId = parseInt(value);
      data.controlTypeName = label;
    }

    setFormData({
      ...data,
    });
  };

  const { displayOrder, controlTypeId, attributeId } = formData;
  const canSubmit = controlTypeId && attributeId;
  return (
    <Fragment>
      <PanelBody>
        <FormRow>
          <AsyncSelect
            className="cate-selection"
            cacheOptions
            defaultOptions
            ref={selectRef}
            defaultValue={loadAttributeSelected()}
            onChange={(e, action) => handleAttributeSelectChange(e, action)}
            loadOptions={loadAttributeSelections}
            isClearable={true}
            placeholder="Select attribute"
          />
        </FormRow>
        <FormRow>
          <AsyncSelect
            className="cate-selection"
            cacheOptions
            defaultOptions
            ref={selectRef}
            defaultValue={loadControlTypeSelected()}
            onChange={(e, action) => handleControlTypeSelectChange(e, action)}
            loadOptions={loadAttributeControlTypeSelections}
            isClearable={true}
            placeholder="Select control type"
          />
        </FormRow>
        <FormRow>
          <PrimaryTextbox
            name="displayOrder"
            value={displayOrder ? displayOrder : ""}
            placeholder="Display Order"
            onChange={(e) => handleInputChange(e, parseInt)}
          />
        </FormRow>
      </PanelBody>
      <PanelFooter>
        <ButtonPrimary
          disabled={!canSubmit}
          onClick={() => editAttribute()}
          size="xs"
        >
          {index || index === 0 ? "Update" : "Add"}
        </ButtonPrimary>
      </PanelFooter>
    </Fragment>
  );
}
