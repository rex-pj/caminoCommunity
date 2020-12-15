import React, { Fragment, useState, useRef } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { withRouter } from "react-router-dom";
import CommonEditor from "../CommonEditor";
import { Textbox } from "../../atoms/Textboxes";
import { ButtonPrimary } from "../../atoms/Buttons/Buttons";
import { checkValidity } from "../../../utils/Validity";
import styled from "styled-components";
import { stateToHTML } from "draft-js-export-html";
import ImageUpload from "../UploadControl/ImageUpload";
import AsyncSelect from "react-select/async";
import ArticleCreationModel from "../../../models/ArticleCreationModel";
import { Thumbnail } from "../../molecules/Thumbnails";

const FormRow = styled.div`
  margin-bottom: ${(p) => p.theme.size.tiny};

  ${Textbox} {
    max-width: 100%;
    width: 500px;
  }

  ${AsyncSelect} {
    max-width: 100%;
    z-index: 10;
    position: relative;
  }
`;

const ThumbnailUpload = styled(ImageUpload)`
  text-align: center;
  margin: auto;
  display: inline-block;
  vertical-align: middle;

  > span {
    color: ${(p) => p.theme.color.neutral};
    height: ${(p) => p.theme.size.normal};
    padding: 0 ${(p) => p.theme.size.tiny};
    font-size: ${(p) => p.theme.fontSize.tiny};
    background-color: ${(p) => p.theme.color.lighter};
    border-radius: ${(p) => p.theme.borderRadius.normal};
    border: 1px solid ${(p) => p.theme.color.neutral};
    cursor: pointer;
    font-weight: 600;

    :hover {
      background-color: ${(p) => p.theme.color.light};
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

export default withRouter((props) => {
  const {
    convertImageCallback,
    onImageValidate,
    height,
    filterCategories,
    currentArticle,
  } = props;
  const initialFormData = JSON.parse(JSON.stringify(ArticleCreationModel));
  const articleState = currentArticle ? currentArticle : initialFormData;
  const [formData, setFormData] = useState(articleState);
  const editorRef = useRef();

  const handleInputChange = (evt) => {
    let data = formData || {};
    const { name, value } = evt.target;

    data[name].isValid = checkValidity(data, value, name);
    data[name].value = value;

    setFormData({
      ...data,
    });
  };

  const onContentChanged = (editorState) => {
    const contentState = editorState.getCurrentContent();
    const html = stateToHTML(contentState);

    let data = formData || {};

    data["content"].isValid = checkValidity(data, html, "content");
    data["content"].value = html;

    setFormData({
      ...data,
    });
  };

  const fetchCategories = async (value) => {
    return await filterCategories(value).then((response) => {
      return response;
    });
  };

  const loadOptions = (value, callback) => {
    setTimeout(async () => {
      callback(await fetchCategories(value));
    }, 1000);
  };

  const loadSelected = () => {
    return {
      label: articleCategoryName.value,
      value: articleCategoryId.value,
    };
  };

  const handleSelectChange = (e) => {
    let data = formData || {};
    const name = "articleCategoryId";

    if (!e) {
      data[name].isValid = false;
      data[name].value = 0;
    } else {
      const { value } = e;
      data[name].isValid = checkValidity(data, value, name);
      data[name].value = parseFloat(value);
    }

    setFormData({
      ...data,
    });
  };

  const handleImageChange = (e) => {
    let data = formData || {};
    const { preview, file } = e;
    const { name, type } = file;

    data["thumbnail"].isValid = checkValidity(data, preview, "thumbnail");
    data["thumbnail"].value = {
      base64Data: preview,
      contentType: type,
      fileName: name,
    };

    setFormData({
      ...data,
    });
  };

  const onArticlePost = async (e) => {
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
    }

    if (!!isFormValid) {
      const articleData = {};
      for (const formIdentifier in formData) {
        articleData[formIdentifier] = formData[formIdentifier].value;
      }

      delete articleData["articleCategoryName"];
      await props.onArticlePost(articleData).then((response) => {
        if (response && response.id) {
          clearFormData();
        }
      });
    }
  };

  const clearFormData = () => {
    for (let formIdentifier in formData) {
      formData[formIdentifier].value = "";
    }

    editorRef.current.clearEditor();
    setFormData({
      ...formData,
    });
  };

  const onImageRemoved = () => {
    let data = formData || {};
    data["thumbnail"].isValid = false;
    data["thumbnail"].value = {
      pictureId: 0,
      fileName: "",
      contentType: "",
      base64Data: "",
    };

    setFormData({
      ...data,
    });
  };

  const { name, articleCategoryId, articleCategoryName, thumbnail } = formData;
  let { value: thumbnailData } = thumbnail;

  return (
    <Fragment>
      <form onSubmit={(e) => onArticlePost(e)} method="POST">
        <FormRow className="row">
          <div className="col-12 col-lg-6 pr-lg-1">
            <Textbox
              name="name"
              value={name.value}
              autoComplete="off"
              onChange={(e) => handleInputChange(e)}
              placeholder="Post title"
            />
          </div>
          <div className="col-10 col-lg-4 px-lg-1 pr-1">
            {articleCategoryId.value ? (
              <AsyncSelect
                defaultValue={loadSelected()}
                cacheOptions
                defaultOptions
                onChange={handleSelectChange}
                loadOptions={loadOptions}
                isClearable={true}
              />
            ) : (
              <AsyncSelect
                defaultValue=""
                cacheOptions
                defaultOptions
                onChange={handleSelectChange}
                loadOptions={loadOptions}
                isClearable={true}
                placeholder="Post category"
              />
            )}
          </div>
          <div className="col-2 col-lg-2 pl-lg-1 pl-1">
            <ThumbnailUpload onChange={handleImageChange}></ThumbnailUpload>
          </div>
        </FormRow>
        {thumbnailData.base64Data ? (
          <FormRow className="row">
            <div className="col-3">
              <ImageEditBox>
                <Thumbnail src={thumbnailData.base64Data}></Thumbnail>
                <RemoveImageButton onClick={onImageRemoved}>
                  <FontAwesomeIcon icon="times"></FontAwesomeIcon>
                </RemoveImageButton>
              </ImageEditBox>
            </div>
          </FormRow>
        ) : null}
        {thumbnailData && thumbnailData.pictureId ? (
          <FormRow className="row">
            <div className="col-3">
              <ImageEditBox>
                <Thumbnail
                  src={`${process.env.REACT_APP_CDN_PHOTO_URL}${thumbnailData.pictureId}`}
                ></Thumbnail>
                <RemoveImageButton onClick={onImageRemoved}>
                  <FontAwesomeIcon icon="times"></FontAwesomeIcon>
                </RemoveImageButton>
              </ImageEditBox>
            </div>
          </FormRow>
        ) : null}
        <CommonEditor
          contentHtml={currentArticle ? currentArticle.content.value : null}
          height={height}
          convertImageCallback={convertImageCallback}
          onImageValidate={onImageValidate}
          placeholder="Enter the content here"
          onChanged={onContentChanged}
          ref={editorRef}
        />
        <Footer className="row mb-3">
          <div className="col-auto"></div>
          <div className="col-auto ml-auto">
            <ButtonPrimary size="sm">Post</ButtonPrimary>
          </div>
        </Footer>
      </form>
    </Fragment>
  );
});
