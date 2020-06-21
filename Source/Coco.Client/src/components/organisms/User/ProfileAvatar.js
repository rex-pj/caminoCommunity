import React from "react";
import styled from "styled-components";
import { ImageRound } from "../../atoms/Images";
import { ButtonPrimary } from "../../atoms/Buttons/Buttons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import NoAvatar from "../../atoms/NoImages/no-avatar";
import { useStore } from "../../../store/hook-store";

const ProfileImage = styled(ImageRound)`
    display: block;
    border-radius: ${(p) => p.theme.borderRadius.medium};
    min-width: 100%;
  `,
  EmptyAvatar = styled(NoAvatar)`
    border-radius: ${(p) => p.theme.borderRadius.medium};
    width: 100px;
    height: 100px;
  `,
  Wrap = styled.div`
    display: block;
  `,
  AvatarUpload = styled(ButtonPrimary)`
    position: absolute;
    top: -15px;
    right: -15px;
    z-index: 2;
    text-align: center;
    margin: auto;

    color: ${(p) => p.theme.color.lighter};
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
    width: 110px;
    height: 110px;
    border: 5px solid ${(p) => p.theme.rgbaColor.cyan};
    background-color: ${(p) => p.theme.rgbaColor.cyan};
    border-radius: 20px;
  `;

function ProfileAvatar({ ...props }) {
  const { userInfo, canEdit, className } = props;
  const { userAvatar } = userInfo;
  const dispatch = useStore(true)[1];

  function onOpenUploadModal(e) {
    dispatch("OPEN_MODAL", {
      data: {
        imageUrl:
          userAvatar && userAvatar.code
            ? `${process.env.REACT_APP_CDN_AVATAR_API_URL}${userAvatar.code}`
            : null,
        title: "Đổi Ảnh Đại Diện",
        canEdit: userInfo.canEdit,
      },
      options: {
        isOpen: true,
        type: "AVATAR_MODAL",
      },
    });
  }

  return (
    <Wrap className={className}>
      <AvatarLink href={userInfo.url}>
        {userAvatar && userAvatar.code ? (
          <ProfileImage
            src={`${process.env.REACT_APP_CDN_AVATAR_API_URL}${userAvatar.code}`}
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
}

export default ProfileAvatar;
