import React, { Component, Fragment } from "react";
import { connect } from "react-redux";
import { Switch, withRouter } from "react-router-dom";
import { Query } from "react-apollo";
import styled from "styled-components";
import loadable from "@loadable/component";
import ProfileNavigation from "../../components/organisms/User/ProfileNavigation";
import Timeline from "./timeline";
import { GET_USER_INFO } from "../../utils/GraphQLQueries";
import Loading from "../../components/atoms/Loading";
import ErrorBlock from "../../components/atoms/ErrorBlock";
import { defaultClient } from "../../utils/GraphQLClient";
import { UPDATE_USER_AVATAR } from "../../utils/GraphQLQueries";
import { Mutation } from "react-apollo";

const AsyncTabContent = loadable(props => import(`${props.page}`));

const UserProfileCover = loadable(() =>
  import("../../components/organisms/User/UserProfileCover")
);

const UserProfileInfo = loadable(() =>
  import("../../components/organisms/User/UserProfileInfo")
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
  }

  async componentDidMount() {
    this._isMounted = true;
  }

  async componentWillReceiveProps(nextProps) {
    if (nextProps.modalPayload) {
      const { eventExecute } = nextProps.modalPayload;
      if (eventExecute) {
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

        return await eventExecute({
          variables: {
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

    const baseUrl = "/profile";

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
        {({ loading, error, data }) => {
          if (loading) {
            return <Loading>Loading</Loading>;
          }
          if (error) {
            return <ErrorBlock>Error</ErrorBlock>;
          }

          const userInfo = this.parseUserInfo(data);
          return (
            <Mutation mutation={UPDATE_USER_AVATAR}>
              {updateAvatar => (
                <Fragment>
                  <CoverNav>
                    <UserProfileCover
                      userInfo={userInfo}
                      canEdit={userInfo.canEdit}
                    />
                    <ProfileNavigation userId={userId} />
                  </CoverNav>
                  <div className="row">
                    <div className="col col-8 col-sm-8 col-md-8 col-lg-9">
                      <Switch>
                        <Timeline
                          path={[`${baseUrl}/:userId/about`]}
                          component={props => (
                            <AsyncTabContent
                              {...props}
                              userId={userId}
                              userUrl={`${baseUrl}/${userId}`}
                              page="./user-about"
                            />
                          )}
                        />
                        <Timeline
                          path={[
                            `${baseUrl}/:userId/posts`,
                            `${baseUrl}/:userId/posts/page/:pageNumber`
                          ]}
                          component={props => (
                            <AsyncTabContent
                              {...props}
                              pageNumber={pageNumber}
                              userUrl={`${baseUrl}/${userId}`}
                              page="./user-posts"
                            />
                          )}
                        />
                        <Timeline
                          path={[
                            `${baseUrl}/:userId/products`,
                            `${baseUrl}/:userId/products/page/:pageNumber`
                          ]}
                          component={props => (
                            <AsyncTabContent
                              {...props}
                              pageNumber={pageNumber}
                              userUrl={`${baseUrl}/${userId}`}
                              page="./user-products"
                            />
                          )}
                        />
                        <Timeline
                          path={[
                            `${baseUrl}/:userId/farms`,
                            `${baseUrl}/:userId/farms/page/:pageNumber`
                          ]}
                          component={props => (
                            <AsyncTabContent
                              {...props}
                              pageNumber={pageNumber}
                              userUrl={`${baseUrl}/${userId}`}
                              page="./user-farms"
                            />
                          )}
                        />
                        <Timeline
                          path={[
                            `${baseUrl}/:userId/followings`,
                            `${baseUrl}/:userId/followings/page/:pageNumber`
                          ]}
                          component={props => (
                            <AsyncTabContent
                              {...props}
                              pageNumber={pageNumber}
                              userUrl={`${baseUrl}/${userId}`}
                              page="./user-followings"
                            />
                          )}
                        />
                        <Timeline
                          path={[
                            `${baseUrl}/:userId`,
                            `${baseUrl}/:userId/feeds`,
                            `${baseUrl}/:userId/feeds/page/:pageNumber`
                          ]}
                          component={props => (
                            <AsyncTabContent
                              {...props}
                              pageNumber={pageNumber}
                              userUrl={`${baseUrl}/${userId}`}
                              page="./user-feeds"
                            />
                          )}
                        />
                        <Timeline
                          path={[`${baseUrl}`]}
                          exact={true}
                          component={() => <div>NOT FOUND</div>}
                        />
                      </Switch>
                    </div>
                    <div className="col col-4 col-sm-4 col-md-4 col-lg-3">
                      <UserProfileInfo userInfo={userInfo} />
                    </div>
                  </div>
                </Fragment>
              )}
            </Mutation>
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
