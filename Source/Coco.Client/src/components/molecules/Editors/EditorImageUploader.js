import React, { Fragment, useState } from "react";
import ImageUpload from "../UploadControl/ImageUpload";
import styled from "styled-components";
import { PanelBody } from "../../atoms/Panels";
import { ButtonPrimary, ButtonSecondary } from "../../atoms/Buttons/Buttons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import Resizer from "react-image-file-resizer";
import { Image } from "../../atoms/Images";
import Slider from "rc-slider";
import NoImage from "../../atoms/NoImages/no-image";

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

const ImageWrap = styled.div`
  text-align: center;
`;

const SliderWrap = styled.div`
  max-width: 300px;
  margin: ${p => p.theme.size.tiny} auto;
`;

const EmptyImage = styled(NoImage)`
  border-radius: ${p => p.theme.borderRadius.medium};
  width: 200px;
  height: 200px;
  display: inline-block;
  font-size: ${p => p.theme.size.large};
`;

export default props => {
  const [photoData, setPhotoData] = useState({
    src: null,
    oldImage: null,
    file: null,
    width: 700,
    height: 700
  });

  const onClose = () => {
    props.onClose();
  };

  const onAddImage = () => {
    props.onAddImage();
  };

  const onChangeImage = e => {
    var fileData = e.file ? e.file : e;
    onImageResizeing(fileData, 700, 700);
  };

  const onScaleChanged = e => {
    const { width, height } = photoData;
    const ratio = e / width;
    const newHeight = height * ratio;

    onImageResizeing(null, e, newHeight);
  };

  const onImageResizeing = (file, width, height) => {
    if (!file) {
      file = photoData.file;
    }

    if (file && width >= 50) {
      Resizer.imageFileResizer(
        file,
        width,
        height,
        "JPEG",
        100,
        0,
        uri => {
          onImageResizsed(file, uri, width, height);
        },
        "base64"
      );
    }
  };

  const onImageResizsed = (file, uri, width, height) => {
    setPhotoData({
      ...photoData,
      src: uri,
      file: file,
      width,
      height
    });
  };

  const { src, fileName, width } = photoData;
  return (
    <Fragment>
      <Body>
        <PhotoUpload onChange={e => onChangeImage(e)}>
          Chọn ảnh để upload
        </PhotoUpload>
        <SliderWrap>
          {src ? (
            <Slider
              onChange={onScaleChanged}
              min={50}
              max={700}
              step={0.5}
              value={width}
            />
          ) : null}
        </SliderWrap>
        <ImageWrap>
          {src ? <Image src={src} alt={fileName} /> : <EmptyImage />}
        </ImageWrap>
      </Body>
      <Footer>
        <ButtonSecondary size="sm" onClick={onClose}>
          Đóng
        </ButtonSecondary>
        <ButtonPrimary size="sm" onClick={onAddImage}>
          <FontAwesomeIcon icon="check" />
          Thêm ảnh
        </ButtonPrimary>
      </Footer>
    </Fragment>
  );
};
