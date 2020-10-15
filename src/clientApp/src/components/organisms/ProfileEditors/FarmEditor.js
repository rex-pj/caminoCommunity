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
import FarmCreationModel from "../../../models/FarmCreationModel";
import { Thumbnail } from "../../molecules/Thumbnails";

const FormRow = styled.div`
  margin-bottom: ${(p) => p.theme.size.tiny};

  ${Textbox} {
    max-width: 100%;
    width: 100%;
  }

  .select {
    z-index: 10;
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
  } = props;
  const initialFormData = FarmCreationModel;
  const [formData, setFormData] = useState(initialFormData);
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

  const handleSelectChange = (e) => {
    let data = formData || {};
    const name = "farmTypeId";
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

    data.thumbnails.value.push({
      base64Data: preview,
      fileName: name,
      contentType: type,
    });

    setFormData({
      ...data,
    });
  };

  const onFarmPost = async (e) => {
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
      const farmData = {};
      for (const formIdentifier in formData) {
        farmData[formIdentifier] = formData[formIdentifier].value;
      }

      await props.onFarmPost(farmData).then((response) => {
        console.log(response);
        var { data } = response;
        var { createFarm } = data;
        if (createFarm && createFarm.id) {
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

  const removeImage = (e) => {
    let data = formData || {};

    var index = data.thumbnails.value.indexOf(e);
    data.thumbnails.value.splice(index, 1);

    setFormData({
      ...data,
    });
  };

  const { name, address, farmTypeId, thumbnails } = formData;
  return (
    <Fragment>
      <form onSubmit={(e) => onFarmPost(e)} method="POST">
        <FormRow className="row">
          <div className="col-6 col-lg-6 pr-lg-1">
            <Textbox
              name="name"
              value={name.value}
              autoComplete="off"
              onChange={(e) => handleInputChange(e)}
              placeholder="Farm title"
            />
          </div>
          <div className="col-6 col-lg-6 pl-lg-1">
            {farmTypeId.value ? (
              <AsyncSelect
                className="select"
                cacheOptions
                defaultOptions
                onChange={handleSelectChange}
                loadOptions={loadOptions}
                isClearable={true}
              />
            ) : (
              <AsyncSelect
                className="select"
                value=""
                cacheOptions
                defaultOptions
                onChange={handleSelectChange}
                loadOptions={loadOptions}
                isClearable={true}
                placeholder="Farm type"
              />
            )}
          </div>
        </FormRow>
        <FormRow className="row">
          <div className="col-9 col-lg-10 pr-lg-1">
            <Textbox
              name="address"
              value={address.value}
              autoComplete="off"
              onChange={(e) => handleInputChange(e)}
              placeholder="Address"
            />
          </div>
          <div className="col-3 col-lg-2 pl-lg-1 pl-1">
            <ThumbnailUpload onChange={handleImageChange}></ThumbnailUpload>
          </div>
        </FormRow>
        {thumbnails.value ? (
          <FormRow className="row">
            {thumbnails.value.map((item, index) => {
              return (
                <div className="col-3" key={index}>
                  <ImageEditBox>
                    <Thumbnail src={item.base64Data}></Thumbnail>
                    <RemoveImageButton onClick={removeImage}>
                      <FontAwesomeIcon icon="times"></FontAwesomeIcon>
                    </RemoveImageButton>
                  </ImageEditBox>
                </div>
              );
            })}
          </FormRow>
        ) : null}
        <CommonEditor
          height={height}
          convertImageCallback={convertImageCallback}
          onImageValidate={onImageValidate}
          placeholder="Enter the description here"
          onChanged={onDescriptionChanged}
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
