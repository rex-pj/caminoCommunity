import React from "react";
import styled from "styled-components";
import ProfileAvatar from "./ProfileAvatar";
import { ButtonIconOutlineSecondary } from "../../molecules/ButtonIcons";
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

  a.profile-avatar {
    position: absolute;
    bottom: ${p => p.theme.size.distance};
    left: ${p => p.theme.size.distance};
    width: 110px;
    height: 110px;
    border: 5px solid ${p => p.theme.rgbaColor.cyanMoreLight};
    background-color: ${p => p.theme.rgbaColor.cyanMoreLight};
    z-index: 1;
    border-radius: 100%;
  }

  a.profile-name {
    position: absolute;
    bottom: ${p => p.theme.size.distance};
    left: 135px;
    z-index: 2;
    font-weight: 600;
    color: ${p => p.theme.color.white};
    font-size: ${p => p.theme.fontSize.large};
  }
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
  const { userInfo, canEdit } = props;
  return (
    <GroupThumbnail>
      <ProfileAvatar userInfo={userInfo} canEdit={canEdit} />
      <a href={userInfo.url} className="profile-name">
        {userInfo.displayName}
      </a>
      <a href={userInfo.url} className="profile-avatar">
        <ProfileImage src={userInfo.avatarUrl} />
      </a>

      <ConnectButton icon="user-plus" size="sm">
        Kết nối
      </ConnectButton>
    </GroupThumbnail>
  );
}
