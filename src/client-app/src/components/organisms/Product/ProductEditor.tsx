import * as React from "react";
import { useState, useRef, useEffect, useMemo } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { SecondaryTextbox } from "../../atoms/Textboxes";
import { ButtonSecondary } from "../../atoms/Buttons/Buttons";
import { checkValidity } from "../../../utils/Validity";
import styled from "styled-components";
import {
  ImageUpload,
  ImageUploadOnChangeEvent,
} from "../UploadControl/ImageUpload";
import AsyncSelect from "react-select/async";
import { ProductCreationModel } from "../../../models/productCreationModel";
import { Thumbnail } from "../../molecules/Thumbnails";
import { ButtonOutlinePrimary } from "../../atoms/Buttons/OutlineButtons";
import ProductAttributeRow from "./ProductAttributeRow";
import { useStore } from "../../../store/hook-store";
import ProductAttributeEditModal from "./ProductAttributeEditModal";
import ProductAttributeValueEditModal from "./ProductAttributeValueEditModal";
import { mapSelectOptions } from "../../../utils/SelectOptionUtils";
import { apiConfig } from "../../../config/api-config";
import { ActionMeta, OnChangeValue } from "react-select";
import {
  IProductAttribute,
  IProductAttributeValue,
} from "../../../models/productAttributesModel";
import { LexicalEditor } from "lexical";
import { $generateHtmlFromNodes } from "@lexical/html";
import { RichTextEditor } from "../CommonEditor/RichTextEditor";
import { LexicalComposer } from "@lexical/react/LexicalComposer";
import { SharedHistoryContext } from "../CommonEditor/context/SharedHistoryContext";
import { TableContext } from "../CommonEditor/plugins/TablePlugin";
import { SharedAutocompleteContext } from "../CommonEditor/context/SharedAutocompleteContext";
import { defaultEditorConfigs } from "../CommonEditor/configs";
import AsyncSubmitFormPlugin from "../CommonEditor/plugins/AsyncSubmitFormPlugin";

const FormRow = styled.div`
  margin-bottom: ${(p) => p.theme.size.tiny};

  ${SecondaryTextbox} {
    max-width: 100%;
    width: 100%;
  }

  .cate-selection {
    z-index: 10;
    max-width: 100%;
    > div {
      border: 1px solid ${(p) => p.theme.color.secondaryBg};
    }
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
  ${ButtonSecondary} {
    width: 200px;
  }
`;

interface Props {
  height?: number;
  filterCategories: (e: any) => Promise<any>;
  filterFarms: (e: any) => Promise<any>;
  filterAttributes: (e: any) => Promise<any>;
  filterProductAttributeControlTypes: (e: any) => Promise<any>;
  currentProduct?: any;
  showValidationError: (title: string, message: string) => void;
  onProductPost: (e: any) => Promise<any>;
}

const ProductEditor = (props: Props) => {
  const {
    filterCategories,
    filterFarms,
    currentProduct,
    filterAttributes,
    filterProductAttributeControlTypes,
  } = props;
  const [formData, setFormData] = useState<ProductCreationModel>(
    JSON.parse(JSON.stringify(new ProductCreationModel()))
  );

  const categorySelectRef = useRef<any>();
  const farmSelectRef = useRef<any>();
  const dispatch = useStore(true)[1];
  const [isSubmitted, setSubmitted] = useState(false);

  const handleInputChange = (evt: React.ChangeEvent<HTMLInputElement>) => {
    let data = { ...formData } || new ProductCreationModel();
    const { name, value } = evt.target;

    data[name].isValid = checkValidity(data, value, name);
    data[name].value = value;

    setFormData({
      ...data,
    });
  };

  const handlePriceChange = (evt: React.ChangeEvent<HTMLInputElement>) => {
    let data = { ...formData } || new ProductCreationModel();
    const { value } = evt.target;
    const name = "price";

    data[name].isValid = checkValidity(data, value, name);
    if (!value || Number.isNaN(value)) {
      data[name].value = 0;
    } else {
      data[name].value = parseFloat(value);
    }

    setFormData({
      ...data,
    });
  };

  const handleImageChange = (e: ImageUploadOnChangeEvent) => {
    let data = { ...formData } || new ProductCreationModel();
    const { preview, file } = e;
    let pictures = Object.assign([], data.pictures.value);
    pictures.push({
      file: file,
      preview: preview,
    });

    data.pictures.value = pictures;
    setFormData({
      ...data,
    });
  };

  const onProductPost: (
    e: React.FormEvent<HTMLFormElement>
  ) => Promise<any> = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    setSubmitted(true);
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

      setSubmitted(false);
      return Promise.reject("validation: Something went wrong with your input");
    }

    let productData: any = {};
    for (let formIdentifier in formData) {
      if (formIdentifier !== "productAttributes") {
        productData[formIdentifier] = formData[formIdentifier].value;
      } else if (formIdentifier === "productAttributes") {
        let productAttributes: any[] = [];
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

    const requestFormData = new FormData();
    for (const key of Object.keys(productData)) {
      if (key === "pictures" && productData[key]) {
        const pictures = productData[key];
        for (const i in pictures) {
          requestFormData.append(`pictures[${i}].file`, pictures[i].file);
          if (pictures[i].pictureId) {
            requestFormData.append(
              `pictures[${i}].pictureId`,
              pictures[i].pictureId
            );
          }
        }
      } else if (key === "farms" && productData[key]) {
        const farms = productData[key];
        for (const i in farms) {
          requestFormData.append(`farms[${i}].id`, farms[i].id);
        }
      } else if (key === "categories" && productData[key]) {
        const categories = productData[key];
        for (const i in categories) {
          requestFormData.append(`categories[${i}].id`, categories[i].id);
        }
      } else if (key === "productAttributes" && productData[key]) {
        const productAttributes = productData[key];
        for (const i in productAttributes) {
          requestFormData.append(
            `productAttributes[${i}].attributeId`,
            productAttributes[i].attributeId
          );
          requestFormData.append(
            `productAttributes[${i}].textPrompt`,
            productAttributes[i].textPrompt
          );
          requestFormData.append(
            `productAttributes[${i}].isRequired`,
            productAttributes[i].isRequired
          );
          requestFormData.append(
            `productAttributes[${i}].controlTypeId`,
            productAttributes[i].controlTypeId
          );
          requestFormData.append(
            `productAttributes[${i}].displayOrder`,
            productAttributes[i].displayOrder
          );
          if (productAttributes[i].id) {
            requestFormData.append(
              `productAttributes[${i}].id`,
              productAttributes[i].id
            );
          }

          if (
            productAttributes[i].attributeRelationValues &&
            productAttributes[i].attributeRelationValues.length > 0
          ) {
            for (const vIndex in productAttributes[i].attributeRelationValues) {
              requestFormData.append(
                `productAttributes[${i}].attributeRelationValues[${vIndex}].displayOrder`,
                productAttributes[i].attributeRelationValues[vIndex]
                  .displayOrder
              );
              requestFormData.append(
                `productAttributes[${i}].attributeRelationValues[${vIndex}].name`,
                productAttributes[i].attributeRelationValues[vIndex].name
              );
              requestFormData.append(
                `productAttributes[${i}].attributeRelationValues[${vIndex}].priceAdjustment`,
                productAttributes[i].attributeRelationValues[vIndex]
                  .priceAdjustment
              );
              requestFormData.append(
                `productAttributes[${i}].attributeRelationValues[${vIndex}].pricePercentageAdjustment`,
                productAttributes[i].attributeRelationValues[vIndex]
                  .pricePercentageAdjustment
              );
              requestFormData.append(
                `productAttributes[${i}].attributeRelationValues[${vIndex}].quantity`,
                productAttributes[i].attributeRelationValues[vIndex].quantity
              );
            }
          }
        }
      } else {
        requestFormData.append(key, productData[key]);
      }
    }

    return await props
      .onProductPost(requestFormData)
      .then((response) => {
        clearFormData();
        setSubmitted(false);
        return Promise.resolve(response);
      })
      .catch((error) => {
        setSubmitted(false);
        return Promise.reject(error);
      });
  };

  const clearFormData = () => {
    categorySelectRef.current.clearValue();
    farmSelectRef.current.clearValue();
    const productFormData = JSON.parse(
      JSON.stringify(new ProductCreationModel())
    );
    setFormData({ ...productFormData });
  };

  const onImageRemoved = (
    e: React.MouseEvent<HTMLSpanElement, MouseEvent>,
    item: any
  ) => {
    let data = { ...formData } || new ProductCreationModel();
    if (!data.pictures || !data.pictures.value) {
      return;
    }

    if (item.pictureId) {
      data.pictures.value = data.pictures.value.filter(
        (x: any) => x.pictureId !== item.pictureId
      );
    } else {
      data.pictures.value = data.pictures.value.filter((x: any) => x !== item);
    }

    setFormData({
      ...data,
    });
  };

  async function handleSelectChange(
    newValue: OnChangeValue<any, any>,
    actionMeta: ActionMeta<any>,
    name: string
  ) {
    let data = { ...formData } || new ProductCreationModel();
    const { action, removedValue } = actionMeta;
    if (action === "clear") {
      data[name].value = [];
      data[name].isValid = false;
    } else if (action === "remove-value") {
      data[name].value = data[name].value.filter(
        (x: any) => x.id !== parseFloat(removedValue.value)
      );
      data[name].isValid = !!data[name].value;
    } else {
      data[name].value = newValue.map((item: any) => {
        return { id: parseFloat(item.value), name: item.label };
      });

      data[name].isValid = data[name].value && data[name].value.length > 0;
    }

    setFormData({
      ...data,
    });
  }

  const loadCategoriesSelected = () => {
    const { categories } = formData;
    if (!categories.value) {
      return null;
    }
    return categories.value.map((item: any) => {
      return { value: item.id, label: item.name };
    });
  };

  const loadCategorySelections = useMemo(() => {
    return async (value: string) => {
      const { categories } = formData;
      const currentIds = categories?.value?.map((cate: any) => cate.id);

      const response = await filterCategories({
        variables: {
          criterias: { query: value, currentIds },
        },
      });

      const { data } = response;
      const { selections } = data;
      return mapSelectOptions(selections);
    };
  }, [filterCategories, formData]);

  const loadFarmsSelected = () => {
    const { farms } = formData;
    if (!farms.value) {
      return null;
    }
    return farms.value.map((item: any) => {
      return { value: item.id, label: item.name };
    });
  };

  const loadFarmSelections = useMemo(() => {
    return async (value: string) => {
      const { farms } = formData;
      const currentIds = farms?.value?.map((farm: any) => farm.id);

      const response = await filterFarms({
        variables: {
          criterias: { query: value, currentIds },
        },
      });

      const { data } = response;
      const { selections } = data;
      return mapSelectOptions(selections);
    };
  }, [filterFarms, formData]);

  /// Attribute features
  const loadAttributeSelections = (value?: string) => {
    const {
      productAttributes: { ...productAttributes },
    } = { ...formData };
    if (!productAttributes || !productAttributes.value) {
      return;
    }
    const {
      value: [...attributes],
    } = productAttributes;
    const attributeIds = attributes.map((item) => item.attributeId);

    return filterAttributes({
      variables: {
        criterias: { query: value, excludedIds: attributeIds },
      },
    })
      .then((response) => {
        const { data } = response;
        const { selections } = data;
        return mapSelectOptions(selections);
      })
      .catch((error) => {
        return [];
      });
  };

  const loadAttributeControlTypeSelections = (value?: string) => {
    return filterProductAttributeControlTypes({
      variables: {
        criterias: { query: value },
      },
    })
      .then((response) => {
        const { data } = response;
        const { selections } = data;
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

  const onOpenEditAttributeModal = (
    currentAttr: IProductAttribute,
    index: number
  ) => {
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

  const onAddAttribute = (newAttr: IProductAttribute) => {
    let {
      productAttributes: { ...productAttributes },
    } = { ...formData };
    if (!productAttributes || !productAttributes.value) {
      return;
    }
    let {
      value: [...attributes],
    } = productAttributes;
    attributes.push(newAttr);

    setFormData({
      ...formData,
      productAttributes: {
        ...productAttributes,
        value: attributes,
      },
    });
  };

  const onUpdateAttribute = (currentAttr: IProductAttribute, index: number) => {
    const {
      productAttributes: { ...productAttributes },
    } = { ...formData };
    if (!productAttributes || !productAttributes.value) {
      return;
    }
    const {
      value: [...attributes],
    } = productAttributes;
    attributes[index] = currentAttr;

    setFormData({
      ...formData,
      productAttributes: {
        ...productAttributes,
        value: attributes,
      },
    });
  };

  const onRemoveAttribute = (currentAttr: IProductAttribute) => {
    let {
      productAttributes: { ...productAttributes },
    } = { ...formData };
    if (!productAttributes || !productAttributes.value) {
      return;
    }
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

  const onAttributeChange = (currentAttr: IProductAttribute, index: number) => {
    let {
      productAttributes: { ...productAttributes },
    } = { ...formData };
    if (!productAttributes || !productAttributes.value) {
      return;
    }
    let {
      value: [...attributes],
    } = productAttributes;
    attributes[index] = currentAttr;

    setFormData({
      ...formData,
      productAttributes: {
        ...productAttributes,
        value: attributes,
      },
    });
  };

  /// Attribute value features
  function onOpenAddAttributeValueModal(attributeIndex: number) {
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
  }

  function onOpenEditAttributeValueModal(
    currentAttributeValue: IProductAttributeValue,
    attributeIndex: number,
    attributeValueIndex: number
  ) {
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
  }

  function onAddAttributeValue(
    attributeValue: IProductAttributeValue,
    attributeIndex: number
  ) {
    if (!attributeIndex && attributeIndex !== 0) {
      return;
    }

    let {
      productAttributes: { ...productAttributes },
    } = { ...formData };
    if (!productAttributes || !productAttributes.value) {
      return;
    }

    let {
      value: [...attributes],
    } = productAttributes;

    let attributeRelationValues: IProductAttributeValue[] = [];
    let currentAttribute = attributes[attributeIndex];
    if (!currentAttribute.attributeRelationValues) {
      attributeRelationValues = [attributeValue];
    } else {
      attributeRelationValues = [...currentAttribute.attributeRelationValues];
      attributeRelationValues.push(attributeValue);
    }

    updateAttributeValue(attributeIndex, attributeRelationValues);
  }

  function onUpdateAttributeValue(
    attributeValue: IProductAttributeValue,
    attributeIndex: number,
    attributeValueIndex: number
  ) {
    if (!attributeIndex && attributeIndex !== 0) {
      return;
    }

    let {
      productAttributes: { ...productAttributes },
    } = { ...formData };

    if (
      !productAttributes ||
      !productAttributes.value ||
      !productAttributes.value[attributeIndex]
    ) {
      return;
    }

    let { attributeRelationValues } = {
      ...productAttributes.value[attributeIndex],
    };

    if (!attributeRelationValues || !attributeRelationValues.length) {
      return;
    }

    const relationValues = [...attributeRelationValues];
    relationValues[attributeValueIndex] = { ...attributeValue };
    updateAttributeValue(attributeIndex, relationValues);
  }

  function updateAttributeValue(
    attributeIndex: number,
    attributeRelationValues: IProductAttributeValue[]
  ) {
    if (!productAttributes || !productAttributes.value) {
      return;
    }

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

    const productData = {
      ...formData,
      productAttributes: {
        ...productAttributes,
        value: cloneProductAttributes,
      },
    };
    setFormData(productData);
  }

  useEffect(() => {
    if (currentProduct && !formData?.id?.value) {
      setFormData(currentProduct);
    }
  }, [currentProduct, formData]);

  const onDescriptionChanged = (editor: LexicalEditor) => {
    const html = $generateHtmlFromNodes(editor, null);
    let data = formData || {};

    data["description"].isValid = checkValidity(data, html, "description");
    data["description"].value = html;

    setFormData({
      ...data,
    });
  };

  const {
    name,
    price,
    categories,
    farms,
    productAttributes,
    pictures,
    description,
  } = formData;
  const htmlContent = description?.value;
  return (
    <LexicalComposer initialConfig={defaultEditorConfigs}>
      <SharedHistoryContext>
        <TableContext>
          <SharedAutocompleteContext>
            <AsyncSubmitFormPlugin
              onSubmitAsync={(e) => onProductPost(e)}
              method="POST"
              clearAfterSubmit={true}
            >
              <FormRow className="row g-0">
                <div className="col-12 col-lg-9 mb-2 mb-lg-0 pe-lg-1">
                  <SecondaryTextbox
                    name="name"
                    defaultValue={name.value}
                    autoComplete="off"
                    onChange={handleInputChange}
                    placeholder="Product title"
                  />
                </div>
                <div className="col-12 col-lg-3 ps-lg-1">
                  <SecondaryTextbox
                    name="price"
                    defaultValue={price.value}
                    autoComplete="off"
                    onChange={handlePriceChange}
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
                    onChange={(e, action) =>
                      handleSelectChange(e, action, "farms")
                    }
                    loadOptions={(e) => loadFarmSelections(e)}
                    isClearable={true}
                    placeholder="Select farms"
                  />
                </div>
                <div className="col-12 col-sm-2 col-md-2 col-lg-2 ps-sm-1">
                  <ThumbnailUpload
                    onChange={handleImageChange}
                  ></ThumbnailUpload>
                </div>
              </FormRow>
              {pictures.value ? (
                <FormRow className="row">
                  {pictures.value.map((item, index) => {
                    if (item.preview) {
                      return (
                        <div className="col-3" key={index}>
                          <ImageEditBox>
                            <Thumbnail src={item.preview}></Thumbnail>
                            <RemoveImageButton
                              onClick={(e) => onImageRemoved(e, item)}
                            >
                              <FontAwesomeIcon icon="times"></FontAwesomeIcon>
                            </RemoveImageButton>
                          </ImageEditBox>
                        </div>
                      );
                    } else if (item.pictureId) {
                      return (
                        <div className="col-3" key={index}>
                          <ImageEditBox>
                            <Thumbnail
                              src={`${apiConfig.paths.pictures.get.getPicture}/${item.pictureId}`}
                            ></Thumbnail>
                            <RemoveImageButton
                              onClick={(e) => onImageRemoved(e, item)}
                            >
                              <FontAwesomeIcon icon="times"></FontAwesomeIcon>
                            </RemoveImageButton>
                          </ImageEditBox>
                        </div>
                      );
                    }

                    return null;
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
                {productAttributes?.value &&
                  productAttributes.value.map((attr, index) => {
                    return (
                      <ProductAttributeRow
                        key={index}
                        attribute={attr}
                        price={price.value ? price.value : 0}
                        onRemoveAttribute={onRemoveAttribute}
                        onAttributeChange={(e) => onAttributeChange(e, index)}
                        onEditAttribute={(e) =>
                          onOpenEditAttributeModal(e, index)
                        }
                        onAddAttributeValue={() =>
                          onOpenAddAttributeValueModal(index)
                        }
                        onEditAttributeValue={(e, attributeValueIndex) =>
                          onOpenEditAttributeValueModal(
                            e,
                            index,
                            attributeValueIndex
                          )
                        }
                      />
                    );
                  })}
              </FormRow>
              <div className="editor-shell">
                <RichTextEditor
                  initialHtml={htmlContent}
                  onChange={onDescriptionChanged}
                />
              </div>
              <Footer className="row mb-3">
                <div className="col-auto"></div>
                <div className="col-auto ms-auto">
                  <ButtonSecondary disabled={isSubmitted} size="xs">
                    Post
                  </ButtonSecondary>
                </div>
              </Footer>
            </AsyncSubmitFormPlugin>
          </SharedAutocompleteContext>
        </TableContext>
      </SharedHistoryContext>
    </LexicalComposer>
  );
};

export default ProductEditor;
