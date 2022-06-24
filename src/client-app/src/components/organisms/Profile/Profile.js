import React, { useState } from "react";
import { Routes, Route } from "react-router-dom";
import loadable from "@loadable/component";
import { Fragment } from "react";

const AsyncPage = loadable(
  (props) => import(`${"../../../pages/user/"}${props.page}`),
  {
    cacheKey: (props) => props.page,
  }
);

const ProfileInfo = loadable(() => import("./ProfileInfo"));

export default (props) => {
  const { userId, baseUrl, pages, userInfo, pageNumber } = props;
  const { canEdit } = userInfo;

  const [editorMode, setEditorMode] = useState("ARTICLE");
  const onToggleCreateMode = (name) => {
    setEditorMode(name);
  };

  return (
    <Fragment>
      <div className="row">
        <div className="col col-12 col-sm-12 col-md-12 col-lg-9 or-last order-lg-first">
          <Routes>
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
                          editorMode={editorMode}
                          onToggleCreateMode={onToggleCreateMode}
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
          </Routes>
        </div>
        <div className="col col-12 col-sm-12 col-md-12 col-lg-3 order-first order-lg-last mb-3">
          <ProfileInfo userInfo={userInfo} />
        </div>
      </div>
    </Fragment>
  );
};
