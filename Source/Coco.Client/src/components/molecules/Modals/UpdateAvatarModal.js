import React, { useState, useContext, Fragment } from "react";
import { useMutation } from "@apollo/client";
import styled from "styled-components";
import { PanelFooter, PanelBody } from "../../atoms/Panels";
import { ButtonOutlineDanger } from "../../atoms/Buttons/OutlineButtons";
import {
  ButtonPrimary,
  ButtonSecondary,
  ButtonAlert,
} from "../../atoms/Buttons/Buttons";
import {
  UPDATE_USER_AVATAR,
  DELETE_USER_AVATAR,
} from "../../../utils/GraphQLQueries/mutations";
import contentClient from "../../../utils/GraphQLClient/contentClient";
import { Image } from "../../atoms/Images";
import AvatarEditor from "react-avatar-editor";
import Slider from "rc-slider";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import ImageUpload from "../UploadControl/ImageUpload";
import AlertPopover from "../Popovers/AlertPopover";
import { SessionContext } from "../../../store/context/SessionContext";
import NoAvatar from "../../atoms/NoImages/no-avatar";

const Wrap = styled.div`
  margin: auto;
`;

const ImageWrap = styled.div`
  text-align: center;

  .ReactCrop {
    max-height: 450px;
  }

  img {
    max-width: 100%;
    height: 100%;
  }
`;

const DefaultImage = styled(Image)`
  max-width: 250px;
  max-height: 250px;
  display: block;
  margin: ${(p) => p.theme.size.medium} auto;
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
    color: ${(p) => p.theme.color.neutral};
    height: ${(p) => p.theme.size.medium};
    padding: 0 ${(p) => p.theme.size.tiny};
    background-color: ${(p) => p.theme.color.lighter};
    border-radius: ${(p) => p.theme.borderRadius.large};
    border: 1px solid ${(p) => p.theme.color.neutral};
    cursor: pointer;
    font-weight: 600;

    :hover {
      background-color: ${(p) => p.theme.color.light};
    }

    svg {
      display: inline-block;
      margin: 10px auto 0 auto;
    }
  }
`;

const ButtonRemove = styled(ButtonAlert)`
  height: ${(p) => p.theme.size.medium};
  width: ${(p) => p.theme.size.medium};
  padding-top: 0;
  padding-bottom: 0;
  vertical-align: middle;
  border-radius: ${(p) => p.theme.borderRadius.large};
  margin-left: ${(p) => p.theme.size.exTiny};
`;

const LeftButtonFooter = styled.div`
  text-align: left;
`;

const FooterButtons = styled.div`
  button > span {
    margin-left: ${(p) => p.theme.size.tiny};
  }
`;

const EmptyAvatar = styled(NoAvatar)`
  border-radius: ${(p) => p.theme.borderRadius.medium};
  width: 255px;
  height: 255px;
  font-size: ${(p) => p.theme.size.large};
  vertical-align: middle;
  margin: auto;
`;

const UpdateAvatarModal = (props) => {
  const { isDisabled } = props;
  const { imageUrl } = props.data;
  const [showDeletePopover] = useState(false);
  const [updateAvatar] = useMutation(UPDATE_USER_AVATAR, {
    client: contentClient,
  });
  const [deleteAvatar] = useMutation(DELETE_USER_AVATAR, {
    client: contentClient,
  });
  var sessionContext = useContext(SessionContext);

  const [cropData, setCropData] = useState({
    width: 255,
    height: 255,
    scale: 1,
  });

  const [avatarData, setAvatarData] = useState({
    src: null,
    oldImage: imageUrl,
    contentType: null,
    fileName: null,
  });

  let avatarEditor = null;
  const setEditorRef = (editor) => (avatarEditor = editor);

  function canSubmit() {
    const { data } = props;
    const { canEdit } = data;
    const image = avatarEditor.getImage();

    const isValid = image.width > 100 && image.height > 100;

    let isSucceed = false;
    if (canEdit && isValid) {
      isSucceed = true;
    }

    props.setDisabled(!isSucceed);

    return isSucceed;
  }

  function onChangeImage(e) {
    setCropData({
      width: 250,
      height: 250,
      scale: 1,
    });
    setAvatarData({
      ...avatarData,
      contentType: e.file.type,
      fileName: e.file.name,
      src: e.preview,
    });
  }

  function onUpdateScale(e) {
    canSubmit();
    let crop = {
      ...cropData,
      scale: e,
    };

    setCropData({
      ...crop,
    });
  }

  function remove() {
    setAvatarData({
      ...avatarData,
      contentType: null,
      fileName: null,
      src: null,
    });
  }

  const onUpload = async (e) => {
    props.setDisabled(true);

    const { src } = avatarData;
    if (avatarEditor && src) {
      const rect = avatarEditor.getCroppingRect();
      if (!canSubmit()) {
        return;
      }

      const { fileName, contentType } = avatarData;
      const { scale } = cropData;
      const { data } = props;
      const { canEdit } = data;

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
          scale,
        },
      };

      await props
        .onExecute(updateAvatar, { variables }, "AVATAR_UPDATED")
        .finally(async () => {
          props.setDisabled(false);
          await sessionContext.relogin();
        });
    }
  };

  const onDelete = async (e) => {
    const { data } = props;
    const { canEdit } = data;

    if (!canEdit) {
      return;
    }

    const variables = {
      criterias: {
        canEdit: canEdit,
      },
    };

    await props
      .onExecute(deleteAvatar, { variables }, "AVATAR_UPDATED")
      .finally(() => {
        props.setDisabled(false);
        sessionContext.relogin();
      });
  };

  function onLoadSuccess(e) {
    canSubmit();
  }

  const { src, oldImage } = avatarData;
  return (
    <Wrap>
      <PanelBody>
        <Tools>
          <AvatarUpload onChange={(e) => onChangeImage(e)}>
            Đổi ảnh đại diện
          </AvatarUpload>
          {src ? (
            <ButtonRemove size="sm" onClick={remove}>
              <FontAwesomeIcon icon="times" />
            </ButtonRemove>
          ) : null}
        </Tools>

        {src ? (
          <Fragment>
            <ImageWrap>
              <AvatarEditor
                ref={setEditorRef}
                image={src}
                width={cropData.width}
                height={cropData.height}
                border={25}
                color={[0, 0, 0, 0.3]} // RGBA
                scale={cropData.scale}
                rotate={0}
                onLoadSuccess={onLoadSuccess}
              />
            </ImageWrap>
            <SliderWrap>
              <Slider onChange={onUpdateScale} min={1} max={5} step={0.1} />
            </SliderWrap>
          </Fragment>
        ) : null}

        {!src && oldImage ? (
          <DefaultImage src={oldImage} alt={oldImage} />
        ) : null}

        {!src && !oldImage ? <EmptyAvatar /> : null}
      </PanelBody>
      <PanelFooter>
        <FooterButtons className="row justify-content-between">
          <LeftButtonFooter className="col">
            <AlertPopover
              isShown={showDeletePopover}
              target="DeleteAvatar"
              title="Bạn có muốn xóa ảnh không?"
              onExecute={(e) => onDelete(e)}
            />
            <ButtonOutlineDanger size="sm" id="DeleteAvatar">
              <FontAwesomeIcon icon="trash-alt" />
              <span>Xóa Ảnh</span>
            </ButtonOutlineDanger>
          </LeftButtonFooter>

          <div className="col">
            <ButtonPrimary
              disabled={isDisabled}
              size="sm"
              onClick={(e) => onUpload(e)}
            >
              <FontAwesomeIcon icon="upload" />
              <span>Tải Ảnh</span>
            </ButtonPrimary>

            <ButtonSecondary size="sm" onClick={() => props.closeModal()}>
              <FontAwesomeIcon icon="times" />
              <span>Hủy</span>
            </ButtonSecondary>
          </div>
        </FooterButtons>
      </PanelFooter>
    </Wrap>
  );
};

export default UpdateAvatarModal;
