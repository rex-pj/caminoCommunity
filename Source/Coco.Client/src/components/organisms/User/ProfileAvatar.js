import React, { Component } from "react";
import { connect } from "react-redux";
import styled from "styled-components";
import ImageUpload from "../../molecules/UploadControl/ImageUpload";
import { ImageRound } from "../../atoms/Images";
import { openModal, closeModal } from "../../../store/commands";
import { UPDATE_USER_INFO_PER_ITEM } from "../../../utils/GraphQLQueries";
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
    this.updateUserInfoItem = null;
  }

  onChangeAvatar = (e, updateUserInfoItem) => {
    this.updateUserInfoItem = updateUserInfoItem;
    this.props.openUploadModal(e.preview, "Upload avatar", "crop-image");
  };

  async componentWillReceiveProps(nextProps) {
    if (this.props.modalPayload !== nextProps.modalPayload) {
      if (this.updateUserInfoItem) {
        const { canEdit, modalPayload } = nextProps;
        const {
          sourceImageUrl,
          xAxis,
          yAxis,
          width,
          height,
          contentType
        } = modalPayload;

        return await this.updateUserInfoItem({
          variables: {
            criterias: {
              PhotoUrl: sourceImageUrl,
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
            const { updateUserInfoItem } = data;
            const { result } = updateUserInfoItem;
            this.setState({
              avatarUrl: result.value
            });
            this.updateUserInfoItem = null;

            this.props.closeUploadModal();
          })
          .catch(error => {
            this.updateUserInfoItem = null;
          });
      }
    }
  }

  render() {
    const { userInfo, canEdit, className } = this.props;

    const { avatarUrl } = this.state;
    return (
      <Mutation mutation={UPDATE_USER_INFO_PER_ITEM}>
        {updateUserInfoItem => (
          <Wrap className={className}>
            <AvatarLink href={userInfo.url}>
              <ProfileImage src={avatarUrl} />
            </AvatarLink>
            {!!canEdit ? (
              <AvatarUpload
                onChange={e => this.onChangeAvatar(e, updateUserInfoItem)}
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
