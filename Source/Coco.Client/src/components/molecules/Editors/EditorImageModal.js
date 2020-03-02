import React, { Fragment, useState } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import styled from "styled-components";
import { ButtonPrimary, ButtonSecondary } from "../../atoms/Buttons/Buttons";
import Tabs from "../Tabs/Tabs";
import { PanelBody } from "../../atoms/Panels";
import ImageUpload from "../UploadControl/ImageUpload";
import AvatarEditor from "react-avatar-editor";

const Body = styled(PanelBody)`
  padding: ${p => p.theme.size.tiny};
`;

const Footer = styled.div`
  min-height: 20px;
  text-align: right;
  border-top: 1px solid ${p => p.theme.color.light};
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
  margin: auto;
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

function ImageUploader(props) {
  const [cropData, setCropData] = useState({
    width: 255,
    height: 255,
    scale: 1
  });

  let photoEditor = null;
  const setEditorRef = editor => (photoEditor = editor);

  const [photoData, setPhotoData] = useState({
    src: null,
    oldImage: null,
    contentType: null,
    fileName: null
  });

  const onClose = () => {
    props.onClose();
  };

  const onAddImage = () => {
    props.onAddImage();
  };

  const onChangeImage = e => {
    setCropData({
      width: 250,
      height: 250,
      scale: 1
    });
    setPhotoData({
      ...photoData,
      contentType: e.file.type,
      fileName: e.file.name,
      src: e.preview
    });
  };

  function onLoadSuccess(e) {
    // canSubmit();
  }

  const { src } = photoData;
  return (
    <Fragment>
      <Body>
        <PhotoUpload onChange={e => onChangeImage(e)}>
          Chọn ảnh để upload
        </PhotoUpload>
        <AvatarEditor
          ref={setEditorRef}
          image={src}
          width={cropData.width}
          height={cropData.height}
          border={25}
          color={[0, 0, 0, 0.3]} // RGBA
          scale={cropData.scale}
          rotate={0}
          onLoadSuccess={onLoadSuccess}
        />
      </Body>
      <Footer>
        <ButtonSecondary size="sm" onClick={onClose}>
          Đóng
        </ButtonSecondary>
        <ButtonPrimary size="sm" onClick={onAddImage}>
          <FontAwesomeIcon icon="check" />
          Lưu
        </ButtonPrimary>
      </Footer>
    </Fragment>
  );
}

function ImageLinkAdd(props) {
  const onClose = () => {
    props.onClose();
  };

  const onAddImage = () => {
    props.onAddImage();
  };

  return (
    <Fragment>
      <Body>Image Linked</Body>
      <Footer>
        <ButtonSecondary size="sm" onClick={onClose}>
          Đóng
        </ButtonSecondary>
        <ButtonPrimary size="sm" onClick={onAddImage}>
          <FontAwesomeIcon icon="check" />
          Lưu
        </ButtonPrimary>
      </Footer>
    </Fragment>
  );
}

export default props => {
  const onClose = () => {
    props.onClose();
  };

  const onUploadImage = () => {};

  const onAddImageLink = () => {};

  return (
    <Fragment>
      <Tabs
        tabs={[
          {
            title: "Upload",
            tabComponent: () => (
              <ImageUploader onAddImage={onUploadImage} onClose={onClose} />
            )
          },
          {
            title: "Đường dẫn",
            tabComponent: () => (
              <ImageLinkAdd onAddImage={onAddImageLink} onClose={onClose} />
            )
          }
        ]}
      />
    </Fragment>
  );
};
