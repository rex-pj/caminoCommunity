import React, { useState, useEffect } from "react";
import Tabs from "../Tabs/Tabs";
import EditorImageUploader from "./EditorImageUploader";
import EditorImageLink from "./EditorImageLink";
import styled from "styled-components";
import EditorImageInfo from "./EditorImageInfo";
import imageInfoModel from "../../../models/imageInfoModel";
import { ButtonPrimary, ButtonLight } from "../../atoms/Buttons/Buttons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { AtomicBlockUtils, EditorState } from "draft-js";
import { fileToBase64 } from "../../../utils/Helper";

const Root = styled.div`
  min-width: 400px;
  margin: ${(p) => p.theme.size.tiny} ${(p) => p.theme.size.tiny} 0
    ${(p) => p.theme.size.tiny};
  min-height: 200px;
  background-color: ${(p) => p.theme.color.darkBg};
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
    color: ${(p) => p.theme.color.neutralText};
  }

  ul.tabs-bar li.actived button {
    background-color: ${(p) => p.theme.rgbaColor.cyanLight};
    color: ${(p) => p.theme.color.neutralText};
  }
`;

const EditorImageModal = (props) => {
  const { editorState, convertImageCallback, onImageValidate } = props;
  const [imageData, setImageData] = useState(imageInfoModel);
  const [imageSrc, setImageSrc] = useState("");

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
    const file = e.file;
    await convertImageCallback(file).then(async (fileData) => {
      const { file } = fileData;
      await handleImageUrlChange(e.target.name, file);
    });
  };

  const handleImageUrlChange = async (formName, file) => {
    let data = imageData || {};
    if (!file) {
      data[formName].isValid = false;
      data[formName].value = "";
      return;
    }

    let formData = new FormData();
    formData.append("file", file);
    await onImageValidate(formData)
      .then(async (response) => {
        data[formName].isValid = true;
        data[formName].value = file;
        const previewSrc = await fileToBase64(file);
        setImageSrc(previewSrc);
        setImageData({
          ...data,
        });
      })
      .catch((error) => {
        data[formName].isValid = false;
        data[formName].value = null;
        setImageSrc("");
        setImageData({
          ...data,
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
    const { width, height, alt } = imageData;

    const contentState = editorState.getCurrentContent();
    const contentStateWithEntity = contentState.createEntity(
      "IMAGE",
      "IMMUTABLE",
      {
        src: imageSrc,
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
                imageSrc={imageSrc}
              />
            ),
          },
          {
            title: "Link",
            tabComponent: () => (
              <EditorImageLink
                onAddImage={onAddImage}
                onClose={onClose}
                editorState={editorState}
                handleImageChange={handleImageUrlChange}
                onScaleChanged={onScaleChanged}
                handleInputChange={handleInputChange}
                imageData={imageData}
                imageSrc={imageSrc}
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
        <ButtonLight size="sm" onClick={onClose}>
          Đóng
        </ButtonLight>
        <ButtonPrimary size="sm" onClick={onImageAdded} disabled={!isValid}>
          <FontAwesomeIcon icon="check" />
          Lưu
        </ButtonPrimary>
      </Footer>
    </Root>
  );
};

export default EditorImageModal;
