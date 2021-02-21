import React from "react";
import { Switch, withRouter, Route } from "react-router-dom";
import loadable from "@loadable/component";

const AsyncPage = loadable(
  (props) => import(`${"../../../pages/user/"}${props.page}`),
  {
    cacheKey: (props) => props.page,
  }
);

const ProfileInfo = loadable(() => import("./ProfileInfo"));

export default withRouter((props) => {
  const { userId, baseUrl, pages, userInfo, pageNumber } = props;
  const { canEdit } = userInfo;

  return (
    <div className="row">
      <div className="col col-8 col-sm-8 col-md-8 col-lg-9">
        <Switch>
          {pages
            ? pages.map((route) => {
                var paths = route.path.map((path) => {
                  return `${baseUrl}${path}`;
                });
                return (
                  <Route
                    key={route.page}
                    path={paths}
                    component={(props) => (
                      <AsyncPage
                        {...props}
                        userId={userId}
                        canEdit={canEdit}
                        userUrl={`${baseUrl}/${userId}`}
                        page={route.page}
                        pageNumber={pageNumber}
                      />
                    )}
                  />
                );
              })
            : null}

          <Route
            path={[`${baseUrl}`]}
            exact={true}
            component={() => <div>NOT FOUND</div>}
          />
        </Switch>
      </div>
      <div className="col col-4 col-sm-4 col-md-4 col-lg-3">
        <ProfileInfo userInfo={userInfo} />
      </div>
    </div>
  );
});
