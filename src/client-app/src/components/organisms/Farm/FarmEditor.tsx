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
import { Controller, useForm } from "react-hook-form";
import { ValidationDangerMessage } from "../../ErrorMessage";

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
  const [isSubmitted, setSubmitted] = useState(false);
  const [previews, setPreviews] = useState<
    { pictureId?: number; url?: string; preview?: string; fileName?: string }[]
  >([]);
  const {
    register,
    handleSubmit,
    setValue,
    reset,
    control,
    getValues,
    formState: { errors },
  } = useForm();

  const onFarmPost = async (farmForm: any) => {
    setSubmitted(true);

    const farmRequest: any = { ...farmForm };
    const requestFormData = new FormData();
    for (const key of Object.keys(farmRequest)) {
      if (key === "farmType") {
        const { value: farmTypeId } = farmRequest[key];
        requestFormData.append("farmTypeId", farmTypeId);
        continue;
      }

      if (key !== "pictures") {
        requestFormData.append(key, farmRequest[key]);
        continue;
      }

      const pictures = farmRequest[key];
      if (!pictures) {
        continue;
      }

      for (const i in pictures) {
        requestFormData.append(`pictures[${i}].file`, pictures[i].file);
        if (pictures[i].pictureId) {
          requestFormData.append(
            `pictures[${i}].pictureId`,
            pictures[i].pictureId
          );
        }
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
    if (action === "clear" || action === "remove-value") {
      setValue("farmType", null);
    } else {
      setValue("farmType", newValue);
    }
  }

  function loadFarmTypeSelected() {
    return () => {
      if (!currentFarm?.farmTypeId) {
        return null;
      }

      const { farmTypeName, farmTypeId } = currentFarm;
      return {
        label: farmTypeName,
        value: farmTypeId,
      };
    };
  }

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

  function handleImageChange(e: ImageUploadOnChangeEvent) {
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
  }

  useEffect(() => {
    if (currentFarm?.pictures) {
      const { pictures } = currentFarm;
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
  }, [currentFarm]);

  const onDescriptionChanged = (editor: LexicalEditor) => {
    const html = $generateHtmlFromNodes(editor, null);
    setValue("description", html);
  };

  return (
    <LexicalComposer initialConfig={defaultEditorConfigs}>
      <SharedHistoryContext>
        <TableContext>
          <SharedAutocompleteContext>
            <AsyncSubmitFormPlugin
              onSubmitAsync={handleSubmit(onFarmPost)}
              method="POST"
              clearAfterSubmit={true}
            >
              <FormRow className="row">
                <div className="col-6 col-lg-6 pr-lg-1">
                  <SecondaryTextbox
                    {...register("name", {
                      required: {
                        value: true,
                        message: "This field is required",
                      },
                      maxLength: {
                        value: 255,
                        message: "The text length cannot exceed the limit 255",
                      },
                    })}
                    defaultValue={currentFarm?.name}
                    autoComplete="off"
                    placeholder="Farm title"
                  />
                  {errors.name && (
                    <ValidationDangerMessage>
                      {errors.name.message?.toString()}
                    </ValidationDangerMessage>
                  )}
                </div>
                <div className="col-6 col-lg-6 pl-lg-1">
                  <Controller
                    control={control}
                    defaultValue={loadFarmTypeSelected()}
                    name="farmType"
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
                        cacheOptions
                        defaultOptions
                        loadOptions={(e) => loadFarmTypeSelections(e)}
                        isClearable={true}
                        placeholder="Select farm type"
                        onChange={handleSelectChange}
                      />
                    )}
                  />
                  {errors.farmType && (
                    <ValidationDangerMessage>
                      {errors.farmType.message?.toString()}
                    </ValidationDangerMessage>
                  )}
                </div>
              </FormRow>
              <FormRow className="row">
                <div className="col-9 col-lg-10 pr-lg-1">
                  <SecondaryTextbox
                    autoComplete="off"
                    {...register("address")}
                    defaultValue={currentFarm?.address}
                    placeholder="Address"
                  />
                </div>
                <div className="col-3 col-lg-2 pl-lg-1 pl-1">
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
              <div className="editor-shell">
                <Controller
                  control={control}
                  defaultValue={currentFarm?.description}
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
                      onChange={(editor: LexicalEditor) =>
                        onDescriptionChanged(editor)
                      }
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
