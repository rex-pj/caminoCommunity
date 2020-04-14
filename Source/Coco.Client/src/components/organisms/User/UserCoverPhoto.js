import React, { Fragment, useState } from "react";
import styled from "styled-components";
import { Thumbnail } from "../../molecules/Thumbnails";
import Overlay from "../../atoms/Overlay";
import { useMutation } from "@apollo/react-hooks";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import ImageUpload from "../../molecules/UploadControl/ImageUpload";
import AvatarEditor from "react-avatar-editor";
import Slider from "rc-slider";
import NoImage from "../../atoms/NoImages/no-image";
import AlertPopover from "../../molecules/Popovers/AlertPopover";
import { ButtonTransparent, ButtonPrimary } from "../../atoms/Buttons/Buttons";
import {
  ButtonOutlineNormal,
  ButtonOutlineSecondary,
} from "../../atoms/Buttons/OutlineButtons";
import {
  UPDATE_USER_COVER,
  DELETE_USER_COVER,
} from "../../../utils/GraphQlQueries/mutations";

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
    font-size: calc(${(p) => p.theme.fontSize.giant} * 2);
    color: ${(p) => p.theme.color.light};
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
    background-color: ${(p) => p.theme.rgbaColor.darker};
    z-index: 2;
  `,
  EditButton = styled(ButtonTransparent)`
    position: absolute;
    border-radius: ${(p) => p.theme.borderRadius.large};
    border: 1px solid ${(p) => p.theme.rgbaColor.dark};
    background-color: ${(p) => p.theme.rgbaColor.darkLight};
    color: ${(p) => p.theme.color.white};
    right: ${(p) => p.theme.size.distance};
    top: ${(p) => p.theme.size.distance};
    z-index: 2;

    :hover {
      background-color: ${(p) => p.theme.rgbaColor.darker};
    }
  `,
  DeleteConfirm = styled.div`
    position: absolute;
    bottom: ${(p) => p.theme.size.distance};
    right: ${(p) => p.theme.size.distance};
    z-index: 4;
  `,
  DeletePopoverConfirm = styled(AlertPopover)`
    right: 0;
    min-width: 200px;

    ::after {
      left: auto;
      right: ${(p) => p.theme.size.distance};
    }

    button:first-child {
      margin-right: ${(p) => p.theme.size.exTiny};
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
    height: ${(p) => p.theme.size.large};
    text-align: center;
  `,
  CoverImageUpload = styled(ImageUpload)`
    display: inline-block;
    margin-right: ${(p) => p.theme.size.exTiny};
    vertical-align: middle;
    cursor: pointer;
    text-align: center;
    position: relative;

    > span {
      border: 0;
      width: ${(p) => p.theme.size.large};
      height: ${(p) => p.theme.size.large};
      font-size: ${(p) => p.theme.fontSize.large};
      color: ${(p) => p.theme.color.lighter};
      border-radius: ${(p) => p.theme.borderRadius.medium};
      padding: ${(p) => p.theme.size.exSmall} 0;

      :hover {
        background-color: ${(p) => p.theme.rgbaColor.darker};
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
    width: ${(p) => p.theme.size.large};
    height: ${(p) => p.theme.size.large};
    font-size: ${(p) => p.theme.fontSize.large};
    color: ${(p) => p.theme.color.lighter};
    margin-left: ${(p) => p.theme.size.exTiny};
    vertical-align: middle;
    border-radius: ${(p) => p.theme.borderRadius.medium};

    svg,
    path {
      color: inherit;
    }

    :hover {
      background-color: ${(p) => p.theme.rgbaColor.darker};
    }
  `,
  UpdateTools = styled.div`
    position: absolute;
    right: ${(p) => p.theme.size.distance};
    bottom: ${(p) => p.theme.size.distance};
  `,
  AcceptUpdateButton = styled(ButtonPrimary)`
    span {
      margin-left: ${(p) => p.theme.size.distance};
    }
    border: 1px solid ${(p) => p.theme.color.primary};
  `,
  CancelUpdateButton = styled(ButtonOutlineNormal)`
    span {
      margin-left: ${(p) => p.theme.size.distance};
    }
    margin-left: ${(p) => p.theme.size.exTiny};
  `,
  SliderWrap = styled.div`
    max-width: 300px;
    margin: 0 auto;
    position: absolute;
    bottom: ${(p) => p.theme.size.distance};
    z-index: 4;
    left: 0;
    right: 0;
  `;

export default (props) => {
  const [isInUpdateMode, setInUpdateMode] = useState(false);
  const [showDeletePopover] = useState(false);
  const [isDisabled, setDisabled] = useState(true);
  const [coverState, setCoverState] = useState({
    contentType: null,
    fileName: null,
    src: null,
  });

  const [coverCrop, setCoverCrop] = useState({
    width: 1044,
    height: 300,
    scale: 1,
  });

  let photoEditor = null;
  const setEditorRef = (editor) => (photoEditor = editor);

  const validateForSubmit = () => {
    const { canEdit } = props;
    const image = photoEditor.getImage();
    const isValid = image.width > 1000 && image.height > 300;

    let isSucceed = false;
    let message = null;
    if (canEdit && isValid) {
      isSucceed = true;
    }

    if (!canEdit) {
      message = "Bạn không được quyền sửa chữa thông tin này";
    }

    if (!isValid) {
      message = "Hình ảnh phải lớn hơn 1000px x 300px";
    }

    setDisabled(!isValid);

    return {
      isSucceed,
      message,
    };
  };

  const turnOnUpdateMode = () => {
    setInUpdateMode(true);

    if (props.onToggleEditMode) {
      props.onToggleEditMode(true);
    }
  };

  const turnOffUpdateMode = () => {
    setInUpdateMode(false);
    setCoverState({
      ...coverState,
      src: null,
    });

    if (props.onToggleEditMode) {
      props.onToggleEditMode(false);
    }
  };

  const onChangeImage = (e) => {
    setCoverState({
      contentType: e.file.type,
      fileName: e.file.name,
      src: e.preview,
    });
  };

  const onUpdateScale = (e) => {
    validateForSubmit();
    setCoverCrop({
      ...coverCrop,
      scale: e,
    });
  };

  const showValidationError = (title, message) => {
    props.showValidationError(title, message);
  };

  const [updateCover] = useMutation(UPDATE_USER_COVER);
  const onUpdate = async () => {
    const { src } = coverState;
    if (photoEditor && props.onUpdated && src) {
      const { fileName, contentType } = coverState;
      const { scale } = coverCrop;
      const { canEdit } = props;
      const rect = photoEditor.getCroppingRect();

      const validateResult = validateForSubmit();
      if (!validateResult.isSucceed) {
        showValidationError(
          "Bạn không thể thay đổi ảnh cover",
          validateResult.message
        );
        return;
      }

      const variables = {
        photoUrl: src,
        xAxis: rect.x,
        yAxis: rect.y,
        width: rect.width,
        height: rect.height,
        fileName,
        contentType,
        scale,
        canEdit: canEdit,
      };

      await props
        .onUpdated(updateCover, variables)
        .then(() => {
          turnOffUpdateMode();
        })
        .catch(() => {});
    }
  };

  const [deleteCover] = useMutation(DELETE_USER_COVER);
  const onDelete = async () => {
    const { canEdit } = props;

    if (!canEdit) {
      return;
    }

    await props
      .onUpdated(deleteCover, {
        canEdit: canEdit,
      })
      .then(() => {
        turnOffUpdateMode();
      });
  };

  const onLoadSuccess = (e) => {
    const { canEdit } = props;

    const isValid = e.width > 1000 && e.height > 300;

    if (canEdit && isValid) {
      setDisabled(false);
      return;
    }
    setDisabled(true);
  };

  const { userInfo } = props;
  const { coverPhotoUrl } = userInfo;
  const { src } = coverState;

  if (src) {
    return (
      <Wrap>
        <AvatarEditor
          ref={setEditorRef}
          image={src}
          width={coverCrop.width}
          height={coverCrop.height}
          border={0}
          scale={coverCrop.scale}
          rotate={0}
          onLoadSuccess={onLoadSuccess}
        />

        <UpdateTools>
          <AcceptUpdateButton
            size="sm"
            onClick={(e) => onUpdate(e)}
            disabled={isDisabled}
          >
            <FontAwesomeIcon icon="check" />
          </AcceptUpdateButton>
          <CancelUpdateButton size="sm" onClick={turnOffUpdateMode}>
            <FontAwesomeIcon icon="times" />
          </CancelUpdateButton>
        </UpdateTools>
        <SliderWrap>
          <Slider onChange={onUpdateScale} min={1} max={5} step={0.1} />
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
              src={`${process.env.REACT_APP_CDN_COVER_PHOTO_API_URL}${coverPhotoUrl}`}
              alt=""
            />
          ) : (
            <span />
          )}

          <FullOverlay />
          <Tools>
            <CoverImageUpload onChange={(e) => onChangeImage(e)} />
            <CancelEditButton onClick={turnOffUpdateMode}>
              <FontAwesomeIcon icon="times" />
            </CancelEditButton>
          </Tools>
          <DeleteConfirm>
            <DeletePopoverConfirm
              isShown={showDeletePopover}
              target="DeleteCover"
              title="Bạn có muốn xóa ảnh không?"
              onExecute={(e) => onDelete()}
            />
            <ButtonOutlineSecondary id="DeleteCover" size="sm">
              <FontAwesomeIcon icon="trash-alt" />
            </ButtonOutlineSecondary>
          </DeleteConfirm>
        </Fragment>
      ) : (
        <Fragment>
          <EditButton size="sm" onClick={turnOnUpdateMode}>
            <FontAwesomeIcon icon="pencil-alt" />
          </EditButton>
          <a href={userInfo.url} className="cover-link">
            {coverPhotoUrl ? (
              <Thumbnail
                className="cover-thumbnail"
                src={`${process.env.REACT_APP_CDN_COVER_PHOTO_API_URL}${coverPhotoUrl}`}
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
};
