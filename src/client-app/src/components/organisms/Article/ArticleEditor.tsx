import * as React from "react";
import { useState, useEffect, useMemo } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { SecondaryTextbox } from "../../atoms/Textboxes";
import { ButtonIconSecondary } from "../../molecules/ButtonIcons";
import styled from "styled-components";
import { ImageUpload, ImageUploadOnChangeEvent } from "../UploadControl/ImageUpload";
import AsyncSelect from "react-select/async";
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
import { RichTextEditor } from "../CommonEditor/RichTextEditor";
import { defaultEditorConfigs } from "../CommonEditor/configs";
import AsyncSubmitFormPlugin from "../CommonEditor/plugins/AsyncSubmitFormPlugin";
import { Controller, useForm } from "react-hook-form";
import { ValidationDangerMessage } from "../../ErrorMessage";

const FormRow = styled.div`
  margin-bottom: ${(p) => p.theme.size.tiny};

  ${SecondaryTextbox} {
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

interface Props {
  height?: number;
  filterCategories: (e: any) => Promise<any>;
  currentArticle?: any;
  showValidationError: (title: string, message: string) => void;
  onArticlePost: (e: any) => Promise<any>;
}

const ArticleEditor = (props: Props) => {
  const { filterCategories, currentArticle } = props;
  const [isSubmitted, setSubmitted] = useState(false);
  const [preview, setPreview] = useState<string>("");
  const {
    register,
    handleSubmit,
    setValue,
    reset,
    control,
    formState: { errors },
  } = useForm();

  const handleImageChange = (e: ImageUploadOnChangeEvent) => {
    const { preview: srcPreview, file } = e;

    setPreview(srcPreview ?? "");
    setValue("picture", { file });
  };

  const onArticlePost = async (articleForm: any) => {
    setSubmitted(true);
    const requestData = new FormData();
    for (const key of Object.keys(articleForm)) {
      if (key === "articleCategory") {
        const { value: articleCategoryId } = articleForm[key];
        requestData.append("articleCategoryId", articleCategoryId);
        continue;
      }

      if (key !== "picture") {
        requestData.append(key, articleForm[key]);
        continue;
      }

      const picture = articleForm[key];
      if (!picture) {
        continue;
      }

      if (picture.file) {
        requestData.append(`picture.file`, picture.file);
      }

      if (picture.pictureId) {
        requestData.append(`file.pictureId`, picture.pictureId);
      }
    }

    return await props
      .onArticlePost(requestData)
      .then((response: any) => {
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
      articleCategory: null,
      content: null,
      picture: null,
    }));
    setPreview("");
  };

  const onImageRemoved = () => {
    reset((formValues) => ({
      ...formValues,
      picture: null,
    }));
    setPreview("");
  };

  const loadCategorySelections = useMemo(() => {
    return async (value: string) => {
      return await filterCategories({
        variables: {
          criterias: { query: value, isParentOnly: true },
        },
      })
        .then((response: any) => {
          const { data } = response;
          const { selections } = data;
          const mappedSelections = mapSelectOptions(selections);
          return mappedSelections;
        })
        .catch((error) => {
          return [];
        });
    };
  }, [filterCategories]);

  const loadCategorySelected = () => {
    if (!currentArticle?.articleCategoryId) {
      return null;
    }

    const { articleCategoryName, articleCategoryId } = currentArticle;
    return {
      label: articleCategoryName,
      value: articleCategoryId,
    };
  };

  const onContentChanged = (editor: LexicalEditor) => {
    const html = $generateHtmlFromNodes(editor, null);
    setValue("content", html);
  };

  const handleSelectChange = (newValue: OnChangeValue<any, any>, actionMeta: ActionMeta<any>) => {
    const { action } = actionMeta;
    if (action === "clear" || action === "remove-value") {
      setValue("articleCategory", null);
    } else {
      setValue("articleCategory", newValue);
    }
  };

  useEffect(() => {
    if (currentArticle?.picture) {
      const { picture } = currentArticle;
      if (picture?.pictureId) {
        const { pictureId } = picture;
        setValue("picture", { pictureId });
        setPreview(`${apiConfig.paths.pictures.get.getPicture}/${picture.pictureId}`);
      }
    }
  }, [currentArticle]);

  return (
    <LexicalComposer initialConfig={defaultEditorConfigs}>
      <SharedHistoryContext>
        <TableContext>
          <SharedAutocompleteContext>
            <AsyncSubmitFormPlugin onSubmitAsync={handleSubmit(onArticlePost)} method="POST" clearAfterSubmit={true}>
              <FormRow className="row">
                <div className="col-12 col-lg-6 pr-lg-1 mb-2 mb-lg-0">
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
                    defaultValue={currentArticle?.name}
                    autoComplete="off"
                    placeholder="Post title"
                  />
                  {errors.name && <ValidationDangerMessage>{errors.name.message?.toString()}</ValidationDangerMessage>}
                </div>
                <div className="col-10 col-lg-4 px-lg-1 pr-1 me-auto">
                  <Controller
                    control={control}
                    defaultValue={loadCategorySelected()}
                    name="articleCategory"
                    rules={{
                      required: {
                        value: true,
                        message: "This field is required",
                      },
                    }}
                    render={({ field }) => <AsyncSelect {...field} className="cate-selection" cacheOptions defaultOptions loadOptions={(e) => loadCategorySelections(e)} isClearable={true} placeholder="Select category" onChange={handleSelectChange} />}
                  />
                  {errors.articleCategory && <ValidationDangerMessage>{errors.articleCategory.message?.toString()}</ValidationDangerMessage>}
                </div>
                <div className="col">
                  <Controller control={control} name="picture" render={({ field }) => <ThumbnailUpload {...field} onChange={handleImageChange} />} />
                </div>
              </FormRow>
              {preview ? (
                <FormRow className="row">
                  <div className="col-3">
                    <ImageEditBox>
                      <Thumbnail src={preview}></Thumbnail>
                      <RemoveImageButton onClick={onImageRemoved}>
                        <FontAwesomeIcon icon="times"></FontAwesomeIcon>
                      </RemoveImageButton>
                    </ImageEditBox>
                  </div>
                </FormRow>
              ) : null}

              <div className="editor-shell">
                <Controller
                  control={control}
                  defaultValue={currentArticle?.content}
                  name="content"
                  rules={{
                    required: {
                      value: true,
                      message: "This field is required",
                    },
                  }}
                  render={({ field }) => <RichTextEditor {...field} onChange={(editor: LexicalEditor) => onContentChanged(editor)} />}
                />
                {errors.content && <ValidationDangerMessage>{errors.content.message?.toString()}</ValidationDangerMessage>}
              </div>

              <Footer className="row mb-3">
                <div className="col-auto"></div>
                <div className="col-auto ms-auto">
                  <ButtonIconSecondary disabled={isSubmitted} size="xs" icon={["far", "paper-plane"]}>
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

export default ArticleEditor;
