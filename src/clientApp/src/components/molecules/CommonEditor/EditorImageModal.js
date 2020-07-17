import React, { useState, useEffect } from "react";
import Tabs from "../Tabs/Tabs";
import EditorImageUploader from "./EditorImageUploader";
import EditorImageLink from "./EditorImageLink";
import styled from "styled-components";
import EditorImageInfo from "./EditorImageInfo";
import imageInfoModel from "../../../models/imageInfoModel";
import { ButtonPrimary, ButtonSecondary } from "../../atoms/Buttons/Buttons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { AtomicBlockUtils, EditorState } from "draft-js";

const Root = styled.div`
  min-width: 400px;
  margin: ${(p) => p.theme.size.tiny} ${(p) => p.theme.size.tiny} 0
    ${(p) => p.theme.size.tiny};
  min-height: 200px;
  background-color: ${(p) => p.theme.color.dark};
  border-radius: ${(p) => p.theme.borderRadius.normal};
`;

const Footer = styled.div`
  min-height: 20px;
  text-align: right;
  border-top: 1px solid ${(p) => p.theme.rgbaColor.darker};
  padding: ${(p) => p.theme.size.exTiny} ${(p) => p.theme.size.tiny};

  button {
    margin-left: ${(p) => p.theme.size.exTiny};
    font-weight: normal;
    padding: ${(p) => p.theme.size.exTiny} ${(p) => p.theme.size.tiny};

    svg {
      margin-right: ${(p) => p.theme.size.exTiny};
    }
  }
`;

const ImageEditorTabs = styled(Tabs)`
  ul.tabs-bar {
    border-bottom: 1px solid ${(p) => p.theme.rgbaColor.cyanLight};
  }
  ul.tabs-bar li button {
    background-color: transparent;
    color: ${(p) => p.theme.color.light};
  }

  ul.tabs-bar li.actived button {
    background-color: ${(p) => p.theme.rgbaColor.cyanLight};
    color: ${(p) => p.theme.color.lighter};
  }
`;

export default (props) => {
  const { editorState, convertImageCallback, onImageValidate } = props;
  const [imageData, setImageData] = useState(imageInfoModel);

  const clearImageData = () => {
    imageInfoModel.src.value = "";
    imageInfoModel.src.isValid = false;
    imageInfoModel.width.value = "auto";
    imageInfoModel.width.isValid = true;
    imageInfoModel.height.value = "auto";
    imageInfoModel.height.isValid = true;
    imageInfoModel.alt.value = "";
    imageInfoModel.alt.isValid = true;
    setImageData({
      ...imageInfoModel,
    });
  };

  const onClose = () => {
    clearImageData();
    props.onClose();
  };

  useEffect(() => {
    return () => {};
  }, []);

  const onAddImage = (e) => {
    props.onAccept(e);
  };

  const handleImageChange = async (e) => {
    var file = e.file;
    await convertImageCallback(file).then(async (fileData) => {
      await handleImageUrlChange(e.target.name, fileData.url);
    });
  };

  const handleImageUrlChange = async (formName, url) => {
    let formData = imageData || {};
    if (!url) {
      formData[formName].isValid = false;
      formData[formName].value = "";
      return;
    }

    await onImageValidate(url).then((response) => {
      const { errors, data } = response;
      if (errors || !data) {
        return;
      }

      const { validateImageUrl } = data;
      const { isSucceed } = validateImageUrl;
      formData[formName].isValid = isSucceed;
      formData[formName].value = url;
      setImageData({
        ...formData,
      });
    });
  };

  const onScaleChanged = (e) => {
    const value = e.target.value;
    const formData = imageData;
    if ("auto".indexOf(value) >= 0) {
      formData[e.target.name].value = value;
    } else if (!value || isNaN(value)) {
      formData[e.target.name].value = "auto";
    } else {
      const newSize = parseFloat(value);
      formData[e.target.name].value = newSize;
    }

    setImageData({
      ...formData,
    });
  };

  const handleInputChange = (evt) => {
    const formData = imageData || {};
    const { name, value } = evt.target;

    formData[name].value = value;
    setImageData({
      ...formData,
    });
  };

  const onImageAdded = () => {
    const { src, width, height, alt } = imageData;

    const contentState = editorState.getCurrentContent();
    const contentStateWithEntity = contentState.createEntity(
      "IMAGE",
      "IMMUTABLE",
      {
        src: src.value,
        height: height.value,
        width: width.value,
        alt: alt.value,
      }
    );
    const entityKey = contentStateWithEntity.getLastCreatedEntityKey();
    const newEditorState = EditorState.set(editorState, {
      currentContent: contentStateWithEntity,
    });

    const imageEditorState = AtomicBlockUtils.insertAtomicBlock(
      newEditorState,
      entityKey,
      " "
    );

    onAddImage(imageEditorState);
    onClose();
  };

  const onTabClicked = (e) => {
    clearImageData();
  };

  const { src } = imageData;
  const { isValid } = src;
  return (
    <Root>
      <ImageEditorTabs
        onTabClicked={onTabClicked}
        tabs={[
          {
            title: "Upload",
            tabComponent: () => (
              <EditorImageUploader
                onAddImage={onAddImage}
                onClose={onClose}
                editorState={editorState}
                handleImageChange={handleImageChange}
                onScaleChanged={onScaleChanged}
                handleInputChange={handleInputChange}
                imageData={imageData}
              />
            ),
          },
          {
            title: "Đường dẫn",
            tabComponent: () => (
              <EditorImageLink
                onAddImage={onAddImage}
                onClose={onClose}
                editorState={editorState}
                handleImageChange={handleImageUrlChange}
                onScaleChanged={onScaleChanged}
                handleInputChange={handleInputChange}
                imageData={imageData}
              />
            ),
          },
        ]}
      />
      <EditorImageInfo
        handleInfoChange={handleInputChange}
        onScaleChanged={onScaleChanged}
        imageData={imageData}
      />
      <Footer>
        <ButtonSecondary size="sm" onClick={onClose}>
          Đóng
        </ButtonSecondary>
        <ButtonPrimary size="sm" onClick={onImageAdded} disabled={!isValid}>
          <FontAwesomeIcon icon="check" />
          Lưu
        </ButtonPrimary>
      </Footer>
    </Root>
  );
};
