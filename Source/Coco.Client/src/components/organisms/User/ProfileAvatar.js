import React, { Component } from "react";
import { connect } from "react-redux";
import styled from "styled-components";
import ImageUpload from "../../molecules/UploadControl/ImageUpload";
import { ImageRound } from "../../atoms/Images";
import { openModal, closeModal } from "../../../store/commands";
import { UPDATE_USER_AVATAR } from "../../../utils/GraphQLQueries";
import { Mutation } from "react-apollo";

const ProfileImage = styled(ImageRound)`
  display: block;
  border-radius: ${p => p.theme.borderRadius.medium};
`;

const Wrap = styled.div`
  display: block;
`;

const AvatarUpload = styled(ImageUpload)`
  position: absolute;
  top: -15px;
  right: -15px;
  z-index: 2;
  text-align: center;
  margin: auto;

  span {
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

    svg {
      display: block;
      margin: 10px auto 0 auto;
    }
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
      avatarUrl: userInfo.photo
    };
    this.updateAvatar = null;
  }

  onChangeAvatar = (e, updateAvatar) => {
    this.updateAvatar = updateAvatar;
    this.props.openUploadModal(e.preview, "Upload avatar", "crop-image");
  };

  async componentWillReceiveProps(nextProps) {
    if (this.props.modalPayload !== nextProps.modalPayload) {
      if (this.updateAvatar) {
        const { canEdit, modalPayload } = nextProps;
        const {
          sourceImageUrl,
          xAxis,
          yAxis,
          width,
          height,
          contentType
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
              contentType
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
            this.updateAvatar = null;

            this.props.closeUploadModal();
          })
          .catch(error => {
            this.updateAvatar = null;
          });
      }
    }
  }

  render() {
    const { userInfo, canEdit, className } = this.props;

    const { avatarUrl } = this.state;
    return (
      <Mutation mutation={UPDATE_USER_AVATAR}>
        {updateAvatar => (
          <Wrap className={className}>
            <AvatarLink href={userInfo.url}>
              <ProfileImage src={`data:image/png;base64,${avatarUrl}`} />
            </AvatarLink>
            {!!canEdit ? (
              <AvatarUpload
                onChange={e => this.onChangeAvatar(e, updateAvatar)}
              />
            ) : null}
          </Wrap>
        )}
      </Mutation>
    );
  }
}

const mapDispatchToProps = dispatch => {
  return {
    openUploadModal: (children, title, modalType) => {
      openModal(dispatch, children, title, modalType);
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
