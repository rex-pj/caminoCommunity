import React, { Fragment, useState, useContext } from "react";
import styled from "styled-components";
import { Thumbnail } from "../../molecules/Thumbnails";
import Overlay from "../../atoms/Overlay";
import { SessionContext } from "../../../store/context/session-context";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import ImageUpload from "../UploadControl/ImageUpload";
import AvatarEditor from "react-avatar-editor";
import Slider from "rc-slider";
import NoImage from "../../molecules/NoImages/no-image";
import AlertPopover from "../../molecules/Popovers/AlertPopover";
import { ButtonTransparent, ButtonPrimary } from "../../atoms/Buttons/Buttons";
import { ButtonOutlineLight } from "../../atoms/Buttons/OutlineButtons";
import { apiConfig } from "../../../config/api-config";
import { base64toFile } from "../../../utils/Helper";

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
    border-radius: 0;
    font-size: calc(${(p) => p.theme.fontSize.giant} * 2);
    color: ${(p) => p.theme.color.neutralText};
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
    color: ${(p) => p.theme.color.whiteText};
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
      color: ${(p) => p.theme.color.lightText};
      border-radius: ${(p) => p.theme.borderRadius.medium};
      padding: ${(p) => p.theme.size.exSmall} 0;
      border: 0;
      background-color: ${(p) => p.theme.rgbaColor.dark};

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
    width: ${(p) => p.theme.size.large};
    height: ${(p) => p.theme.size.large};
    font-size: ${(p) => p.theme.fontSize.large};
    color: ${(p) => p.theme.color.lightText};
    margin-left: ${(p) => p.theme.size.exTiny};
    vertical-align: middle;
    border-radius: ${(p) => p.theme.borderRadius.medium};
    border: 0;
    background-color: ${(p) => p.theme.rgbaColor.dark};

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
    border: 1px solid ${(p) => p.theme.color.primaryBg};
  `,
  CancelUpdateButton = styled(ButtonOutlineLight)`
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

const ProfileCover = (props) => {
  const [isInUpdateMode, setInUpdateMode] = useState(false);
  const [showDeletePopover] = useState(false);
  const [isDisabled, setDisabled] = useState(true);
  const { isLogin, currentUser } = useContext(SessionContext);
  const [coverState, setCoverState] = useState({
    fileName: null,
    src: null,
  });

  const [coverCrop, setCoverCrop] = useState({
    width: 1224,
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
      message = "Your are unauthorized to edit this information";
    }

    if (!isValid) {
      message = "The image size must be bigger than 1000px x 300px";
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
      file: base64toFile(e.preview, e.file.name),
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

  const onUpdate = async () => {
    const { src } = coverState;
    if (photoEditor && props.onUpdated && src) {
      const validateResult = validateForSubmit();
      if (!validateResult.isSucceed) {
        showValidationError(
          "Your are unable to update your cover photo",
          validateResult.message
        );
        return;
      }

      const rect = photoEditor.getCroppingRect();
      const { fileName, file } = coverState;
      const { scale } = coverCrop;

      let formData = new FormData();
      formData.append("xAxis", rect.x);
      formData.append("yAxis", rect.y);
      formData.append("width", rect.width);
      formData.append("height", rect.height);
      formData.append("fileName", fileName);
      formData.append("scale", scale);
      formData.append("file", file);

      await props
        .onUpdated(formData)
        .then(() => {
          turnOffUpdateMode();
        })
        .catch(() => {});
    }
  };

  const onDelete = async () => {
    if (!props.canEdit) {
      return;
    }

    await props.onDeleted().then(() => {
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
  const { userCover, userIdentityId } = userInfo;
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
            onClick={onUpdate}
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
          {userCover && userCover.id ? (
            <Thumbnail
              className="cover-thumbnail"
              src={`${apiConfig.paths.userPhotos.get.getAvatar}/${userCover.id}`}
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
              title="Please confirm your cover photo deletion?"
              onExecute={(e) => onDelete()}
            />
            <ButtonOutlineLight id="DeleteCover" size="sm">
              <FontAwesomeIcon icon="trash-alt" />
            </ButtonOutlineLight>
          </DeleteConfirm>
        </Fragment>
      ) : (
        <Fragment>
          {isLogin && currentUser.userIdentityId === userIdentityId ? (
            <EditButton size="sm" onClick={turnOnUpdateMode}>
              <FontAwesomeIcon icon="pencil-alt" />
            </EditButton>
          ) : null}

          <a href={userInfo.url} className="cover-link">
            {userCover && userCover.id ? (
              <Thumbnail
                className="cover-thumbnail"
                src={`${apiConfig.paths.userPhotos.get.getCover}/${userCover.id}`}
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

export default ProfileCover;
