import React, { Fragment, useState } from "react";
import ImageUpload from "../UploadControl/ImageUpload";
import styled from "styled-components";
import { PanelBody } from "../../atoms/Panels";
import { ButtonPrimary, ButtonSecondary } from "../../atoms/Buttons/Buttons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { AtomicBlockUtils, EditorState } from "draft-js";
import EditorImageScalePreview from "./EditorImageScalePreview";
import { validateImageLink } from "../../../utils/Validity";

const Body = styled(PanelBody)`
  padding: ${p => p.theme.size.tiny};
`;

const Footer = styled.div`
  min-height: 20px;
  text-align: right;
  border-top: 1px solid ${p => p.theme.rgbaColor.darker};
  padding: ${p => p.theme.size.exTiny} ${p => p.theme.size.tiny};

  button {
    margin-left: ${p => p.theme.size.exTiny};
    font-weight: normal;
    padding: ${p => p.theme.size.exTiny} ${p => p.theme.size.tiny};

    svg {
      margin-right: ${p => p.theme.size.exTiny};
    }
  }
`;

const PhotoUpload = styled(ImageUpload)`
  text-align: center;
  margin: 0 auto ${p => p.theme.size.tiny} auto;
  display: block;
  width: 200px;

  > span {
    color: ${p => p.theme.color.neutral};
    height: ${p => p.theme.size.medium};
    padding: 0 ${p => p.theme.size.tiny};
    background-color: ${p => p.theme.color.lighter};
    border-radius: ${p => p.theme.borderRadius.large};
    border: 1px solid ${p => p.theme.color.neutral};
    cursor: pointer;
    font-weight: 600;

    :hover {
      background-color: ${p => p.theme.color.light};
    }

    svg {
      display: inline-block;
      margin: 10px auto 0 auto;
    }
  }
`;

export default props => {
  const [photoData, setPhotoData] = useState({
    src: "",
    width: "auto",
    height: "auto",
    alt: "",
    isValid: false
  });

  const onClose = () => {
    props.onClose();
  };

  const onChangeImage = async e => {
    const { convertImageCallback } = props;
    var file = e.file;
    await convertImageCallback(file).then(data => {
      setPhotoData({
        ...photoData,
        src: data.url,
        alt: data.fileName
      });
    });
  };

  const onUploadImage = () => {
    const { src, width, height, alt } = photoData;
    const { editorState, onAddImage } = props;

    const contentState = editorState.getCurrentContent();
    const contentStateWithEntity = contentState.createEntity(
      "IMAGE",
      "IMMUTABLE",
      { src, height, width, alt }
    );
    const entityKey = contentStateWithEntity.getLastCreatedEntityKey();
    const newEditorState = EditorState.set(editorState, {
      currentContent: contentStateWithEntity
    });

    const imageEditorState = AtomicBlockUtils.insertAtomicBlock(
      newEditorState,
      entityKey,
      " "
    );

    onAddImage(imageEditorState);
    onClose();
  };

  const onWithScaleChanged = e => {
    const value = e.target.value;
    const formData = photoData;
    if ("auto".indexOf(value) >= 0) {
      formData[e.target.name] = value;
    } else if (!value || isNaN(value)) {
      formData[e.target.name] = "auto";
    } else {
      const newSize = parseFloat(value);
      formData[e.target.name] = newSize;
    }

    setPhotoData({
      ...formData
    });
  };

  const handleInputChange = evt => {
    const formData = photoData || {};
    const { name, value } = evt.target;
    formData.isValid = validateImageLink(value);

    formData[name] = value;
    setPhotoData({
      ...formData
    });
  };

  const { isValid } = photoData;
  return (
    <Fragment>
      <Body>
        <PhotoUpload onChange={e => onChangeImage(e)}>
          Chọn ảnh để upload
        </PhotoUpload>
        <EditorImageScalePreview
          {...photoData}
          handleInputChange={handleInputChange}
          onWithScaleChanged={onWithScaleChanged}
        />
      </Body>
      <Footer>
        <ButtonSecondary size="sm" onClick={onClose}>
          Đóng
        </ButtonSecondary>
        <ButtonPrimary size="sm" onClick={onUploadImage} disabled={!isValid}>
          <FontAwesomeIcon icon="check" />
          Thêm ảnh
        </ButtonPrimary>
      </Footer>
    </Fragment>
  );
};
