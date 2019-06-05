import React, { Component, Fragment } from "react";
import { Switch, withRouter } from "react-router-dom";
import Timeline from "./timeline";
import styled from "styled-components";
import loadable from "@loadable/component";
import ProfileNavigation from "../../components/organisms/User/ProfileNavigation";
import queryString from "query-string";

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

      var search = queryString.parse(props.location.search);
      const userIdentity = {
        avatarUrl: `${process.env.PUBLIC_URL}/photos/farmer-avatar.jpg`,
        url: "/trungle.it",
        name: "Anh Sáu",
        coverImageUrl: `${process.env.PUBLIC_URL}/photos/profile-cover.jpg`
      };

      this.state = {
        userIdentity,
        userInfo: null
      };
    }

    loadUserInfo = () => {
      this.setState({
        userInfo: {
          blast:
            "Boru Land is a community platform for the future. This community is a great place to ask questions, request features, report bugs, and chat with the Spectrum team.",
          address: "189 Hòa Nam, Ninh Hương, Bình Tuy, Việt Nam",
          country: "Việt Nam",
          joinedDate: "07/12/2017",
          birthDate: "22/03/1990",
          email: "trungle.it@gmail.com",
          mobile: "0905095576"
        }
      });
    };

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
            <div className="col col-4 col-sm-4 col-md-4 col-lg-3">
              <UserProfileInfo
                userInfo={userInfo}
                loadUserInfo={this.loadUserInfo}
              />
            </div>
            <div className="col col-8 col-sm-8 col-md-8 col-lg-9">
              <Switch>
                <Timeline
                  path={[
                    `${match.url}/posts/`,
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
          </div>
        </Fragment>
      );
    }
  }
);
