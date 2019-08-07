import React, { Component, Fragment } from "react";
import { Switch, withRouter } from "react-router-dom";
import loadable from "@loadable/component";
import Timeline from "./timeline";

const AsyncTabContent = loadable(props => import(`${props.page}`)),
  UserProfileInfo = loadable(() =>
    import("../../components/organisms/User/UserProfileInfo")
  );

class ProfileBody extends Component {
  constructor(props) {
    super(props);
    this._isMounted = false;
    this._uploadAvatar = null;
    this._refetch = null;
  }

  async componentDidMount() {
    this._isMounted = true;
  }

  componentWillUnmount() {
    this._isMounted = false;
  }

  render() {
    const { match, userInfo, baseUrl, pageNumber } = this.props;
    const { params } = match;
    const { userId } = params;
    const { canEdit } = userInfo;

    return (
      <Fragment>
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
                path={[`${baseUrl}/:userId/update`]}
                component={props => (
                  <AsyncTabContent
                    {...props}
                    userId={userId}
                    userUrl={`${baseUrl}/${userId}`}
                    page="./user-update"
                  />
                )}
              />
              <Timeline
                path={[`${baseUrl}/:userId/security`]}
                component={props => (
                  <AsyncTabContent
                    {...props}
                    userId={userId}
                    canEdit={canEdit}
                    userUrl={`${baseUrl}/${userId}`}
                    page="./user-security"
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
    );
  }
}

export default withRouter(ProfileBody);
