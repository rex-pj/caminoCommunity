import React from "react";
import styled from "styled-components";
import { Thumbnail } from "../../molecules/Thumbnails";
import { ButtonIconOutlineSecondary } from "../../molecules/ButtonIcons";
import Overlay from "../../atoms/Overlay";
import { ImageCircle } from "../../atoms/Images";

const GroupThumbnail = styled.div`
  margin-top: 0;
  position: relative;
  height: 300px;
  max-height: 300px;
  background-color: ${p => p.theme.color.normal};

  a.cover-link {
    display: block;
    height: 100%;
    overflow: hidden;
  }

  a.profile-link {
    position: absolute;
    bottom: -${p => p.theme.size.normal};
    left: ${p => p.theme.size.small};
    width: 110px;
    height: 110px;
    border: 5px solid ${p => p.theme.rgbaColor.cyanMoreLight};
    background-color: ${p => p.theme.rgbaColor.cyanMoreLight};
    z-index: 1;
    border-radius: 100%;
    z-index: 1;
  }

  a.profile-name {
    position: absolute;
    bottom: ${p => p.theme.size.exTiny};
    left: 150px;
    z-index: 2;
    font-weight: 600;
    color: ${p => p.theme.color.white};
    font-size: ${p => p.theme.fontSize.large};
  }
`;

const ThumbnailOverlay = styled(Overlay)`
  height: 100px;
  top: auto;
  bottom: 0;
`;

const ConnectButton = styled(ButtonIconOutlineSecondary)`
  padding: ${p => p.theme.size.tiny};
  font-size: ${p => p.theme.fontSize.exSmall};
  line-height: 1;

  position: absolute;
  bottom: ${p => p.theme.size.distance};
  right: ${p => p.theme.size.distance};
  z-index: 1;
`;

const ProfileImage = styled(ImageCircle)`
  display: block;
`;

export default function(props) {
  const { userIdentity } = props;
  return (
    <GroupThumbnail>
      <a href={userIdentity.url} className="cover-link">
        <Thumbnail src={userIdentity.coverImageUrl} alt="" />
        <ThumbnailOverlay />
      </a>
      <a href={userIdentity.url} className="profile-name">
        {userIdentity.name}
      </a>
      <a href={userIdentity.url} className="profile-link">
        <ProfileImage src={userIdentity.avatarUrl} />
      </a>

      <ConnectButton icon="user-plus" size="sm">
        Kết nối
      </ConnectButton>
    </GroupThumbnail>
  );
}
