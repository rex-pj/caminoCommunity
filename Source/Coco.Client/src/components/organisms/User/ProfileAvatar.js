import React, { Component } from "react";
import { connect } from "react-redux";
import styled from "styled-components";
import ImageUpload from "../../molecules/UploadControl/ImageUpload";
import { ImageRound } from "../../atoms/Images";
import { openModal } from "../../../store/commands";
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
  onChangeAvatar = e => {
    this.props.openUploadModal(e.preview, "Upload avatar", "crop-image");
  };

  componentWillReceiveProps(nextProps) {
    if (this.props.modalPayload !== nextProps.modalPayload) {
      console.log(nextProps);
    }
  }

  render() {
    const { userInfo, canEdit, className } = this.props;

    return (
      <Wrap className={className}>
        <AvatarLink href={userInfo.url}>
          <ProfileImage src={userInfo.avatarUrl} />
        </AvatarLink>
        {!!canEdit ? <AvatarUpload onChange={this.onChangeAvatar} /> : null}
      </Wrap>
    );
  }
}

const mapDispatchToProps = dispatch => {
  return {
    openUploadModal: (children, title, modalType) => {
      openModal(dispatch, children, title, modalType);
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
