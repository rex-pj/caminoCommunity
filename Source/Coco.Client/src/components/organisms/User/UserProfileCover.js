import React, { Component } from "react";
import styled from "styled-components";
import loadable from "@loadable/component";
import { ButtonIconOutlineSecondary } from "../../molecules/ButtonIcons";
const ProfileAvatar = loadable(() => import("./ProfileAvatar")),
  UserCoverPhoto = loadable(() => import("./UserCoverPhoto"));

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

export default class extends Component {
  constructor(props) {
    super(props);

    this.state = {
      isEditCoverMode: false
    };
  }
  onToggleEditMode = e => {
    this.setState({
      isEditCoverMode: e
    });
  };

  render() {
    const { userInfo, canEdit } = this.props;
    const { isEditCoverMode } = this.state;

    return (
      <GroupThumbnail>
        <UserCoverPhoto
          userInfo={userInfo}
          canEdit={canEdit}
          onEditMode={this.onToggleEditMode}
        />
        <AvatarBlock
          userInfo={userInfo}
          canEdit={canEdit && !isEditCoverMode}
        />
        <h2>
          <a href={userInfo.url} className="profile-name">
            {userInfo.displayName}
          </a>
        </h2>
        {!isEditCoverMode ? (
          <ConnectButton icon="user-plus" size="sm">
            Kết nối
          </ConnectButton>
        ) : null}
      </GroupThumbnail>
    );
  }
}
