import React, { Component } from "react";
import { connect } from "react-redux";
import styled from "styled-components";
import { ImageRound } from "../../atoms/Images";
import { openModal, closeModal } from "../../../store/commands";
import { Button } from "../../atoms/Buttons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { UPDATE_USER_AVATAR } from "../../../utils/GraphQLQueries";
import { Mutation } from "react-apollo";

const ProfileImage = styled(ImageRound)`
  display: block;
  border-radius: ${p => p.theme.borderRadius.medium};
`;

const Wrap = styled.div`
  display: block;
`;

const AvatarUpload = styled(Button)`
  position: absolute;
  top: -15px;
  right: -15px;
  z-index: 2;
  text-align: center;
  margin: auto;

  color: ${p => p.theme.color.exLight};
  width: ${p => p.theme.size.medium};
  height: ${p => p.theme.size.medium};
  border-radius: 100%;
  padding: 0 ${p => p.theme.size.exTiny};
  background-color: ${p => p.theme.rgbaColor.moreDark};
  border: 1px solid ${p => p.theme.rgbaColor.dark};
  cursor: pointer;
  font-weight: 600;

  :hover {
    background-color: ${p => p.theme.rgbaColor.exDark};
  }
`;

const AvatarLink = styled.a`
  display: block;
  width: 110px;
  height: 110px;
  border: 5px solid ${p => p.theme.rgbaColor.cyanMoreLight};
  background-color: ${p => p.theme.rgbaColor.cyanMoreLight};
  border-radius: 20px;
`;

class ProfileAvatar extends Component {
  onOpenModalUpload = (e, updateAvatar) => {
    const { userInfo } = this.props;
    const { avatarUrl } = userInfo;
    this.props.openUploadModal({
      imageUrl: `${process.env.REACT_APP_CDN_AVATAR_API_URL}${avatarUrl}`,
      eventExecute: updateAvatar,
      title: "Upload avatar",
      modalType: "crop-image"
    });
  };

  render() {
    const { userInfo, canEdit, className } = this.props;
    const { avatarUrl } = userInfo;
    var random = Math.random();
    return (
      <Mutation mutation={UPDATE_USER_AVATAR}>
        {updateAvatar => (
          <Wrap className={className}>
            <AvatarLink href={userInfo.url}>
              {avatarUrl ? (
                <ProfileImage
                  src={`${
                    process.env.REACT_APP_CDN_AVATAR_API_URL
                  }${avatarUrl}?${random}`}
                />
              ) : null}
            </AvatarLink>
            {!!canEdit ? (
              <AvatarUpload
                onClick={e => this.onOpenModalUpload(e, updateAvatar)}
              >
                <FontAwesomeIcon icon="pencil-alt" />
              </AvatarUpload>
            ) : null}
          </Wrap>
        )}
      </Mutation>
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
