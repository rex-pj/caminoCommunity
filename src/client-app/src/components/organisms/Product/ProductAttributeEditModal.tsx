import * as React from "react";
import { Fragment, useState, useRef } from "react";
import { PanelBody, PanelFooter } from "../../molecules/Panels";
import { ButtonPrimary } from "../../atoms/Buttons/Buttons";
import AsyncSelect from "react-select/async";
import { PrimaryTextbox } from "../../atoms/Textboxes";
import styled from "styled-components";
import { IProductAttribute } from "../../../models/productAttributesModel";

const FormRow = styled.div`
  margin-bottom: ${(p) => p.theme.size.tiny};
  .cate-selection {
    > div {
      border: 1px solid ${(p) => p.theme.color.primaryBg};
    }
  }
`;

type Props = {
  data: {
    attribute: IProductAttribute;
    index: number;
  };
  execution: {
    onEditAttribute: (formData: IProductAttribute, index: number) => void;
    loadAttributeSelections: () => void;
    loadAttributeControlTypeSelections: () => void;
  };
  closeModal: () => void;
};

const ProductAttributeEditModal = (props: Props) => {
  const { data, execution } = props;
  const {
    onEditAttribute,
    loadAttributeSelections,
    loadAttributeControlTypeSelections,
  } = execution;
  const { attribute, index } = data;
  const selectRef = useRef<any>();
  const [formData, setFormData] = useState<IProductAttribute>(attribute);

  const handleInputChange = (
    evt: React.ChangeEvent<HTMLInputElement>,
    formatFunc?: any
  ) => {
    let attribute: IProductAttribute = { ...formData };
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

  const handleAttributeSelectChange = (newValue: any, actionMeta: any) => {
    const { action } = actionMeta;
    let data: IProductAttribute = { ...formData } || ({} as IProductAttribute);
    if (action === "clear" || action === "remove-value") {
      data.attributeId = 0;
      data.name = "";
    } else {
      const { value, label } = newValue;
      data.attributeId = value;
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

  const handleControlTypeSelectChange = (newValue: any, actionMeta: any) => {
    const { action } = actionMeta;
    let data = { ...formData } || ({} as IProductAttribute);
    if (action === "clear" || action === "remove-value") {
      data.controlTypeId = 0;
      data.controlTypeName = "";
    } else {
      const { value, label } = newValue;
      data.controlTypeId = value;
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
            placeholder="Chọn thuộc tính"
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
            placeholder="Chọn kiểu hiển thị"
          />
        </FormRow>
        <FormRow>
          <PrimaryTextbox
            name="displayOrder"
            value={displayOrder ? displayOrder : ""}
            placeholder="Thứ tự"
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
          {index || index === 0 ? "Cập nhật" : "Thêm thuộc tính"}
        </ButtonPrimary>
      </PanelFooter>
    </Fragment>
  );
};

export default ProductAttributeEditModal;
