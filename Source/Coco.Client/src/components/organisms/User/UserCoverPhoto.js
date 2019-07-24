import React, { Component, Fragment } from "react";
import styled from "styled-components";
import { Thumbnail } from "../../molecules/Thumbnails";
import Overlay from "../../atoms/Overlay";
import { ButtonTransparent, ButtonPrimary } from "../../atoms/Buttons/Buttons";
import { ButtonOutlineNormal } from "../../atoms/Buttons/OutlineButtons";
import { Mutation } from "react-apollo";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import ImageUpload from "../../molecules/UploadControl/ImageUpload";
import AvatarEditor from "react-avatar-editor";
import Slider from "rc-slider";
import {
  UPDATE_USER_COVER,
  DELETE_USER_COVER
} from "../../../utils/GraphQLQueries";
import NoImage from "../../atoms/NoImages/no-image";
import AlertPopover from "../../molecules/Popovers/AlertPopover";

const Wrap = styled.div`
    position: relative;
    height: inherit;
    display: block;

    a.cover-link {
      display: block;
      height: inherit;
    }

    .cover-thumbnail {
      height: inherit;
    }

    img {
      min-width: 100%;
      height: inherit;
    }
  `,
  NoImageDisplayed = styled(NoImage)`
    position: absolute;
    top: 0;
    bottom: 0;
    left: 0;
    right: 0;
    margin: auto;
    z-index: 0;
    font-size: calc(${p => p.theme.fontSize.giant} * 2);
    color: ${p => p.theme.color.light};
    svg,
    path {
      font-size: inherit;
      color: inherit;
    }
  `,
  ThumbnailOverlay = styled(Overlay)`
    height: 100px;
    top: auto;
    bottom: 0;
  `,
  FullOverlay = styled.div`
    position: absolute;
    top: 0;
    bottom: 0;
    left: 0;
    right: 0;
    background-color: ${p => p.theme.rgbaColor.darker};
    z-index: 2;
  `,
  EditButton = styled(ButtonTransparent)`
    position: absolute;
    border: 0;
    background-color: ${p => p.theme.rgbaColor.darker};
    left: ${p => p.theme.size.distance};
    top: ${p => p.theme.size.distance};
    z-index: 2;

    :hover {
      border: 1px solid ${p => p.theme.color.light};
      background-color: ${p => p.theme.color.lighter};
      color: ${p => p.theme.color.dark};
    }
  `,
  DeleteConfirm = styled.div`
    position: absolute;
    bottom: ${p => p.theme.size.distance};
    right: ${p => p.theme.size.distance};
    z-index: 4;
  `,
  DeletePopoverConfirm = styled(AlertPopover)`
    right: 0;
    min-width: 200px;

    ::after {
      left: auto;
      right: ${p => p.theme.size.distance};
    }

    button:first-child {
      margin-right: ${p => p.theme.size.exTiny};
    }
  `,
  Tools = styled.div`
    position: absolute;
    top: 0;
    bottom: 0;
    left: 0;
    right: 0;
    margin: auto;
    z-index: 3;
    height: ${p => p.theme.size.large};
    text-align: center;
  `,
  CoverImageUpload = styled(ImageUpload)`
    display: inline-block;
    margin-right: ${p => p.theme.size.exTiny};
    vertical-align: middle;
    cursor: pointer;
    text-align: center;
    position: relative;

    > span {
      border: 0;
      width: ${p => p.theme.size.large};
      height: ${p => p.theme.size.large};
      font-size: ${p => p.theme.fontSize.large};
      color: ${p => p.theme.color.lighter};
      border-radius: ${p => p.theme.borderRadius.normal};
      padding: ${p => p.theme.size.exSmall} 0;

      :hover {
        background-color: ${p => p.theme.rgbaColor.darker};
      }
    }

    svg {
      position: absolute;
      top: 0;
      bottom: 0;
      left: 0;
      right: 0;
      margin: auto;
    }
  `,
  CancelEditButton = styled(ButtonTransparent)`
    border: 0;
    width: ${p => p.theme.size.large};
    height: ${p => p.theme.size.large};
    font-size: ${p => p.theme.fontSize.large};
    color: ${p => p.theme.color.lighter};
    margin-left: ${p => p.theme.size.exTiny};
    vertical-align: middle;

    svg,
    path {
      color: inherit;
    }

    :hover {
      background-color: ${p => p.theme.rgbaColor.darker};
    }
  `,
  UpdateTools = styled.div`
    position: absolute;
    right: ${p => p.theme.size.distance};
    bottom: ${p => p.theme.size.distance};
  `,
  AcceptUpdateButton = styled(ButtonPrimary)`
    span {
      margin-left: ${p => p.theme.size.distance};
    }
    border: 1px solid ${p => p.theme.color.primary};
  `,
  CancelUpdateButton = styled(ButtonOutlineNormal)`
    span {
      margin-left: ${p => p.theme.size.distance};
    }
    margin-left: ${p => p.theme.size.exTiny};
  `,
  SliderWrap = styled.div`
    max-width: 300px;
    margin: 0 auto;
    position: absolute;
    bottom: ${p => p.theme.size.distance};
    z-index: 4;
    left: 0;
    right: 0;
  `;

export default class extends Component {
  constructor(props) {
    super(props);

    this.state = {
      isInUpdateMode: false,
      contentType: null,
      fileName: null,
      src: null,
      isDisabled: true,
      crop: {
        width: 1044,
        height: 300,
        scale: 1
      },
      showDeletePopover: false
    };

    this.setEditorRef = editor => (this.editor = editor);
  }

  canSubmit = () => {
    const { canEdit } = this.props;
    const image = this.editor.getImage();

    const isValid = image.width > 1000 && image.height > 300;

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

  turnOnUpdateMode = () => {
    this.setState({
      isInUpdateMode: true
    });

    if (this.props.onToggleEditMode) {
      this.props.onToggleEditMode(true);
    }
  };

  turnOffUpdateMode = () => {
    this.setState({
      isInUpdateMode: false,
      src: null
    });

    if (this.props.onToggleEditMode) {
      this.props.onToggleEditMode(false);
    }
  };

  onChangeImage = e => {
    this.setState({
      contentType: e.file.type,
      fileName: e.file.name,
      src: e.preview
    });
  };

  onUpdateScale = e => {
    this.canSubmit();
    let { crop } = this.state;
    crop = {
      ...crop,
      scale: e
    };

    this.setState({
      crop
    });
  };

  onUpdate = async (e, updateUserCover) => {
    const { src } = this.state;
    if (this.editor && this.props.onUploaded && src) {
      const { fileName, contentType, crop } = this.state;
      const { scale } = crop;
      const { canEdit } = this.props;
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

      return await updateUserCover({ variables })
        .then(response => {
          this.props.onUploaded({
            avatarUrl: src
          });

          this.turnOffUpdateMode();
          this.props.closeModal();
        })
        .catch(error => {});
    }
  };

  delete = async deleteCover => {
    const { canEdit } = this.props;

    if (!canEdit) {
      return;
    }

    const variables = {
      criterias: {
        canEdit: canEdit
      }
    };

    return await deleteCover({ variables })
      .then(response => {
        this.props.onUploaded({
          avatarUrl: null
        });

        this.turnOffUpdateMode();
        this.props.closeModal();
      })
      .catch(error => {});
  };

  onLoadSuccess = e => {
    const { canEdit } = this.props;

    const isValid = e.width > 1000 && e.height > 300;

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
    const { userInfo } = this.props;
    const { coverPhotoUrl } = userInfo;
    const {
      isInUpdateMode,
      src,
      crop,
      isDisabled,
      showDeletePopover
    } = this.state;

    if (src) {
      return (
        <Wrap>
          <AvatarEditor
            ref={this.setEditorRef}
            image={src}
            width={crop.width}
            height={crop.height}
            border={0}
            scale={crop.scale}
            rotate={0}
            onLoadSuccess={this.onLoadSuccess}
          />

          <UpdateTools>
            <Mutation mutation={UPDATE_USER_COVER}>
              {updateUserCover => {
                return (
                  <AcceptUpdateButton
                    size="sm"
                    onClick={e => this.onUpdate(e, updateUserCover)}
                    disabled={isDisabled}
                  >
                    <FontAwesomeIcon icon="check" />
                  </AcceptUpdateButton>
                );
              }}
            </Mutation>
            <CancelUpdateButton size="sm" onClick={this.turnOffUpdateMode}>
              <FontAwesomeIcon icon="times" />
            </CancelUpdateButton>
          </UpdateTools>
          <SliderWrap>
            <Slider onChange={this.onUpdateScale} min={1} max={5} step={0.1} />
          </SliderWrap>
        </Wrap>
      );
    }

    return (
      <Wrap>
        {!!isInUpdateMode ? (
          <Fragment>
            {coverPhotoUrl ? (
              <Thumbnail
                className="cover-thumbnail"
                src={`${
                  process.env.REACT_APP_CDN_COVER_PHOTO_API_URL
                }${coverPhotoUrl}`}
                alt=""
              />
            ) : (
              <span />
            )}

            <FullOverlay />
            <Tools>
              <CoverImageUpload onChange={e => this.onChangeImage(e)} />
              <CancelEditButton onClick={this.turnOffUpdateMode}>
                <FontAwesomeIcon icon="times" />
              </CancelEditButton>
            </Tools>
            <DeleteConfirm>
              <Mutation mutation={DELETE_USER_COVER}>
                {deleteCover => {
                  return (
                    <DeletePopoverConfirm
                      isShown={showDeletePopover}
                      target="DeleteCover"
                      title="Bạn có muốn xóa ảnh không?"
                      onExecute={e => this.delete(deleteCover)}
                    />
                  );
                }}
              </Mutation>
              <ButtonOutlineNormal id="DeleteCover" size="sm">
                <FontAwesomeIcon icon="trash-alt" />
              </ButtonOutlineNormal>
            </DeleteConfirm>
          </Fragment>
        ) : (
          <Fragment>
            <EditButton size="sm" onClick={this.turnOnUpdateMode}>
              <FontAwesomeIcon icon="pencil-alt" />
            </EditButton>
            <a href={userInfo.url} className="cover-link">
              {coverPhotoUrl ? (
                <Thumbnail
                  className="cover-thumbnail"
                  src={`${
                    process.env.REACT_APP_CDN_COVER_PHOTO_API_URL
                  }${coverPhotoUrl}`}
                  alt=""
                />
              ) : (
                <NoImageDisplayed />
              )}
              <ThumbnailOverlay />
            </a>
          </Fragment>
        )}
      </Wrap>
    );
  }
}
