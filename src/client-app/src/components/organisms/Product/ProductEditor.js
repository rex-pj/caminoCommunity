import React, { useState, useRef, useEffect } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { useLocation, useNavigate } from "react-router-dom";
import CommonEditor from "../CommonEditor";
import { SecondaryTextbox } from "../../atoms/Textboxes";
import { ButtonPrimary } from "../../atoms/Buttons/Buttons";
import { checkValidity } from "../../../utils/Validity";
import styled from "styled-components";
import { stateToHTML } from "draft-js-export-html";
import ImageUpload from "../UploadControl/ImageUpload";
import AsyncSelect from "react-select/async";
import productCreationModel from "../../../models/productCreationModel";
import { Thumbnail } from "../../molecules/Thumbnails";
import { ButtonOutlinePrimary } from "../../atoms/Buttons/OutlineButtons";
import ProductAttributeRow from "./ProductAttributeRow";
import { useStore } from "../../../store/hook-store";
import ProductAttributeEditModal from "./ProductAttributeEditModal";
import ProductAttributeValueEditModal from "./ProductAttributeValueEditModal";
import { mapSelectOptions } from "../../../utils/SelectOptionUtils";

const FormRow = styled.div`
  margin-bottom: ${(p) => p.theme.size.tiny};

  ${SecondaryTextbox} {
    max-width: 100%;
    width: 100%;
  }

  .cate-selection {
    z-index: 10;

    > div {
      border: 1px solid ${(p) => p.theme.color.secondaryBg};
    }
  }

  ${AsyncSelect} {
    max-width: 100%;
  }
`;

const ThumbnailUpload = styled(ImageUpload)`
  text-align: center;
  margin: auto;
  display: inline-block;
  vertical-align: middle;

  > span {
    color: ${(p) => p.theme.color.primaryText};
    height: ${(p) => p.theme.size.normal};
    padding: 0 ${(p) => p.theme.size.tiny};
    font-size: ${(p) => p.theme.fontSize.tiny};
    background-color: ${(p) => p.theme.color.lightBg};
    border-radius: ${(p) => p.theme.borderRadius.normal};
    border: 1px solid ${(p) => p.theme.color.secondaryBg};
    cursor: pointer;
    font-weight: 600;

    :hover {
      background-color: ${(p) => p.theme.color.secondaryBg};
    }

    svg {
      display: inline-block;
      margin: 10px auto 0 auto;
    }
  }
`;

const ImageEditBox = styled.div`
  position: relative;
`;

const RemoveImageButton = styled.span`
  position: absolute;
  top: -${(p) => p.theme.size.exSmall};
  right: -${(p) => p.theme.size.exTiny};
  cursor: pointer;
`;

const Footer = styled.div`
  ${ButtonPrimary} {
    width: 200px;
  }
`;

export default (props) => {
  const {
    convertImageCallback,
    onImageValidate,
    height,
    filterCategories,
    filterFarms,
    currentProduct,
    filterAttributes,
    filterProductAttributeControlTypes,
  } = props;
  const [formData, setFormData] = useState(
    JSON.parse(JSON.stringify(productCreationModel))
  );
  const editorRef = useRef();
  const categorySelectRef = useRef();
  const farmSelectRef = useRef();
  const dispatch = useStore(true)[1];

  const handleInputChange = (evt) => {
    let data = formData || {};
    const { name, value } = evt.target;

    data[name].isValid = checkValidity(data, value, name);
    data[name].value = value;

    setFormData({
      ...data,
    });
  };

  const handlePriceChange = (evt) => {
    let data = formData || {};
    const { value } = evt.target;
    const name = "price";

    data[name].isValid = checkValidity(data, value, name);
    if (!value || isNaN(value)) {
      data[name].value = 0;
    } else {
      data[name].value = parseFloat(value);
    }

    setFormData({
      ...data,
    });
  };

  const onDescriptionChanged = (editorState) => {
    const contentState = editorState.getCurrentContent();
    const html = stateToHTML(contentState);

    let data = formData || {};

    data["description"].isValid = checkValidity(data, html, "description");
    data["description"].value = html;

    setFormData({
      ...data,
    });
  };

  const handleImageChange = (e) => {
    let data = formData || {};
    const { preview, file } = e;
    const { name, type } = file;

    let pictures = Object.assign([], data.pictures.value);
    pictures.push({
      base64Data: preview,
      fileName: name,
      contentType: type,
    });

    data.pictures.value = pictures;
    setFormData({
      ...data,
    });
  };

  const onProductPost = async (e) => {
    e.preventDefault();

    let isFormValid = true;
    for (let formIdentifier in formData) {
      isFormValid = formData[formIdentifier].isValid && isFormValid;
      break;
    }

    if (!isFormValid) {
      props.showValidationError(
        "Something went wrong with your input",
        "Something went wrong with your information, please check and input again"
      );

      return;
    }

    const productData = {};
    for (let formIdentifier in formData) {
      if (formIdentifier !== "productAttributes") {
        productData[formIdentifier] = formData[formIdentifier].value;
      } else if (formIdentifier === "productAttributes") {
        let productAttributes = [];
        const attributes = formData[formIdentifier].value;
        if (!attributes || attributes.length === 0) {
          continue;
        }
        const attributeLength = attributes.length;
        for (let i = 0; i < attributeLength; i++) {
          const attribute = attributes[i];
          productAttributes.push({
            id: attribute.id,
            attributeId: attribute.attributeId,
            textPrompt: attribute.textPrompt,
            isRequired: attribute.isRequired,
            controlTypeId: attribute.controlTypeId,
            displayOrder: attribute.displayOrder,
            attributeRelationValues: attribute.attributeRelationValues,
          });
        }

        productData[formIdentifier] = productAttributes;
      }
    }

    if (!productData.id) {
      delete productData["id"];
    }

    await props.onProductPost(productData).then((response) => {
      if (response && response.id) {
        clearFormData();
      }
    });
  };

  const clearFormData = () => {
    editorRef.current.clearEditor();
    categorySelectRef.current.select.select.clearValue();
    farmSelectRef.current.select.select.clearValue();
    var productFormData = JSON.parse(JSON.stringify(productCreationModel));
    setFormData({ ...productFormData });
  };

  const onImageRemoved = (e, item) => {
    let data = formData || {};
    if (!data.pictures) {
      return;
    }

    if (item.pictureId) {
      data.pictures.value = data.pictures.value.filter(
        (x) => x.pictureId !== item.pictureId
      );
    } else {
      data.pictures.value = data.pictures.value.filter((x) => x !== item);
    }

    setFormData({
      ...data,
    });
  };

  const handleSelectChange = async (e, method, name) => {
    let data = formData || {};
    const { action, removedValue } = method;
    if (action === "clear") {
      data[name].value = [];
      data[name].isValid = false;
    } else if (action === "remove-value") {
      data[name].value = data[name].value.filter(
        (x) => x.id !== parseFloat(removedValue.value)
      );
      data[name].isValid = !!data[name].value;
    } else {
      data[name].value = e.map((item) => {
        return { id: parseFloat(item.value), name: item.label };
      });

      data[name].isValid = data[name].value && data[name].value.length > 0;
    }

    setFormData({
      ...data,
    });
  };

  const loadCategoriesSelected = () => {
    const { categories } = formData;
    if (!categories.value) {
      return null;
    }
    return categories.value.map((item) => {
      return { value: item.id, label: item.name };
    });
  };

  const loadCategorySelections = (value) => {
    const { categories } = formData;
    var currentIds = categories?.value?.map((cate) => cate.id);

    return filterCategories({
      variables: {
        criterias: { query: value, currentIds },
      },
    })
      .then((response) => {
        var { data } = response;
        var { selections } = data;
        return mapSelectOptions(selections);
      })
      .catch((error) => {
        return [];
      });
  };

  const loadFarmsSelected = () => {
    const { farms } = formData;
    if (!farms.value) {
      return null;
    }
    return farms.value.map((item) => {
      return { value: item.id, label: item.name };
    });
  };

  const loadFarmSelections = (value) => {
    const { farms } = formData;
    const currentIds = farms?.value?.map((farm) => farm.id);
    return filterFarms({
      variables: {
        criterias: { query: value, currentIds },
      },
    })
      .then((response) => {
        var { data } = response;
        var { selections } = data;
        return mapSelectOptions(selections);
      })
      .catch((error) => {
        return [];
      });
  };

  /// Attribute features
  const loadAttributeSelections = (value) => {
    const {
      productAttributes: { ...productAttributes },
    } = { ...formData };
    let {
      value: [...attributes],
    } = productAttributes;
    const attributeIds = attributes.map((item) => parseInt(item.attributeId));

    return filterAttributes({
      variables: {
        criterias: { query: value, excludedIds: attributeIds },
      },
    })
      .then((response) => {
        var { data } = response;
        var { selections } = data;
        return mapSelectOptions(selections);
      })
      .catch((error) => {
        return [];
      });
  };

  const loadAttributeControlTypeSelections = (value) => {
    return filterProductAttributeControlTypes({
      variables: {
        criterias: { query: value },
      },
    })
      .then((response) => {
        var { data } = response;
        var { selections } = data;
        return mapSelectOptions(selections);
      })
      .catch((error) => {
        return [];
      });
  };

  const openAddAttributeModal = () => {
    dispatch("OPEN_MODAL", {
      data: {
        attribute: {
          attributeId: 0,
          textPrompt: "",
          isRequired: false,
          controlTypeId: 0,
          displayOrder: 0,
        },
        title: "Thêm thuộc tính sản phẩm",
      },
      execution: {
        onEditAttribute: onAddAttribute,
        loadAttributeSelections,
        loadAttributeControlTypeSelections,
      },
      options: {
        isOpen: true,
        position: "fixed",
        innerModal: ProductAttributeEditModal,
      },
    });
  };

  const onOpenEditAttributeModal = (currentAttr, index) => {
    dispatch("OPEN_MODAL", {
      data: {
        attribute: currentAttr,
        title: "Sửa thuộc tính sản phẩm",
        index: index,
      },
      execution: {
        onEditAttribute: onUpdateAttribute,
        loadAttributeSelections,
        loadAttributeControlTypeSelections,
      },
      options: {
        isOpen: true,
        position: "fixed",
        innerModal: ProductAttributeEditModal,
      },
    });
  };

  const onAddAttribute = (data, index) => {
    let {
      productAttributes: { ...productAttributes },
    } = { ...formData };

    let {
      value: [...attributes],
    } = productAttributes;
    attributes.push(data);

    setFormData({
      ...formData,
      productAttributes: {
        ...productAttributes,
        value: attributes,
      },
    });
  };

  const onUpdateAttribute = (data, index) => {
    let {
      productAttributes: { ...productAttributes },
    } = { ...formData };

    let {
      value: [...attributes],
    } = productAttributes;
    attributes[index] = data;

    setFormData({
      ...formData,
      productAttributes: {
        ...productAttributes,
        value: attributes,
      },
    });
  };

  const onRemoveAttribute = (currentAttr) => {
    let {
      productAttributes: { ...productAttributes },
    } = { ...formData };
    let {
      value: [...attributes],
    } = productAttributes;
    const index = attributes.indexOf(currentAttr);
    attributes.splice(index, 1);

    setFormData({
      ...formData,
      productAttributes: {
        ...productAttributes,
        value: attributes,
      },
    });
  };

  const onAttributeChange = (e, index) => {
    let {
      productAttributes: { ...productAttributes },
    } = { ...formData };
    let {
      value: [...attributes],
    } = productAttributes;
    attributes[index] = e;

    setFormData({
      ...formData,
      productAttributes: {
        ...productAttributes,
        value: attributes,
      },
    });
  };

  /// Attribute value features
  const onOpenAddAttributeValueModal = (attributeIndex) => {
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
        attributeIndex: attributeIndex,
      },
      execution: {
        onEditAttributeValue: onAddAttributeValue,
      },
      options: {
        isOpen: true,
        innerModal: ProductAttributeValueEditModal,
      },
    });
  };

  const onOpenEditAttributeValueModal = (
    currentAttributeValue,
    attributeIndex,
    attributeValueIndex
  ) => {
    dispatch("OPEN_MODAL", {
      data: {
        attributeValue: currentAttributeValue,
        title: "Sửa giá trị của thuộc tính sản phẩm",
        attributeIndex: attributeIndex,
        attributeValueIndex: attributeValueIndex,
      },
      execution: {
        onEditAttributeValue: onUpdateAttributeValue,
      },
      options: {
        isOpen: true,
        innerModal: ProductAttributeValueEditModal,
      },
    });
  };

  const onAddAttributeValue = (data, attributeIndex) => {
    if (!attributeIndex && attributeIndex !== 0) {
      return;
    }

    let {
      productAttributes: { ...productAttributes },
    } = { ...formData };
    let {
      value: [...attributes],
    } = productAttributes;

    let attributeRelationValues = [];
    let currentAttribute = attributes[attributeIndex];
    if (!currentAttribute.attributeRelationValues) {
      attributeRelationValues = [data];
    } else {
      attributeRelationValues = [...currentAttribute.attributeRelationValues];
      attributeRelationValues.push(data);
    }

    updateAttributeValue(attributeIndex, attributeRelationValues);
  };

  const onUpdateAttributeValue = (
    data,
    attributeIndex,
    attributeValueIndex
  ) => {
    if (!attributeIndex && attributeIndex !== 0) {
      return;
    }

    let {
      productAttributes: { ...productAttributes },
    } = { ...formData };

    let {
      attributeRelationValues: [...attributeRelationValues],
    } = {
      ...productAttributes.value[attributeIndex],
    };
    attributeRelationValues[attributeValueIndex] = { ...data };
    updateAttributeValue(attributeIndex, attributeRelationValues);
  };

  const updateAttributeValue = (attributeIndex, attributeRelationValues) => {
    const cloneProductAttributes = productAttributes.value.map(
      (elem, index) => {
        if (attributeIndex === index) {
          return {
            ...elem,
            attributeRelationValues: attributeRelationValues,
          };
        }
        return { ...elem };
      }
    );

    var productData = {
      ...formData,
      productAttributes: {
        ...productAttributes,
        value: cloneProductAttributes,
      },
    };
    setFormData(productData);
  };

  useEffect(() => {
    if (currentProduct && !formData?.id?.value) {
      setFormData(currentProduct);
    }
  }, [currentProduct, formData]);

  const { name, price, pictures, categories, farms, productAttributes } =
    formData;

  return (
    <form onSubmit={(e) => onProductPost(e)} method="POST">
      <FormRow className="row g-0">
        <div className="col-12 col-lg-9 mb-2 mb-lg-0 pe-lg-1">
          <SecondaryTextbox
            name="name"
            value={name.value}
            autoComplete="off"
            onChange={(e) => handleInputChange(e)}
            placeholder="Product title"
          />
        </div>
        <div className="col-12 col-lg-3 ps-lg-1">
          <SecondaryTextbox
            name="price"
            value={price.value}
            autoComplete="off"
            onChange={(e) => handlePriceChange(e)}
            placeholder="Price"
          />
        </div>
      </FormRow>
      <FormRow className="row g-0 mb-2">
        <div className="col-12 col-sm-5 col-md-5 col-lg-5 mb-2 mb-md-0 pe-sm-1">
          <AsyncSelect
            key={JSON.stringify(categories)}
            className="cate-selection"
            defaultOptions
            isMulti
            ref={categorySelectRef}
            defaultValue={loadCategoriesSelected()}
            onChange={(e, action) =>
              handleSelectChange(e, action, "categories")
            }
            loadOptions={loadCategorySelections}
            isClearable={true}
            cache={false}
            placeholder="Select categories"
          />
        </div>
        <div className="col-12 col-sm-5 col-md-5 col-lg-5 mb-2 mb-md-0 pe-sm-1">
          <AsyncSelect
            className="cate-selection"
            key={JSON.stringify(farms)}
            defaultOptions
            isMulti
            ref={farmSelectRef}
            defaultValue={loadFarmsSelected()}
            onChange={(e, action) => handleSelectChange(e, action, "farms")}
            loadOptions={loadFarmSelections}
            isClearable={true}
            placeholder="Select farms"
          />
        </div>
        <div className="col-12 col-sm-2 col-md-2 col-lg-2 ps-sm-1">
          <ThumbnailUpload onChange={handleImageChange}></ThumbnailUpload>
        </div>
      </FormRow>
      {pictures.value ? (
        <FormRow className="row">
          {pictures.value.map((item, index) => {
            let imageSrc = item.base64Data;
            if (!imageSrc && item.pictureId) {
              imageSrc = `${process.env.REACT_APP_CDN_PHOTO_URL}${item.pictureId}`;
            } else if (!imageSrc) {
              imageSrc = null;
            }
            return (
              <div className="col-3" key={index}>
                <ImageEditBox>
                  <Thumbnail src={imageSrc}></Thumbnail>
                  <RemoveImageButton onClick={(e) => onImageRemoved(e, item)}>
                    <FontAwesomeIcon icon="times"></FontAwesomeIcon>
                  </RemoveImageButton>
                </ImageEditBox>
              </div>
            );
          })}
        </FormRow>
      ) : null}
      <FormRow>
        <label className="me-1">Thuộc tính sản phẩm</label>
        <ButtonOutlinePrimary
          type="button"
          size="xs"
          title="Thêm thuộc tính sản phẩm"
          onClick={openAddAttributeModal}
        >
          <FontAwesomeIcon icon="plus"></FontAwesomeIcon>
        </ButtonOutlinePrimary>
      </FormRow>
      <FormRow className="mb-4">
        {productAttributes && productAttributes.value
          ? productAttributes.value.map((attr, index) => {
              return (
                <ProductAttributeRow
                  key={index}
                  attribute={attr}
                  price={price.value ? price.value : 0}
                  onRemoveAttribute={onRemoveAttribute}
                  onAttributeChange={(e) => onAttributeChange(e, index)}
                  onEditAttribute={(e) => onOpenEditAttributeModal(e, index)}
                  onAddAttributeValue={() =>
                    onOpenAddAttributeValueModal(index)
                  }
                  onEditAttributeValue={(e, attributeValueIndex) =>
                    onOpenEditAttributeValueModal(e, index, attributeValueIndex)
                  }
                />
              );
            })
          : null}
      </FormRow>
      <CommonEditor
        contentHtml={currentProduct ? currentProduct.description.value : null}
        height={height}
        convertImageCallback={convertImageCallback}
        onImageValidate={onImageValidate}
        placeholder="Enter the description here"
        onChanged={onDescriptionChanged}
        ref={editorRef}
      />
      <Footer className="row mb-3">
        <div className="col-auto"></div>
        <div className="col-auto ms-auto">
          <ButtonPrimary size="xs">Post</ButtonPrimary>
        </div>
      </Footer>
    </form>
  );
};
