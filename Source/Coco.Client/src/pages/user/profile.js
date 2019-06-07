import React, { Component, Fragment } from "react";
import { Switch, withRouter } from "react-router-dom";
import queryString from "query-string";
import Timeline from "./timeline";
import styled from "styled-components";
import loadable from "@loadable/component";
import ProfileNavigation from "../../components/organisms/User/ProfileNavigation";
import { getFullLoggedUserInfo } from "../../services/AuthService";

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

      await getFullLoggedUserInfo(searchParams.userHashedId).then(user => {
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
      });
    }

    componentWillUnmount() {
      this._isMounted = false;
    }

    render() {
      const { userIdentity, userInfo } = this.state;
      const { match } = this.props;
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
                    `${match.url}/posts/`,
                    `${match.url}/posts?id=:id`,
                    `${match.url}/posts/page/:page`
                  ]}
                  exact={true}
                  component={props => (
                    <AsyncTabContent
                      {...props}
                      userUrl={this.props.match.url}
                      page="./user-posts"
                    />
                  )}
                />
                <Timeline
                  path={[
                    `${match.url}/products/`,
                    `${match.url}/products?id=:id`,
                    `${match.url}/products/page/:page`
                  ]}
                  exact={true}
                  component={props => (
                    <AsyncTabContent
                      userUrl={this.props.match.url}
                      {...props}
                      page="./user-products"
                    />
                  )}
                />
                <Timeline
                  path={[
                    `${match.url}/farms/`,
                    `${match.url}/farms?id=:id`,
                    `${match.url}/farms/page/:page`
                  ]}
                  exact={true}
                  component={props => (
                    <AsyncTabContent
                      userUrl={this.props.match.url}
                      {...props}
                      page="./user-farms"
                    />
                  )}
                />
                <Timeline
                  path={[
                    `${match.url}/followings/`,
                    `${match.url}/followings?id=:id`,
                    `${match.url}/followings/page/:page`
                  ]}
                  exact={true}
                  component={props => (
                    <AsyncTabContent
                      userUrl={this.props.match.url}
                      {...props}
                      page="./user-followings"
                    />
                  )}
                />
                <Timeline
                  path={[`${match.url}/about`]}
                  exact={true}
                  component={props => (
                    <AsyncTabContent
                      userUrl={this.props.match.url}
                      {...props}
                      page="./user-about"
                    />
                  )}
                />
                <Timeline
                  path={[
                    `${match.url}`,
                    `${match.url}/page/:page`,
                    `${match.url}/feeds`,
                    `${match.url}/feeds?id=:id`,
                    `${match.url}/feeds/page/:page`
                  ]}
                  exact={true}
                  component={props => (
                    <AsyncTabContent
                      userUrl={this.props.match.url}
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
