import React, { Fragment, useState } from "react";
import ImageUpload from "../UploadControl/ImageUpload";
import styled from "styled-components";
import { PanelBody } from "../../atoms/Panels";
import { ButtonPrimary, ButtonSecondary } from "../../atoms/Buttons/Buttons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import Resizer from "react-image-file-resizer";
import Slider from "rc-slider";

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

const SliderWrap = styled.div`
  max-width: 300px;
  margin: 0 auto;
`;

export default props => {
  const [photoData, setPhotoData] = useState({
    src: null,
    oldImage: null,
    contentType: null,
    fileName: null,
    file: null
  });

  const [cropData, setCropData] = useState({
    width: 100,
    height: 100
  });

  const onClose = () => {
    props.onClose();
  };

  const onAddImage = () => {
    props.onAddImage();
  };

  const onChangeImage = e => {
    var fileData = e.file ? e.file : e;
    const { width, height } = cropData;
    if (fileData) {
      Resizer.imageFileResizer(
        fileData,
        width,
        height,
        "JPEG",
        100,
        0,
        uri => {
          onImageResizsed(fileData, uri);
        },
        "base64"
      );
    }
  };

  const onImageResizsed = (file, uri) => {
    setPhotoData({
      ...photoData,
      contentType: file.type,
      fileName: file.name,
      src: uri,
      file
    });
  };

  function onUpdateScale(e) {
    var { width, height } = cropData;
    var ratio = e / width;
    var newHeight = height * ratio;
    let crop = {
      ...cropData,
      width: e,
      height: newHeight
    };

    setCropData({
      ...crop
    });

    const { file } = photoData;
    onChangeImage(file);
  }

  const { src, fileName } = photoData;
  return (
    <Fragment>
      <Body>
        <PhotoUpload onChange={e => onChangeImage(e)}>
          Chọn ảnh để upload
        </PhotoUpload>
        <img src={src} alt={fileName} />
        <SliderWrap>
          <Slider onChange={onUpdateScale} min={100} max={700} step={1} />
        </SliderWrap>
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
};
