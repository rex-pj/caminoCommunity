import React, { useEffect, useState } from "react";
import { Switch, withRouter, Route } from "react-router-dom";
import loadable from "@loadable/component";
import { useWindowSize } from "../../../store/hook-store/window-size-store";
import { Fragment } from "react";
const ToggleSidebar = loadable(() =>
  import("../../organisms/Containers/ToggleSidebar")
);

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

  const [sidebarState, setSidebarState] = useState({
    isLeftShown: true,
    isRightShown: false,
    isInit: true,
  });
  const [windowSize, resetWindowSize] = useWindowSize();

  useEffect(() => {
    if (windowSize.isSizeTypeChanged && !sidebarState.isInit) {
      setSidebarState({
        isLeftShown: true,
        isRightShown: false,
        isInit: true,
      });

      resetWindowSize();
    }
  }, [setSidebarState, windowSize, resetWindowSize, sidebarState.isInit]);

  const showLeftSidebar = () => {
    setSidebarState({
      isInit: false,
      isLeftShown: true,
      isRightShown: false,
    });
  };

  const showRightSidebar = () => {
    setSidebarState({
      isInit: false,
      isLeftShown: false,
      isRightShown: true,
    });
  };

  const resetSidebars = () => {
    setSidebarState({
      isLeftShown: true,
      isRightShown: false,
      isInit: true,
    });
  };

  const { isLeftShown, isRightShown, isInit } = sidebarState;
  return (
    <Fragment>
      <ToggleSidebar
        className="mb-4 d-lg-none"
        showRightSidebar={showRightSidebar}
        showLeftSidebar={showLeftSidebar}
        resetSidebars={resetSidebars}
        isLeftShown={isLeftShown}
        isRightShown={isRightShown}
      />
      <div className="row">
        {isLeftShown ? (
          <div className="col col-12 col-sm-12 col-md-12 col-lg-9">
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
        ) : null}
        {isRightShown || isInit ? (
          <div
            className={`col col-12 col-sm-12 col-md-4 col-lg-3 ${
              isRightShown && !isInit ? "" : "d-none"
            } d-lg-block`}
          >
            <ProfileInfo userInfo={userInfo} />
          </div>
        ) : null}
      </div>
    </Fragment>
  );
});
