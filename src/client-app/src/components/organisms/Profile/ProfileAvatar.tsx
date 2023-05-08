import * as React from "react";
import styled from "styled-components";
import { ImageRound } from "../../atoms/Images";
import { ButtonPrimary } from "../../atoms/Buttons/Buttons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import NoAvatar from "../../molecules/NoImages/no-avatar";
import { useStore } from "../../../store/hook-store";
import UpdateAvatarModal from "../../organisms/Modals/UpdateAvatarModal";
import { apiConfig } from "../../../config/api-config";

const ProfileImage = styled(ImageRound)`
    display: block;
    min-width: 100%;
  `,
  EmptyAvatar = styled(NoAvatar)`
    border-radius: ${(p) => p.theme.borderRadius.normal};
    width: 100%;
    height: 100%;
  `,
  Wrap = styled.div`
    display: block;
    width: 110px;
    height: 110px;
  `,
  AvatarUpload = styled(ButtonPrimary)`
    position: absolute;
    top: -15px;
    right: -15px;
    z-index: 2;
    text-align: center;
    margin: auto;
    color: ${(p) => p.theme.color.whiteText};
    width: ${(p) => p.theme.size.medium};
    height: ${(p) => p.theme.size.medium};
    border-radius: 100%;
    padding: 0 ${(p) => p.theme.size.exTiny};
    background-color: ${(p) => p.theme.rgbaColor.dark};
    border: 1px solid ${(p) => p.theme.rgbaColor.darkLight};
    cursor: pointer;
    font-weight: 600;

    :hover {
      background-color: ${(p) => p.theme.rgbaColor.darker};
    }
  `,
  AvatarLink = styled.a`
    display: block;
    width: 100%;
    height: 100%;
    border: 3px solid ${(p) => p.theme.rgbaColor.cyan};
    background-color: ${(p) => p.theme.rgbaColor.cyan};
    border-radius: 7px;
  `;

const ProfileAvatar = ({ ...props }) => {
  const { userInfo, canEdit, onUpload, onDelete } = props;
  const { userAvatar } = userInfo;
  const dispatch = useStore(true)[1];

  function onOpenUploadModal(e) {
    dispatch("OPEN_MODAL", {
      data: {
        imageUrl:
          userAvatar && userAvatar.id
            ? `${apiConfig.paths.userPhotos.get.getAvatar}/${userAvatar.id}`
            : null,
        title: "Update Avatar",
        canEdit: userInfo.canEdit,
      },
      execution: {
        onUpload: onUpload,
        onDelete: onDelete,
      },
      options: {
        isOpen: true,
        innerModal: UpdateAvatarModal,
      },
    });
  }

  return (
    <Wrap className={props.className}>
      <AvatarLink href={userInfo.url}>
        {userAvatar && userAvatar.id ? (
          <ProfileImage
            src={`${apiConfig.paths.userPhotos.get.getAvatar}/${userAvatar.id}`}
          />
        ) : (
          <EmptyAvatar />
        )}
      </AvatarLink>
      {!!canEdit ? (
        <AvatarUpload onClick={(e) => onOpenUploadModal(e)}>
          <FontAwesomeIcon icon="pencil-alt" />
        </AvatarUpload>
      ) : null}
    </Wrap>
  );
};

export default ProfileAvatar;
