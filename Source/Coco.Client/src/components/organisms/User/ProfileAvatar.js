import React, { Component } from "react";
import { connect } from "react-redux";
import styled from "styled-components";
import { ImageRound } from "../../atoms/Images";
import { openModal, closeModal } from "../../../store/commands";
import { Button } from "../../atoms/Buttons";
import { UPDATE_USER_AVATAR } from "../../../utils/GraphQLQueries";
import { Mutation } from "react-apollo";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

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
  constructor(props) {
    super(props);

    const { userInfo } = props;
    this.state = {
      avatarUrl: userInfo.avatarUrl
    };

    this.updateAvatar = null;
  }

  onOpenModalUpload = (e, updateAvatar) => {
    const { avatarUrl } = this.state;
    this.updateAvatar = updateAvatar;
    this.props.openUploadModal(
      { imageUrl: `${process.env.REACT_APP_CDN_AVATAR_API_URL}${avatarUrl}` },
      "Upload avatar",
      "crop-image"
    );
  };

  async componentWillReceiveProps(nextProps) {
    if (this.props.modalPayload !== nextProps.modalPayload) {
      if (this.updateAvatar) {
        const { modalPayload } = nextProps;
        const { canEdit } = this.props;
        const {
          sourceImageUrl,
          xAxis,
          yAxis,
          width,
          height,
          contentType,
          fileName
        } = modalPayload;

        return await this.updateAvatar({
          variables: {
            criterias: {
              photoUrl: sourceImageUrl,
              canEdit,
              xAxis,
              yAxis,
              width,
              height,
              contentType,
              fileName
            }
          }
        })
          .then(response => {
            const { data } = response;
            const { updateAvatar } = data;
            const { result } = updateAvatar;
            this.setState({
              avatarUrl: result.photoUrl
            });

            this.props.closeUploadModal();
          })
          .catch(error => {});
      }
    }
  }

  render() {
    const { userInfo, canEdit, className } = this.props;

    const { avatarUrl } = this.state;
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
    openUploadModal: (data, title, modalType) => {
      openModal(dispatch, data, title, modalType);
    },
    closeUploadModal: () => {
      closeModal(dispatch);
    }
  };
};

const mapStateToProps = state => {
  return {
    modalPayload: state.modalReducer.payload
  };
};

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(ProfileAvatar);
