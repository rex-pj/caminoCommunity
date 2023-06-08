import * as React from "react";
import { useState, useRef, useEffect, useMemo } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { SecondaryTextbox } from "../../atoms/Textboxes";
import { ButtonIconSecondary } from "../../molecules/ButtonIcons";
import { checkValidity } from "../../../utils/Validity";
import styled from "styled-components";
import {
  ImageUpload,
  ImageUploadOnChangeEvent,
} from "../UploadControl/ImageUpload";
import AsyncSelect from "react-select/async";
import { FarmCreationModel } from "../../../models/farmCreationModel";
import { Thumbnail } from "../../molecules/Thumbnails";
import { mapSelectOptions } from "../../../utils/SelectOptionUtils";
import { apiConfig } from "../../../config/api-config";
import { ActionMeta, OnChangeValue } from "react-select";
import { LexicalEditor } from "lexical";
import { $generateHtmlFromNodes } from "@lexical/html";
import { LexicalComposer } from "@lexical/react/LexicalComposer";
import { SharedHistoryContext } from "../CommonEditor/context/SharedHistoryContext";
import { TableContext } from "../CommonEditor/plugins/TablePlugin";
import { SharedAutocompleteContext } from "../CommonEditor/context/SharedAutocompleteContext";
import { defaultEditorConfigs } from "../CommonEditor/configs";
import { RichTextEditor } from "../CommonEditor/RichTextEditor";
import AsyncSubmitFormPlugin from "../CommonEditor/plugins/AsyncSubmitFormPlugin";

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

interface FarmEditorProps {
  height?: number;
  filterCategories: (e: any) => Promise<any>;
  currentFarm?: any;
  showValidationError: (title: string, message: string) => void;
  onFarmPost: (e: any) => Promise<any>;
}

const FarmEditor = (props: FarmEditorProps) => {
  const { filterCategories, currentFarm } = props;
  const [formData, setFormData] = useState({ ...new FarmCreationModel() });
  const selectRef = useRef<any>();
  const [isSubmitted, setSubmitted] = useState(false);

  function handleInputChange(evt: React.ChangeEvent<HTMLInputElement>) {
    let data = formData || {};
    const { name, value } = evt.target;

    data[name].isValid = checkValidity(data, value, name);
    data[name].value = value;

    setFormData({
      ...data,
    });
  }

  function handleImageChange(e: ImageUploadOnChangeEvent) {
    let data = { ...formData } || new FarmCreationModel();
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
  }

  const onFarmPost: (
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

    let farmData: any = {};
    for (const formIdentifier in formData) {
      farmData[formIdentifier] = formData[formIdentifier].value;
    }

    if (!farmData.id) {
      delete farmData["id"];
    }

    delete farmData["farmTypeName"];
    const requestFormData = new FormData();
    for (const key of Object.keys(farmData)) {
      if (key === "pictures" && farmData[key]) {
        const pictures = farmData["pictures"];
        for (const i in pictures.value) {
          requestFormData.append(`pictures[${i}].file`, pictures.value[i].file);
          if (pictures.value[i].pictureId) {
            requestFormData.append(
              `pictures[${i}].pictureId`,
              pictures.value[i].pictureId
            );
          }
        }
      } else {
        requestFormData.append(key, farmData[key]);
      }
    }

    return await props
      .onFarmPost(requestFormData)
      .then((id) => {
        clearFormData();
        let timeoutId = setTimeout(() => {
          setSubmitted(false);
          clearTimeout(timeoutId);
        }, 3000);

        return Promise.resolve(id);
      })
      .catch((error) => {
        setSubmitted(false);
        return Promise.reject(error);
      });
  };

  const loadFarmTypeSelections = useMemo(() => {
    return async (value: string) => {
      const response = await filterCategories({
        variables: {
          criterias: { query: value },
        },
      });

      const { data } = response;
      const { selections } = data;
      return mapSelectOptions(selections);
    };
  }, [filterCategories]);

  function handleSelectChange(
    newValue: OnChangeValue<any, any>,
    actionMeta: ActionMeta<any>
  ) {
    const { action } = actionMeta;
    let data = formData || {};
    if (action === "clear" || action === "remove-value") {
      data.farmTypeId.isValid = false;
      data.farmTypeId.value = 0;
      data.farmTypeName.value = "";
    } else {
      const { value, label } = newValue;
      data.farmTypeId.isValid = checkValidity(data, value, "farmTypeId");
      data.farmTypeId.value = parseFloat(value);
      data.farmTypeName.value = label;
    }

    setFormData({
      ...data,
    });
  }

  function loadFarmTypeSelected() {
    const { farmTypeId, farmTypeName } = formData;
    if (!farmTypeId.value) {
      return null;
    }

    return {
      label: farmTypeName.value,
      value: farmTypeId.value,
    };
  }

  const clearFormData = () => {
    selectRef.current.clearValue();
    const farmFormData = { ...new FarmCreationModel() };
    setFormData({ ...farmFormData });
  };

  const onImageRemoved = (
    e: React.MouseEvent<HTMLSpanElement, MouseEvent>,
    item: any
  ) => {
    let data = formData || {};
    if (!data.pictures) {
      return;
    }

    if (item.pictureId && data.pictures.value) {
      data.pictures.value = data.pictures.value.filter(
        (x: any) => x.pictureId !== item.pictureId
      );
    } else if (data.pictures.value) {
      data.pictures.value = data.pictures.value.filter((x: any) => x !== item);
    }

    setFormData({
      ...data,
    });
  };

  useEffect(() => {
    if (currentFarm && !formData?.id?.value) {
      setFormData(currentFarm);
    }
  }, [currentFarm, formData]);

  const onDescriptionChanged = (editor: LexicalEditor) => {
    const html = $generateHtmlFromNodes(editor, null);
    let data = formData || {};

    data.description.isValid = checkValidity(data, html, "description");
    data.description.value = html;

    setFormData({
      ...data,
    });
  };

  const { name, address, farmTypeId, pictures, description } = formData;
  const htmlContent = description?.value;
  return (
    <LexicalComposer initialConfig={defaultEditorConfigs}>
      <SharedHistoryContext>
        <TableContext>
          <SharedAutocompleteContext>
            <AsyncSubmitFormPlugin
              onSubmitAsync={(e) => onFarmPost(e)}
              method="POST"
              clearAfterSubmit={true}
            >
              <FormRow className="row">
                <div className="col-6 col-lg-6 pr-lg-1">
                  <SecondaryTextbox
                    name="name"
                    defaultValue={name.value}
                    autoComplete="off"
                    onChange={handleInputChange}
                    placeholder="Farm title"
                  />
                </div>
                <div className="col-6 col-lg-6 pl-lg-1">
                  <AsyncSelect
                    key={JSON.stringify(farmTypeId)}
                    className="cate-selection"
                    cacheOptions
                    defaultOptions
                    ref={selectRef}
                    defaultValue={loadFarmTypeSelected()}
                    onChange={(e, action) => handleSelectChange(e, action)}
                    loadOptions={loadFarmTypeSelections}
                    isClearable={true}
                  />
                </div>
              </FormRow>
              <FormRow className="row">
                <div className="col-9 col-lg-10 pr-lg-1">
                  <SecondaryTextbox
                    name="address"
                    defaultValue={address.value ? address.value : ""}
                    autoComplete="off"
                    onChange={handleInputChange}
                    placeholder="Address"
                  />
                </div>
                <div className="col-3 col-lg-2 pl-lg-1 pl-1">
                  <ThumbnailUpload
                    onChange={handleImageChange}
                  ></ThumbnailUpload>
                </div>
              </FormRow>
              {pictures.value ? (
                <FormRow className="row">
                  {pictures.value.map((item: any, index: number) => {
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
              <div className="editor-shell">
                <RichTextEditor
                  initialHtml={htmlContent}
                  onChange={onDescriptionChanged}
                />
              </div>
              <Footer className="row mb-3">
                <div className="col-auto"></div>
                <div className="col-auto ms-auto">
                  <ButtonIconSecondary
                    type="submit"
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
  );
};

export default FarmEditor;
