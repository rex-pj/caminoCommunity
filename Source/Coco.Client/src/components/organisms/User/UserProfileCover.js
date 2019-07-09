import React from "react";
import styled from "styled-components";
import ProfileAvatar from "./ProfileAvatar";
import { ButtonIconOutlineSecondary } from "../../molecules/ButtonIcons";
import { Thumbnail } from "../../molecules/Thumbnails";
import Overlay from "../../atoms/Overlay";

const ThumbnailOverlay = styled(Overlay)`
  height: 100px;
  top: auto;
  bottom: 0;
`;

const GroupThumbnail = styled.div`
  margin-top: 0;
  position: relative;
  height: 300px;
  max-height: 300px;
  background-color: ${p => p.theme.color.normal};

  .profile-name {
    font-weight: 600;
    color: ${p => p.theme.color.white};
    font-size: ${p => p.theme.fontSize.large};
  }

  h2 {
    left: 135px;
    bottom: ${p => p.theme.size.small};
    z-index: 3;
    margin-bottom: 0;
    position: absolute;
  }

  a.cover-link {
    display: block;
    height: 100%;
    overflow: hidden;
  }
`;

const AvatarBlock = styled(ProfileAvatar)`
  position: absolute;
  bottom: ${p => p.theme.size.distance};
  left: ${p => p.theme.size.distance};
  z-index: 3;
`;

const ConnectButton = styled(ButtonIconOutlineSecondary)`
  padding: ${p => p.theme.size.tiny};
  font-size: ${p => p.theme.fontSize.exSmall};
  line-height: 1;

  position: absolute;
  bottom: ${p => p.theme.size.distance};
  right: ${p => p.theme.size.distance};
  z-index: 3;
`;

export default function(props) {
  const { userInfo, canEdit } = props;
  return (
    <GroupThumbnail>
      <a href={userInfo.url} className="cover-link">
        <Thumbnail src={userInfo.coverImageUrl} alt="" />
        <ThumbnailOverlay />
      </a>
      <AvatarBlock userInfo={userInfo} canEdit={canEdit} />
      <h2>
        <a href={userInfo.url} className="profile-name">
          {userInfo.displayName}
        </a>
      </h2>
      <ConnectButton icon="user-plus" size="sm">
        Kết nối
      </ConnectButton>
    </GroupThumbnail>
  );
}
