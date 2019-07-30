import React, { Component, Fragment } from "react";
import styled from "styled-components";
import { Thumbnail } from "../../molecules/Thumbnails";
import Overlay from "../../atoms/Overlay";
import {
  ButtonTransparent,
  Button,
  ButtonOutlineNormal
} from "../../atoms/Buttons";
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
    background-color: ${p => p.theme.rgbaColor.exDark};
    z-index: 2;
  `,
  EditButton = styled(ButtonTransparent)`
    position: absolute;
    border: 0;
    background-color: ${p => p.theme.rgbaColor.exDark};
    left: ${p => p.theme.size.distance};
    top: ${p => p.theme.size.distance};

    :hover {
      border: 1px solid ${p => p.theme.color.light};
      background-color: ${p => p.theme.color.exLight};
      color: ${p => p.theme.color.dark};
    }
  `,
  DeleteButton = styled(ButtonOutlineNormal)`
    position: absolute;
    bottom: ${p => p.theme.size.distance};
    right: ${p => p.theme.size.distance};
    z-index: 4;
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

    > span {
      border: 0;
      width: ${p => p.theme.size.large};
      height: ${p => p.theme.size.large};
      font-size: ${p => p.theme.fontSize.large};
      color: ${p => p.theme.color.exLight};
      border-radius: ${p => p.theme.borderRadius.normal};
      padding: ${p => p.theme.size.exSmall} 0;

      :hover {
        background-color: ${p => p.theme.rgbaColor.exDark};
      }

      svg,
      path {
        color: inherit;
      }
    }
  `,
  CancelEditButton = styled(ButtonTransparent)`
    border: 0;
    width: ${p => p.theme.size.large};
    height: ${p => p.theme.size.large};
    font-size: ${p => p.theme.fontSize.large};
    color: ${p => p.theme.color.exLight};
    margin-left: ${p => p.theme.size.exTiny};
    vertical-align: middle;

    svg,
    path {
      color: inherit;
    }

    :hover {
      background-color: ${p => p.theme.rgbaColor.exDark};
    }
  `,
  UpdateTools = styled.div`
    position: absolute;
    right: ${p => p.theme.size.distance};
    bottom: ${p => p.theme.size.distance};
  `,
  AcceptUpdateButton = styled(Button)`
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
      crop: {
        width: 1044,
        height: 300,
        scale: 1
      }
    };

    this.setEditorRef = editor => (this.editor = editor);
  }

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
      const { fileName, contentType } = this.state;
      const { canEdit } = this.props;
      const rect = this.editor.getCroppingRect();

      if (!canEdit && this._isMounted) {
        this.setState({
          isDisabled: false
        });
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
          contentType
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

  delete = async (e, deleteCover) => {
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

  render() {
    const { userInfo } = this.props;
    const { coverPhotoUrl } = userInfo;
    const { isInUpdateMode, src, crop } = this.state;

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
          />

          <UpdateTools>
            <Mutation mutation={UPDATE_USER_COVER}>
              {updateUserCover => {
                return (
                  <AcceptUpdateButton
                    size="sm"
                    onClick={e => this.onUpdate(e, updateUserCover)}
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
        {!isInUpdateMode ? (
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
                <NoImage />
              )}
              <ThumbnailOverlay />
            </a>
          </Fragment>
        ) : (
          <Wrap>
            {coverPhotoUrl ? (
              <Thumbnail
                className="cover-thumbnail"
                src={`${
                  process.env.REACT_APP_CDN_COVER_PHOTO_API_URL
                }${coverPhotoUrl}`}
                alt=""
              />
            ) : (
              <NoImage />
            )}

            <FullOverlay />
            <Tools>
              <CoverImageUpload onChange={e => this.onChangeImage(e)} />
              <CancelEditButton onClick={this.turnOffUpdateMode}>
                <FontAwesomeIcon icon="times" />
              </CancelEditButton>
            </Tools>
            <Mutation mutation={DELETE_USER_COVER}>
              {deleteCover => {
                return (
                  <DeleteButton onClick={e => this.delete(e, deleteCover)}>
                    <FontAwesomeIcon icon="trash-alt" />
                  </DeleteButton>
                );
              }}
            </Mutation>
          </Wrap>
        )}
      </Wrap>
    );
  }
}
