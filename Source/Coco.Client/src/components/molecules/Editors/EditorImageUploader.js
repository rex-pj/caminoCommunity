import React, { Fragment, useState } from "react";
import ImageUpload from "../UploadControl/ImageUpload";
import styled from "styled-components";
import { PanelBody } from "../../atoms/Panels";
import { ButtonPrimary, ButtonSecondary } from "../../atoms/Buttons/Buttons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { Image } from "../../atoms/Images";
import { Textbox } from "../../atoms/Textboxes";
import NoImage from "../../atoms/NoImages/no-image";
import { LabelNormal } from "../../../components/atoms/Labels";
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

const ImageWrap = styled.div`
  text-align: center;
`;

const FormInput = styled.div`
  text-align: center;
  margin: ${p => p.theme.size.tiny} auto;

  svg {
    color: ${p => p.theme.color.light};
    margin-right: ${p => p.theme.size.exTiny};
    font-size: ${p => p.theme.size.small};
    path {
      color: inherit;
    }
  }

  ${LabelNormal} {
    color: ${p => p.theme.color.light};
    margin-right: ${p => p.theme.size.exTiny};
    font-weight: bold;
    font-size: ${p => p.theme.size.distance};
  }

  .image-title {
    width: 300px;
  }
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
    src: "",
    width: 700,
    alt: "",
    height: "auto"
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
    const entityData = { src, height, width, alt };

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

    formData[name] = value;
    setPhotoData({
      ...formData
    });
  };

  const { src, alt, width, height } = photoData;
  return (
    <Fragment>
      <Body>
        <PhotoUpload onChange={e => onChangeImage(e)}>
          Chọn ảnh để upload
        </PhotoUpload>
        {src ? (
          <Fragment>
            <FormInput>
              <div className="row">
                <div className="col-md-12">
                  <LabelNormal>Title</LabelNormal>
                  <Textbox
                    className="image-title"
                    name="alt"
                    value={alt}
                    autoComplete="off"
                    onChange={e => handleInputChange(e)}
                  />
                </div>
              </div>
            </FormInput>
            <FormInput>
              <div className="row">
                <div className="col-md-6">
                  <FontAwesomeIcon icon="arrows-alt-h" />
                  <Textbox
                    name="width"
                    value={width}
                    autoComplete="off"
                    onChange={e => onWithScaleChanged(e)}
                  />
                </div>
                <div className="col-md-6">
                  <FontAwesomeIcon icon="arrows-alt-v" />
                  <Textbox
                    name="height"
                    value={height}
                    autoComplete="off"
                    onChange={e => onWithScaleChanged(e)}
                  />
                </div>
              </div>
            </FormInput>
          </Fragment>
        ) : null}

        <ImageWrap>
          {src ? (
            <Image src={src} alt={alt} width={width} height={height} />
          ) : (
            <EmptyImage />
          )}
        </ImageWrap>
      </Body>
      <Footer>
        <ButtonSecondary size="sm" onClick={onClose}>
          Đóng
        </ButtonSecondary>
        <ButtonPrimary size="sm" onClick={onUploadImage}>
          <FontAwesomeIcon icon="check" />
          Thêm ảnh
        </ButtonPrimary>
      </Footer>
    </Fragment>
  );
};
