import React, { Component, Fragment } from "react";
import { connect } from "react-redux";
import { withRouter } from "react-router-dom";
import { Query } from "react-apollo";
import loadable from "@loadable/component";
import ErrorBlock from "../../components/atoms/ErrorBlock";
import { defaultClient } from "../../utils/GraphQLClient";
import { GET_USER_INFO } from "../../utils/GraphQLQueries";
import ProfileBody from "./profile-body";
import Loading from "../../components/atoms/Loading";
import styled from "styled-components";
import * as avatarActions from "../../store/actions/avatarActions";
import { ButtonIconOutlineSecondary } from "../../components/molecules/ButtonIcons";
const ProfileAvatar = loadable(() =>
    import("../../components/organisms/User/ProfileAvatar")
  ),
  UserCoverPhoto = loadable(() =>
    import("../../components/organisms/User/UserCoverPhoto")
  ),
  ProfileNavigation = loadable(() =>
    import("../../components/organisms/User/ProfileNavigation")
  );

const CoverPageBlock = styled.div`
  position: relative;
  height: 300px;
  overflow: hidden;

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

const CoverNav = styled.div`
  box-shadow: ${p => p.theme.shadow.BoxShadow};
  background-color: ${p => p.theme.color.white};
  border-bottom-left-radius: ${p => p.theme.borderRadius.normal};
  border-bottom-right-radius: ${p => p.theme.borderRadius.normal};
  margin-bottom: ${p => p.theme.size.distance};
`;

const AvatarBlock = styled(ProfileAvatar)`
  position: absolute;
  bottom: ${p => p.theme.size.distance};
  left: ${p => p.theme.size.distance};
  z-index: 3;
`;

const ConnectButton = styled(ButtonIconOutlineSecondary)`
  padding: ${p => p.theme.size.tiny};
  font-size: ${p => p.theme.rgbaColor.small};
  line-height: 1;

  position: absolute;
  bottom: ${p => p.theme.size.distance};
  right: ${p => p.theme.size.distance};
  z-index: 3;
`;

class Profile extends Component {
  constructor(props) {
    super(props);
    this._isMounted = false;
    this._canEdit = false;
    this._refetch = null;
    this._baseUrl = "/profile";

    this.state = {
      isEditCoverMode: false
    };
  }

  async componentDidMount() {
    this._isMounted = true;
  }

  componentWillReceiveProps(nextProps) {
    const { avatarPayload } = nextProps;
    if (!avatarPayload) {
      return;
    }

    if (avatarPayload.actionType === avatarActions.AVATAR_RELOAD) {
      if (this._isMounted) {
        this._refetch();
      }
    }
  }

  onToggleEditCoverMode = e => {
    if (this._isMounted) {
      this.setState({
        isEditCoverMode: e
      });
    }
  };

  userCoverUploaded = () => {
    if (this._isMounted) {
      this._refetch();
    }
  };

  parseUserInfo(response) {
    const { fullUserInfo } = response;
    const { result, accessMode } = fullUserInfo;
    const canEdit = accessMode === "CAN_EDIT";

    return {
      ...result,
      canEdit: canEdit,
      avatarUrl: result.avatarUrl,
      url: result.userIdentityId ? `/profile/${result.userIdentityId}` : "",
      coverPhotoUrl: result.coverPhotoUrl
    };
  }

  componentWillUnmount() {
    this._isMounted = false;
  }

  render() {
    const { match } = this.props;
    const { params } = match;
    const { userId, pageNumber } = params;
    const { isEditCoverMode } = this.state;
    return (
      <Query
        query={GET_USER_INFO}
        variables={{
          criterias: {
            userId
          }
        }}
        client={defaultClient}
      >
        {({ loading, error, data, refetch }) => {
          this._refetch = refetch;
          if (loading) {
            return <Loading>Loading</Loading>;
          }
          if (error) {
            return <ErrorBlock>Error</ErrorBlock>;
          }

          const userInfo = this.parseUserInfo(data);

          return (
            <Fragment>
              <CoverPageBlock>
                {!isEditCoverMode ? (
                  <ConnectButton icon="user-plus" size="sm">
                    Kết nối
                  </ConnectButton>
                ) : null}
                <UserCoverPhoto
                  userInfo={userInfo}
                  canEdit={userInfo.canEdit}
                  onUploaded={this.userCoverUploaded}
                  onToggleEditMode={this.onToggleEditCoverMode}
                />
                <AvatarBlock
                  userInfo={userInfo}
                  canEdit={userInfo.canEdit && !isEditCoverMode}
                />
                <h2>
                  <a href={userInfo.url} className="profile-name">
                    {userInfo.displayName}
                  </a>
                </h2>
              </CoverPageBlock>
              <CoverNav>
                <ProfileNavigation userId={userId} />
              </CoverNav>
              <ProfileBody
                userInfo={userInfo}
                pageNumber={pageNumber}
                baseUrl={this._baseUrl}
              />
            </Fragment>
          );
        }}
      </Query>
    );
  }
}

const mapStateToProps = state => {
  return {
    avatarPayload: state.avatarReducer.payload
  };
};

export default connect(mapStateToProps)(withRouter(Profile));
