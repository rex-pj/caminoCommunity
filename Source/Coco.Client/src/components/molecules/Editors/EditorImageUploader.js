import React, { Fragment, useState } from "react";
import ImageUpload from "../UploadControl/ImageUpload";
import styled from "styled-components";
import { PanelBody } from "../../atoms/Panels";
import { ButtonPrimary, ButtonSecondary } from "../../atoms/Buttons/Buttons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { Image } from "../../atoms/Images";
import { Textbox } from "../../atoms/Textboxes";
import NoImage from "../../atoms/NoImages/no-image";
import { AtomicBlockUtils } from "draft-js";

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
  text-align: center;
  max-width: 450px;
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

  const onChangeImage = e => {
    const { uploadCallback } = props;
    var file = e.file ? e.file : e;
    uploadCallback(file).then(({ data }) => {
      this.setState({
        src: data.link || data.url
      });
    });
  };

  const onAddImage = () => {
    const { src, width, height } = photoData;
    const { editorState, onAddImage } = props;
    const entityData = { src, height, width };

    const entityKey = editorState
      .getCurrentContent()
      .createEntity("IMAGE", "MUTABLE", entityData)
      .getLastCreatedEntityKey();
    const newEditorState = AtomicBlockUtils.insertAtomicBlock(
      editorState,
      entityKey,
      " "
    );

    onAddImage(newEditorState);
    onClose();
  };

  const onImageResizeing = file => {
    if (!file) {
      file = photoData.file;
    }

    if (file && width >= 50) {
      onImageResizsed(file);
    }
  };

  const onImageResizsed = src => {
    setPhotoData({
      ...photoData,
      src
    });
  };

  const onWithScaleChanged = e => {
    var value = e.target.value;
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

  const { src, fileName, width, height } = photoData;
  return (
    <Fragment>
      <Body>
        <PhotoUpload onChange={e => onChangeImage(e)}>
          Chọn ảnh để upload
        </PhotoUpload>
        <SliderWrap>
          <div className="row">
            <div className="col-md-6">
              <Textbox
                name="width"
                value={width}
                autoComplete="off"
                onChange={e => onWithScaleChanged(e)}
              />
            </div>
            <div className="col-md-6">
              <Textbox
                name="height"
                value={height}
                autoComplete="off"
                onChange={e => onWithScaleChanged(e)}
              />
            </div>
          </div>
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
