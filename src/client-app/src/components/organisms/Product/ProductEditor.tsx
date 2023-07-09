import * as React from "react";
import { useState, useEffect, useMemo } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { SecondaryTextbox } from "../../atoms/Textboxes";
import { ButtonIconSecondary } from "../../molecules/ButtonIcons";
import styled from "styled-components";
import {
  ImageUpload,
  ImageUploadOnChangeEvent,
} from "../UploadControl/ImageUpload";
import AsyncSelect from "react-select/async";
import { Thumbnail } from "../../molecules/Thumbnails";
import { ButtonOutlinePrimary } from "../../atoms/Buttons/OutlineButtons";
import ProductAttributeRow from "./ProductAttributeRow";
import { useStore } from "../../../store/hook-store";
import ProductAttributeEditModal from "./ProductAttributeEditModal";
import { mapSelectOptions } from "../../../utils/SelectOptionUtils";
import { apiConfig } from "../../../config/api-config";
import { ActionMeta, OnChangeValue } from "react-select";
import { IProductAttribute } from "../../../models/productAttributesModel";
import { LexicalEditor } from "lexical";
import { $generateHtmlFromNodes } from "@lexical/html";
import { RichTextEditor } from "../CommonEditor/RichTextEditor";
import { LexicalComposer } from "@lexical/react/LexicalComposer";
import { SharedHistoryContext } from "../CommonEditor/context/SharedHistoryContext";
import { TableContext } from "../CommonEditor/plugins/TablePlugin";
import { SharedAutocompleteContext } from "../CommonEditor/context/SharedAutocompleteContext";
import { defaultEditorConfigs } from "../CommonEditor/configs";
import AsyncSubmitFormPlugin from "../CommonEditor/plugins/AsyncSubmitFormPlugin";
import { FormProvider, Controller, useForm } from "react-hook-form";
import { ValidationDangerMessage } from "../../ErrorMessage";

const FormRow = styled.div`
  margin-bottom: ${(p) => p.theme.size.tiny};

  ${SecondaryTextbox} {
    max-width: 100%;
    width: 100%;
  }

  .cate-selection {
    z-index: 10;
    max-width: 100%;
    > div:first-child {
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
  button {
    min-width: 150px;
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
  const [previews, setPreviews] = useState<
    { pictureId?: number; url?: string; preview?: string; fileName?: string }[]
  >([]);
  const methods = useForm();
  const {
    register,
    handleSubmit,
    setValue,
    reset,
    control,
    getValues,
    trigger,
    formState: { errors },
  } = methods;

  const dispatch = useStore(true)[1];
  const [isSubmitted, setSubmitted] = useState(false);

  const handleImageChange = (e: ImageUploadOnChangeEvent) => {
    const { pictures } = getValues();
    const { preview, file } = e;
    const pictureList = pictures ? [...pictures] : [];
    pictureList.push({
      file: file,
      preview: preview,
    });

    let previewPics = [...previews];
    previewPics.push({
      preview: preview,
    });

    setPreviews([...previewPics]);
    setValue("pictures", pictureList);
  };

  const onProductPost = async (productForm: any) => {
    setSubmitted(true);
    const productData: any = { ...productForm };
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
          requestFormData.append(`farms[${i}].id`, farms[i].value);
        }
      } else if (key === "categories" && productData[key]) {
        const categories = productData[key];
        for (const i in categories) {
          requestFormData.append(`categories[${i}].id`, categories[i].value);
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
    reset((formValues) => ({
      ...formValues,
      name: null,
      farmType: null,
      description: null,
      pictures: [],
    }));
    setPreviews([]);
  };

  const onImageRemoved = (item: {
    pictureId?: number;
    url?: string;
    preview?: string;
    fileName?: string;
  }) => {
    const formData = getValues();
    const pictures = formData.pictures;
    if (!pictures || pictures.length < 0) {
      return;
    }

    let updatedPictures = [...formData.pictures];
    let previewPics = [...previews];
    if (item.pictureId) {
      updatedPictures = updatedPictures.filter(
        (x: any) => x.pictureId !== item.pictureId
      );
      previewPics = previewPics.filter(
        (x: any) => x.pictureId !== item.pictureId
      );
    } else {
      previewPics = previewPics.filter((x: any) => x.preview !== item.preview);
      updatedPictures = updatedPictures.filter((x: any) => x !== item);
    }

    setPreviews([...previewPics]);
    setValue("pictures", updatedPictures);
  };

  const handleSelectChange = (
    newValue: OnChangeValue<any, any>,
    actionMeta: ActionMeta<any>,
    name: string
  ) => {
    const { action } = actionMeta;
    if (action === "clear") {
      setValue(name, []);
    } else if (action === "remove-value") {
      setValue(name, newValue);
    } else {
      setValue(name, newValue);
    }
  };

  const loadCategoriesSelected = () => {
    if (!currentProduct?.categories) {
      return null;
    }
    const { categories } = currentProduct;
    return categories.map((item: any) => {
      return { value: item.id, label: item.name };
    });
  };

  const loadCategorySelections = useMemo(() => {
    return async (value: string) => {
      const categories = currentProduct?.categories;
      const currentIds = categories
        ? categories.map((cate: any) => cate.id)
        : [];

      const response = await filterCategories({
        variables: {
          criterias: { query: value, currentIds },
        },
      });

      const { data } = response;
      const { selections } = data;
      return mapSelectOptions(selections);
    };
  }, [filterCategories, currentProduct]);

  const loadFarmsSelected = () => {
    if (!currentProduct?.farms) {
      return null;
    }
    const { farms } = currentProduct;
    return farms.map((item: any) => {
      return { value: item.id, label: item.name };
    });
  };

  const loadFarmSelections = useMemo(() => {
    return async (value: string) => {
      const famrs = currentProduct?.famrs;
      const currentIds = famrs ? famrs.map((cate: any) => cate.id) : [];

      const response = await filterFarms({
        variables: {
          criterias: { query: value, currentIds },
        },
      });

      const { data } = response;
      const { selections } = data;
      return mapSelectOptions(selections);
    };
  }, [filterFarms, currentProduct]);

  /// Attribute features
  const loadAttributeSelections = (value?: string) => {
    const productAttributes = currentProduct?.productAttributes;
    const attributeIds = productAttributes
      ? productAttributes.map((item) => item.attributeId)
      : [];
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
    const formValues = getValues();
    let productAttributes = formValues["productAttributes"];
    if (!productAttributes) {
      productAttributes = [];
    }

    productAttributes.push(newAttr);
    setValue("productAttributes", productAttributes);
  };

  const onUpdateAttribute = (changed: IProductAttribute, index: number) => {
    const formValues = getValues();
    let productAttributes = formValues["productAttributes"];
    if (!productAttributes) {
      productAttributes = [changed];
    } else {
      productAttributes[index] = changed;
    }

    setValue("productAttributes", productAttributes);
  };

  const onRemoveAttribute = (currentAttr: IProductAttribute) => {
    const formValues = getValues();
    let productAttributes = formValues["productAttributes"];
    if (!productAttributes) {
      return;
    }
    productAttributes = productAttributes.filter(
      (x: IProductAttribute) => x != currentAttr
    );
    setValue("productAttributes", productAttributes);
    trigger("productAttributes");
  };

  const onAttributeChange = (changed: IProductAttribute, index: number) => {
    onUpdateAttribute(changed, index);
    trigger("productAttributes");
  };

  useEffect(() => {
    if (currentProduct?.productAttributes) {
      setValue("productAttributes", currentProduct?.productAttributes);
    }
  }, [currentProduct, currentProduct]);

  useEffect(() => {
    if (currentProduct?.pictures) {
      const { pictures } = currentProduct;
      const previewPics: { url?: string; preview?: string }[] = [];
      if (!pictures) {
        return;
      }

      const pictureList: any[] = [];
      for (const picture of pictures) {
        if (picture.pictureId) {
          const { pictureId } = picture;
          pictureList.push({ pictureId: pictureId });
          previewPics.push({
            url: `${apiConfig.paths.pictures.get.getPicture}/${pictureId}`,
          });
        }
      }
      setValue("pictures", pictureList);
      setPreviews([...previewPics]);
    }
  }, [currentProduct]);

  const onDescriptionChanged = (editor: LexicalEditor) => {
    const html = $generateHtmlFromNodes(editor, null);
    setValue("description", html);
  };

  const handlePriceChange = (evt: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = evt.target;
    if (!value || Number.isNaN(value)) {
      setValue(name, 0);
    } else {
      setValue(name, parseFloat(value));
    }
    trigger(name);
  };

  const formValues = getValues();
  const productAttributes = formValues["productAttributes"];
  const price = formValues["price"];
  return (
    <FormProvider {...methods}>
      <LexicalComposer initialConfig={defaultEditorConfigs}>
        <SharedHistoryContext>
          <TableContext>
            <SharedAutocompleteContext>
              <AsyncSubmitFormPlugin
                onSubmitAsync={handleSubmit(onProductPost)}
                method="POST"
                clearAfterSubmit={true}
              >
                <FormRow className="row g-0">
                  <div className="col-12 col-lg-9 mb-2 mb-lg-0 pe-lg-1">
                    <SecondaryTextbox
                      {...register("name", {
                        required: {
                          value: true,
                          message: "This field is required",
                        },
                        maxLength: {
                          value: 255,
                          message:
                            "The text length cannot exceed the limit 255",
                        },
                      })}
                      defaultValue={currentProduct?.name}
                      autoComplete="off"
                      placeholder="Product title"
                    />
                    {errors.name && (
                      <ValidationDangerMessage>
                        {errors.name.message?.toString()}
                      </ValidationDangerMessage>
                    )}
                  </div>
                  <div className="col-12 col-lg-3 ps-lg-1">
                    <SecondaryTextbox
                      {...register("price")}
                      defaultValue={currentProduct?.price}
                      autoComplete="off"
                      placeholder="Price"
                      onChange={handlePriceChange}
                    />
                    {errors.price && (
                      <ValidationDangerMessage>
                        {errors.price.message?.toString()}
                      </ValidationDangerMessage>
                    )}
                  </div>
                </FormRow>

                <FormRow className="row g-0 mb-2">
                  <div className="col-12 col-sm-5 col-md-5 col-lg-5 mb-2 mb-md-0 pe-sm-1">
                    <Controller
                      control={control}
                      defaultValue={loadCategoriesSelected()}
                      name="categories"
                      rules={{
                        required: {
                          value: true,
                          message: "This field is required",
                        },
                      }}
                      render={({ field }) => (
                        <AsyncSelect
                          {...field}
                          className="cate-selection"
                          isMulti
                          cacheOptions
                          defaultOptions
                          loadOptions={(e) => loadCategorySelections(e)}
                          isClearable={true}
                          placeholder="Select categories"
                          onChange={(e, action) =>
                            handleSelectChange(e, action, "categories")
                          }
                        />
                      )}
                    />
                    {errors.categories && (
                      <ValidationDangerMessage>
                        {errors.categories.message?.toString()}
                      </ValidationDangerMessage>
                    )}
                  </div>
                  <div className="col-12 col-sm-5 col-md-5 col-lg-5 mb-2 mb-md-0 pe-sm-1">
                    <Controller
                      control={control}
                      defaultValue={loadFarmsSelected()}
                      name="farms"
                      render={({ field }) => (
                        <AsyncSelect
                          {...field}
                          className="cate-selection"
                          cacheOptions
                          isMulti
                          defaultOptions
                          loadOptions={(e) => loadFarmSelections(e)}
                          isClearable={true}
                          placeholder="Select farms"
                          onChange={(e, action) =>
                            handleSelectChange(e, action, "farms")
                          }
                        />
                      )}
                    />
                  </div>
                  <div className="col-12 col-sm-2 col-md-2 col-lg-2 ps-sm-1">
                    <Controller
                      control={control}
                      name="pictures"
                      render={({ field }) => (
                        <ThumbnailUpload
                          {...field}
                          onChange={handleImageChange}
                        />
                      )}
                    />
                  </div>
                </FormRow>
                {previews ? (
                  <FormRow className="row">
                    {previews.map((item, index: number) => {
                      if (item.preview) {
                        return (
                          <div className="col-3" key={index}>
                            <ImageEditBox>
                              <Thumbnail src={item.preview}></Thumbnail>
                              <RemoveImageButton
                                onClick={(e) => onImageRemoved(item)}
                              >
                                <FontAwesomeIcon icon="times"></FontAwesomeIcon>
                              </RemoveImageButton>
                            </ImageEditBox>
                          </div>
                        );
                      } else if (item.url) {
                        return (
                          <div className="col-3" key={index}>
                            <ImageEditBox>
                              <Thumbnail src={item.url}></Thumbnail>
                              <RemoveImageButton
                                onClick={() => onImageRemoved(item)}
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
                  {productAttributes &&
                    productAttributes.map((attr, index) => {
                      return (
                        <ProductAttributeRow
                          key={index}
                          attribute={attr}
                          price={price ? price : 0}
                          onRemoveAttribute={onRemoveAttribute}
                          onChange={(e) => onAttributeChange(e, index)}
                          onEditAttribute={(e) =>
                            onOpenEditAttributeModal(e, index)
                          }
                        />
                      );
                    })}
                </FormRow>
                <div className="editor-shell">
                  <Controller
                    control={control}
                    defaultValue={currentProduct?.description}
                    name="description"
                    rules={{
                      required: {
                        value: true,
                        message: "This field is required",
                      },
                      maxLength: {
                        value: 4000,
                        message: "The text length cannot exceed the limit 4000",
                      },
                    }}
                    render={({ field }) => (
                      <RichTextEditor
                        {...field}
                        onChange={onDescriptionChanged}
                      />
                    )}
                  />
                  {errors.description && (
                    <ValidationDangerMessage>
                      {errors.description.message?.toString()}
                    </ValidationDangerMessage>
                  )}
                </div>
                <Footer className="row mb-3">
                  <div className="col-auto"></div>
                  <div className="col-auto ms-auto">
                    <ButtonIconSecondary
                      disabled={isSubmitted}
                      size="xs"
                      icon={["far", "paper-plane"]}
                    >
                      Post
                    </ButtonIconSecondary>
                  </div>
                </Footer>
              </AsyncSubmitFormPlugin>
            </SharedAutocompleteContext>
          </TableContext>
        </SharedHistoryContext>
      </LexicalComposer>
    </FormProvider>
  );
};

export default ProductEditor;
