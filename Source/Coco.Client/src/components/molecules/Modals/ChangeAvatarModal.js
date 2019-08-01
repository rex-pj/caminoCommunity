import React, { Component, Fragment } from "react";
import styled from "styled-components";
import { connect } from "react-redux";
import { Mutation } from "react-apollo";
import { PanelFooter, PanelBody } from "../../atoms/Panels";
import { ButtonOutlineDanger } from "../../atoms/Buttons/OutlineButtons";
import {
  ButtonPrimary,
  ButtonSecondary,
  ButtonAlert
} from "../../atoms/Buttons/Buttons";
import { Image } from "../../atoms/Images";
import AvatarEditor from "react-avatar-editor";
import Slider from "rc-slider";
import { avatarReload } from "../../../store/commands";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import ImageUpload from "../UploadControl/ImageUpload";
import AlertPopover from "../../molecules/Popovers/AlertPopover";
import {
  UPDATE_USER_AVATAR,
  DELETE_USER_AVATAR
} from "../../../utils/GraphQLQueries";

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
      isDisabled: true,
      contentType: null,
      fileName: null,
      crop: {
        width: 250,
        height: 250,
        scale: 1
      },
      showDeletePopover: false
    };

    this._isMounted = false;
    this.setEditorRef = editor => (this.editor = editor);
  }

  componentDidMount() {
    this._isMounted = true;
  }

  componentWillUnmount() {
    this._isMounted = false;
  }

  canSubmit = () => {
    const { data } = this.props;
    const { canEdit } = data;
    const image = this.editor.getImage();

    const isValid = image.width > 100 && image.height > 100;

    if (canEdit && isValid) {
      this.setState({
        isDisabled: false
      });
      return true;
    }
    this.setState({
      isDisabled: true
    });
    return false;
  };

  onChangeImage = e => {
    if (this._isMounted) {
      this.setState({
        contentType: e.file.type,
        fileName: e.file.name,
        src: e.preview
      });
    }
  };

  onUpdateScale = e => {
    if (this._isMounted) {
      this.canSubmit();
      let { crop } = this.state;
      crop = {
        ...crop,
        scale: e
      };

      this.setState({
        crop
      });
    }
  };

  remove = () => {
    if (this._isMounted) {
      this.setState({
        contentType: null,
        fileName: null,
        src: null
      });
    }
  };

  onUpload = async (e, uploadAvatar) => {
    if (this._isMounted) {
      this.setState({
        isDisabled: true
      });
    }

    const { src } = this.state;
    if (this.editor && this.props.onAvatarReload && src) {
      const { fileName, contentType, crop } = this.state;
      const { scale } = crop;
      const { data } = this.props;
      const { canEdit } = data;
      const rect = this.editor.getCroppingRect();
      if (!this.canSubmit()) {
        return;
      }

      const variables = {
        criterias: {
          photoUrl: src,
          canEdit: canEdit,
          xAxis: rect.x,
          yAxis: rect.y,
          width: rect.width,
          height: rect.height,
          fileName,
          contentType,
          scale
        }
      };

      return await uploadAvatar({ variables })
        .then(response => {
          this.props.closeModal();
          if (this._isMounted) {
            this.setState({
              isDisabled: false
            });
          }
          this.props.onAvatarReload();
        })
        .catch(error => {
          if (this._isMounted) {
            this.setState({
              isDisabled: false
            });
          }
        });
    }
  };

  onDelete = async (e, deleteAvatar) => {
    const { data } = this.props;
    const { canEdit } = data;

    if (!canEdit) {
      return;
    }

    const variables = {
      criterias: {
        canEdit: canEdit
      }
    };
    return await deleteAvatar({ variables })
      .then(response => {
        this.props.onAvatarReload();
        this.props.closeUploadModal();
      })
      .catch(error => {});
  };

  onLoadSuccess = e => {
    const { data } = this.props;
    const { canEdit } = data;

    const isValid = e.width > 100 && e.height > 100;

    if (canEdit && isValid) {
      this.setState({
        isDisabled: false
      });
      return;
    }
    this.setState({
      isDisabled: true
    });
  };

  render() {
    const { crop, src, isDisabled, oldImage, showDeletePopover } = this.state;
    return (
      <Wrap>
        <PanelBody>
          <Tools>
            <AvatarUpload onChange={e => this.onChangeImage(e)}>
              Đổi ảnh đại diện
            </AvatarUpload>
            {src ? (
              <ButtonRemove size="sm" onClick={this.remove}>
                <FontAwesomeIcon icon="times" />
              </ButtonRemove>
            ) : null}
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
                  onLoadSuccess={this.onLoadSuccess}
                />
              </ImageWrap>
              <SliderWrap>
                <Slider
                  onChange={this.onUpdateScale}
                  min={1}
                  max={5}
                  step={0.1}
                />
              </SliderWrap>
            </Fragment>
          ) : (
            <DefaultImage src={oldImage} alt={oldImage} />
          )}
        </PanelBody>
        <PanelFooter>
          <FooterButtons className="row justify-content-between">
            <LeftButtonFooter className="col">
              <Mutation mutation={DELETE_USER_AVATAR}>
                {deleteAvatar => {
                  return (
                    <AlertPopover
                      isShown={showDeletePopover}
                      target="DeleteAvatar"
                      title="Bạn có muốn xóa ảnh không?"
                      onExecute={e => this.onDelete(e, deleteAvatar)}
                    />
                  );
                }}
              </Mutation>

              <ButtonOutlineDanger size="sm" id="DeleteAvatar">
                <FontAwesomeIcon icon="trash-alt" />
                <span>Xóa Ảnh</span>
              </ButtonOutlineDanger>
            </LeftButtonFooter>

            <div className="col">
              <Mutation mutation={UPDATE_USER_AVATAR}>
                {updateAvatar => {
                  return (
                    <ButtonPrimary
                      disabled={isDisabled}
                      size="sm"
                      onClick={e => this.onUpload(e, updateAvatar)}
                    >
                      <FontAwesomeIcon icon="upload" />
                      <span>Tải Ảnh</span>
                    </ButtonPrimary>
                  );
                }}
              </Mutation>

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
    onAvatarReload: () => {
      avatarReload(dispatch);
    }
  };
};

export default connect(
  null,
  mapDispatchToProps
)(ChangeAvatarModal);
