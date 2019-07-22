import React, { Component, Fragment } from "react";
import styled from "styled-components";
import { PanelFooter, PanelBody } from "../../atoms/Panels";
import { Button, ButtonSecondary, ButtonAlert } from "../../atoms/Buttons";
import { Image } from "../../atoms/Images";
import AvatarEditor from "react-avatar-editor";
import Slider from "rc-slider";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import ImageUpload from "../../molecules/UploadControl/ImageUpload";

const ImageWrap = styled.div`
  text-align: center;
  height: 80%;

  .ReactCrop {
    max-height: 450px;
  }

  img {
    max-width: 100%;
    height: 100%;
  }
`;

const DefaultImage = styled(Image)`
  width: 250px;
  height: 250px;
  display: block;
  margin: ${p => p.theme.size.medium} auto;
`;

const SliderWrap = styled.div`
  max-width: 300px;
  margin: 0 auto;
`;

const Tools = styled.div`
  width: 250px;
  margin: 10px auto;
  text-align: center;
`;

const AvatarUpload = styled(ImageUpload)`
  text-align: center;
  margin: auto;
  display: inline-block;
  vertical-align: middle;

  > span {
    color: ${p => p.theme.color.normal};
    height: ${p => p.theme.size.medium};
    padding: 0 ${p => p.theme.size.exTiny};
    background-color: ${p => p.theme.color.exLight};
    border-radius: ${p => p.theme.borderRadius.normal};
    border: 1px solid ${p => p.theme.color.normal};
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

const ButtonRemove = styled(ButtonAlert)`
  height: ${p => p.theme.size.medium};
  padding-top: 0;
  padding-bottom: 0;
  vertical-align: middle;
  margin-left: ${p => p.theme.size.exTiny};
`;

export default class extends Component {
  constructor(props) {
    super(props);

    const { imageUrl } = props.data;
    this.state = {
      src: null,
      oldImage: imageUrl,
      isDisabled: false,
      contentType: null,
      fileName: null,
      crop: {
        width: 250,
        height: 250,
        scale: 1
      }
    };
  }

  setEditorRef = editor => (this.editor = editor);

  onChangeImage = e => {
    this.setState({
      contentType: e.file.type,
      fileName: e.file.name,
      src: e.preview
    });
  };

  onExecute = e => {
    this.setState({
      isDisabled: true
    });
    if (this.editor) {
      const { src, fileName, contentType } = this.state;
      const rect = this.editor.getCroppingRect();

      if (this.props.onExecute) {
        this.props.onExecute({
          event: e,
          sourceImageUrl: src,
          xAxis: rect.x,
          yAxis: rect.y,
          width: rect.width,
          height: rect.height,
          fileName,
          contentType
        });
      }
    }

    this.setState({
      isDisabled: false
    });
  };

  onUpdateScale = e => {
    let { crop } = this.state;
    crop = {
      ...crop,
      scale: e
    };

    this.setState({
      crop
    });
  };

  render() {
    const { crop, src, isDisabled, oldImage } = this.state;
    return (
      <Fragment>
        <PanelBody>
          <Tools>
            <AvatarUpload onChange={e => this.onChangeImage(e)}>
              Đổi ảnh đại diện
            </AvatarUpload>
            <ButtonRemove size="sm">
              <FontAwesomeIcon icon="times" />
            </ButtonRemove>
          </Tools>

          {src ? (
            <Fragment>
              <ImageWrap>
                <AvatarEditor
                  ref={this.setEditorRef}
                  image={src}
                  width={crop.width}
                  height={crop.height}
                  border={50}
                  color={[255, 255, 255, 0.6]} // RGBA
                  scale={crop.scale}
                  rotate={0}
                />
              </ImageWrap>
              <SliderWrap>
                <Slider onChange={this.onUpdateScale} min={1} max={5} />
              </SliderWrap>
            </Fragment>
          ) : (
            <DefaultImage src={oldImage} alt={oldImage} />
          )}
        </PanelBody>
        <PanelFooter>
          <Button disabled={isDisabled} size="sm" onClick={this.onExecute}>
            Đồng ý
          </Button>
          <ButtonSecondary size="sm" onClick={() => this.props.closeModal()}>
            Hủy
          </ButtonSecondary>
        </PanelFooter>
      </Fragment>
    );
  }
}
