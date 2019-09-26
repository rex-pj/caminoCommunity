import React, { Component } from "react";
import { connect } from "react-redux";
import styled from "styled-components";
import { ImageRound } from "../../atoms/Images";
import { openModal, closeModal } from "../../../store/commands";
import { ButtonPrimary } from "../../atoms/Buttons/Buttons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import NoAvatar from "../../atoms/NoImages/no-avatar";

const ProfileImage = styled(ImageRound)`
    display: block;
    border-radius: ${p => p.theme.borderRadius.medium};
    min-width: 100%;
  `,
  EmptyAvatar = styled(NoAvatar)`
    border-radius: ${p => p.theme.borderRadius.medium};
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

    color: ${p => p.theme.color.lighter};
    width: ${p => p.theme.size.medium};
    height: ${p => p.theme.size.medium};
    border-radius: 100%;
    padding: 0 ${p => p.theme.size.exTiny};
    background-color: ${p => p.theme.rgbaColor.dark};
    border: 1px solid ${p => p.theme.rgbaColor.darkLight};
    cursor: pointer;
    font-weight: 600;

    :hover {
      background-color: ${p => p.theme.rgbaColor.darker};
    }
  `,
  AvatarLink = styled.a`
    display: block;
    width: 110px;
    height: 110px;
    border: 5px solid ${p => p.theme.rgbaColor.cyan};
    background-color: ${p => p.theme.rgbaColor.cyan};
    border-radius: 20px;
  `;

class ProfileAvatar extends Component {
  onOpenUploadModal = e => {
    const { userInfo } = this.props;
    const { avatarUrl } = userInfo;
    this.props.openUploadModal({
      imageUrl: avatarUrl
        ? `${process.env.REACT_APP_CDN_AVATAR_API_URL}${avatarUrl}`
        : null,
      title: "Đổi Ảnh Đại Diện",
      modalType: "change-avatar",
      canEdit: userInfo.canEdit
    });
  };

  render() {
    const { userInfo, canEdit, className } = this.props;
    const { avatarUrl } = userInfo;

    return (
      <Wrap className={className}>
        <AvatarLink href={userInfo.url}>
          {avatarUrl ? (
            <ProfileImage
              src={`${process.env.REACT_APP_CDN_AVATAR_API_URL}${avatarUrl}`}
            />
          ) : (
            <EmptyAvatar />
          )}
        </AvatarLink>
        {!!canEdit ? (
          <AvatarUpload onClick={e => this.onOpenUploadModal(e)}>
            <FontAwesomeIcon icon="pencil-alt" />
          </AvatarUpload>
        ) : null}
      </Wrap>
    );
  }
}

const mapDispatchToProps = dispatch => {
  return {
    openUploadModal: e => {
      openModal(dispatch, e);
    },
    closeUploadModal: () => {
      closeModal(dispatch);
    }
  };
};

export default connect(
  null,
  mapDispatchToProps
)(ProfileAvatar);
