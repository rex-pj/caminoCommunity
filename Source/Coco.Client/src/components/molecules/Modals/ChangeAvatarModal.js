import React, { Component, Fragment } from "react";
import styled from "styled-components";
import { connect } from "react-redux";
import { PanelFooter, PanelBody } from "../../atoms/Panels";
import {
  Button,
  ButtonSecondary,
  ButtonAlert,
  ButtonOutlineDanger
} from "../../atoms/Buttons";
import { Image } from "../../atoms/Images";
import AvatarEditor from "react-avatar-editor";
import Slider from "rc-slider";
import { modalUploadAvatar, modalDeleteAvatar } from "../../../store/commands";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import ImageUpload from "../UploadControl/ImageUpload";

const Wrap = styled.div`
  margin: auto;
`;

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
    padding: 0 ${p => p.theme.size.tiny};
    background-color: ${p => p.theme.color.exLight};
    border-radius: ${p => p.theme.borderRadius.large};
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
  width: ${p => p.theme.size.medium};
  padding-top: 0;
  padding-bottom: 0;
  vertical-align: middle;
  border-radius: ${p => p.theme.borderRadius.large};
  margin-left: ${p => p.theme.size.exTiny};
`;

const LeftButtonFooter = styled.div`
  text-align: left;
`;

const FooterButtons = styled.div`
  button > span {
    margin-left: ${p => p.theme.size.tiny};
  }
`;

class ChangeAvatarModal extends Component {
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

    this.setEditorRef = editor => (this.editor = editor);
  }

  onChangeImage = e => {
    this.setState({
      contentType: e.file.type,
      fileName: e.file.name,
      src: e.preview
    });
  };

  onUpload = e => {
    this.setState({
      isDisabled: true
    });

    const { src } = this.state;
    if (this.editor && this.props.onUpload && src) {
      const { fileName, contentType } = this.state;
      const rect = this.editor.getCroppingRect();
      this.props.onUpload({
        sourceImageUrl: src,
        xAxis: rect.x,
        yAxis: rect.y,
        width: rect.width,
        height: rect.height,
        fileName,
        contentType
      });
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

  remove = () => {
    this.setState({
      contentType: null,
      fileName: null,
      src: null
    });
  };

  onDelete = () => {
    this.props.onDelete();
  };

  render() {
    const { crop, src, isDisabled, oldImage } = this.state;
    return (
      <Wrap>
        <PanelBody>
          <Tools>
            <AvatarUpload onChange={e => this.onChangeImage(e)}>
              Đổi ảnh đại diện
            </AvatarUpload>
            <ButtonRemove size="sm" onClick={this.remove}>
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
                  color={[0, 0, 0, 0.3]} // RGBA
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
          <FooterButtons className="row justify-content-between">
            <LeftButtonFooter className="col">
              <ButtonOutlineDanger size="sm" onClick={this.onDelete}>
                <FontAwesomeIcon icon="trash-alt" />
                <span>Xóa Ảnh</span>
              </ButtonOutlineDanger>
            </LeftButtonFooter>

            <div className="col">
              <Button disabled={isDisabled} size="sm" onClick={this.onUpload}>
                <FontAwesomeIcon icon="upload" />
                <span>Tải Ảnh</span>
              </Button>
              <ButtonSecondary
                size="sm"
                onClick={() => this.props.closeModal()}
              >
                <FontAwesomeIcon icon="times" />
                <span>Hủy</span>
              </ButtonSecondary>
            </div>
          </FooterButtons>
        </PanelFooter>
      </Wrap>
    );
  }
}

const mapDispatchToProps = dispatch => {
  return {
    onUpload: data => {
      modalUploadAvatar(dispatch, data);
    },
    onDelete: () => {
      modalDeleteAvatar(dispatch);
    }
  };
};

export default connect(
  null,
  mapDispatchToProps
)(ChangeAvatarModal);
