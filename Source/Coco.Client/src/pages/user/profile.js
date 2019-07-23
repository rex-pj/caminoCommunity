import React, { Component, Fragment } from "react";
import { connect } from "react-redux";
import { withRouter } from "react-router-dom";
import { Query, Mutation } from "react-apollo";
import loadable from "@loadable/component";
import ErrorBlock from "../../components/atoms/ErrorBlock";
import { defaultClient } from "../../utils/GraphQLClient";
import { UPDATE_USER_AVATAR } from "../../utils/GraphQLQueries";
import ProfileBody from "./profile-body";
import { GET_USER_INFO } from "../../utils/GraphQLQueries";
import Loading from "../../components/atoms/Loading";
import styled from "styled-components";
import ProfileNavigation from "../../components/organisms/User/ProfileNavigation";

const UserProfileCover = loadable(() =>
  import("../../components/organisms/User/UserProfileCover")
);

const CoverNav = styled.div`
  box-shadow: ${p => p.theme.shadow.BoxShadow};
  background-color: ${p => p.theme.color.white};
  border-bottom-left-radius: ${p => p.theme.borderRadius.normal};
  border-bottom-right-radius: ${p => p.theme.borderRadius.normal};
  margin-bottom: ${p => p.theme.size.distance};
`;

class Profile extends Component {
  constructor(props) {
    super(props);
    this._isMounted = false;
    this._canEdit = false;
    this._uploadAvatar = null;
    this._refetch = null;
    this._baseUrl = "/profile";
  }

  async componentDidMount() {
    this._isMounted = true;
  }

  async componentWillReceiveProps(nextProps) {
    const { modalPayload } = nextProps;
    if (
      modalPayload &&
      modalPayload.actionType === "PUSH_DATA" &&
      this._uploadAvatar
    ) {
      const { modalPayload } = nextProps;
      const {
        sourceImageUrl,
        xAxis,
        yAxis,
        width,
        height,
        contentType,
        fileName
      } = modalPayload;

      const variables = {
        criterias: {
          photoUrl: sourceImageUrl,
          canEdit: this._canEdit,
          xAxis,
          yAxis,
          width,
          height,
          contentType,
          fileName
        }
      };

      return await this._uploadAvatar({
        variables: variables
      })
        .then(response => {
          this._refetch();

          this.props.closeUploadModal();
        })
        .catch(error => {});
    }
  }

  parseUserInfo(response) {
    const { fullUserInfo } = response;
    const { result, accessMode } = fullUserInfo;
    this._canEdit = accessMode === "CAN_EDIT";

    return {
      ...result,
      canEdit: this._canEdit,
      avatarUrl: result.avatarUrl,
      url: result.userIdentityId ? `/profile/${result.userIdentityId}` : "",
      coverImageUrl: `${process.env.PUBLIC_URL}/photos/profile-cover.jpg`
    };
  }

  componentWillUnmount() {
    this._isMounted = false;
  }

  render() {
    const { match } = this.props;
    const { params } = match;
    const { userId, pageNumber } = params;

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
              <Mutation mutation={UPDATE_USER_AVATAR}>
                {updateAvatar => {
                  this._uploadAvatar = updateAvatar;
                  return (
                    <UserProfileCover
                      userInfo={userInfo}
                      canEdit={userInfo.canEdit}
                    />
                  );
                }}
              </Mutation>
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
    modalPayload: state.modalReducer.payload
  };
};

export default connect(mapStateToProps)(withRouter(Profile));
