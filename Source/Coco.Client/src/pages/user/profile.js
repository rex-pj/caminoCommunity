import React, { Component, Fragment } from "react";
import { Switch, withRouter } from "react-router-dom";
import queryString from "query-string";
import Timeline from "./timeline";
import styled from "styled-components";
import loadable from "@loadable/component";
import ProfileNavigation from "../../components/organisms/User/ProfileNavigation";
import AuthService from "../../services/AuthService";

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

export default withRouter(
  class extends Component {
    constructor(props) {
      super(props);
      this._isMounted = false;

      this.state = {
        userIdentity: {},
        userInfo: {}
      };
    }

    async componentDidMount() {
      this._isMounted = true;
      var searchParams = queryString.parse(this.props.location.search);

      await AuthService.getFullLoggedUserInfo(searchParams.userHashedId).then(
        user => {
          const userIdentity = {
            avatarUrl: `${process.env.PUBLIC_URL}/photos/farmer-avatar.jpg`,
            url: user.userHashedId ? `/profile?id=${user.userHashedId}` : "",
            name: user.displayName,
            coverImageUrl: `${process.env.PUBLIC_URL}/photos/profile-cover.jpg`
          };

          const userInfo = {
            blast: user.description,
            address: user.address,
            country: user.countryName,
            joinedDate: user.createdDate,
            birthDate: user.birthDate,
            email: user.email,
            mobile: user.phoneNumber
          };

          if (this._isMounted) {
            this.setState({
              userIdentity,
              userInfo
            });
          }
        }
      );
    }

    componentWillUnmount() {
      this._isMounted = false;
    }

    render() {
      const { userIdentity, userInfo } = this.state;
      const { match, location } = this.props;

      const currentUrl = match.url;
      const search = location.search;

      return (
        <Fragment>
          <CoverNav>
            <UserProfileCover userIdentity={userIdentity} />
            <ProfileNavigation />
          </CoverNav>
          <div className="row">
            <div className="col col-8 col-sm-8 col-md-8 col-lg-9">
              <Switch>
                <Timeline
                  path={[
                    `${currentUrl}/posts`,
                    `${currentUrl}/posts/page/:page`,
                    `${currentUrl}/posts${search}`,
                    `${currentUrl}/posts/page/:page${search}`
                  ]}
                  exact={true}
                  component={props => (
                    <AsyncTabContent
                      {...props}
                      userUrl={match.url}
                      page="./user-posts"
                    />
                  )}
                />
                <Timeline
                  path={[
                    `${currentUrl}/products`,
                    `${currentUrl}/products/page/:page`,
                    `${currentUrl}/products${search}`,
                    `${currentUrl}/products/page/:page${search}`
                  ]}
                  exact={true}
                  component={props => (
                    <AsyncTabContent
                      userUrl={match.url}
                      {...props}
                      page="./user-products"
                    />
                  )}
                />
                <Timeline
                  path={[
                    `${currentUrl}/farms`,
                    `${currentUrl}/farms/page/:page`,
                    `${currentUrl}/farms${search}`,
                    `${currentUrl}/farms/page/:page${search}`
                  ]}
                  exact={true}
                  component={props => (
                    <AsyncTabContent
                      userUrl={match.url}
                      {...props}
                      page="./user-farms"
                    />
                  )}
                />
                <Timeline
                  path={[
                    `${currentUrl}/followings`,
                    `${currentUrl}/followings/page/:page`,
                    `${currentUrl}/followings${search}`,
                    `${currentUrl}/followings/page/:page${search}`
                  ]}
                  exact={true}
                  component={props => (
                    <AsyncTabContent
                      userUrl={match.url}
                      {...props}
                      page="./user-followings"
                    />
                  )}
                />
                <Timeline
                  path={[`${currentUrl}/about`]}
                  exact={true}
                  component={props => (
                    <AsyncTabContent
                      userUrl={match.url}
                      {...props}
                      page="./user-about"
                    />
                  )}
                />
                <Timeline
                  location={{
                    pathname: currentUrl,
                    search: search
                  }}
                  exact={true}
                  component={props => (
                    <AsyncTabContent
                      userUrl={match.url}
                      {...props}
                      page="./user-feeds"
                    />
                  )}
                />
                <Timeline
                  location={{
                    pathname: currentUrl
                  }}
                  exact={true}
                  component={props => (
                    <AsyncTabContent
                      userUrl={match.url}
                      {...props}
                      page="../error/not-found"
                    />
                  )}
                />
                <Timeline
                  path={[
                    `${currentUrl}/page/:page${search}`,
                    `${currentUrl}/feeds`,
                    `${currentUrl}/feeds/page/:page`,
                    `${currentUrl}/feeds${search}`,
                    `${currentUrl}/feeds/page/:page${search}`
                  ]}
                  exact={true}
                  component={props => (
                    <AsyncTabContent
                      userUrl={match.url}
                      {...props}
                      page="./user-feeds"
                    />
                  )}
                />
              </Switch>
            </div>
            <div className="col col-4 col-sm-4 col-md-4 col-lg-3">
              <UserProfileInfo userInfo={userInfo} />
            </div>
          </div>
        </Fragment>
      );
    }
  }
);
