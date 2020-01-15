import React, { Fragment } from "react";
import { Switch, withRouter } from "react-router-dom";
import loadable from "@loadable/component";
import Timeline from "./Timeline";

const AsyncTabContent = loadable(props =>
    import(`${"../../../pages/user/"}${props.page}`)
  ),
  UserProfileInfo = loadable(() => import("./UserProfileInfo"));

const ProfileBody = props => {
  const { match, userInfo, baseUrl, pages } = props;
  const { params } = match;
  const { userId } = params;
  const { canEdit } = userInfo;

  return (
    <Fragment>
      <div className="row">
        <div className="col col-8 col-sm-8 col-md-8 col-lg-9">
          <Switch>
            {pages
              ? pages.map(item => {
                  return (
                    <Timeline
                      key={item.dir}
                      path={item.path}
                      component={props => (
                        <AsyncTabContent
                          {...props}
                          userId={userId}
                          canEdit={canEdit}
                          userUrl={`${baseUrl}/${userId}`}
                          page={item.dir}
                        />
                      )}
                    />
                  );
                })
              : null}

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
};

export default withRouter(ProfileBody);
