import React, { useContext, useState, useEffect } from "react";
import loadable from "@loadable/component";
import styled from "styled-components";
import { PageColumnPanel } from "../../atoms/Panels";
import { farmQueries, userQueries } from "../../../graphql/fetching/queries";
import { useQuery } from "@apollo/client";
import {
  FarmSuggestions,
  CommunitySuggestions,
  ConnectionSuggestions,
} from "../../organisms/Suggestions";
import { SessionContext } from "../../../store/context/session-context";
import { useWindowSize } from "../../../store/hook-store/window-size-store";
import { navigationQueries } from "../../../graphql/fetching/queries";

const ToggleSidebar = loadable(() =>
  import("../../organisms/Containers/ToggleSidebar")
);
const Shortcut = loadable(() => import("../../organisms/Shortcut"));
const Interesting = loadable(() => import("../../organisms/Interesting"));
const LoggedInCard = loadable(() =>
  import("../../organisms/ProfileCard/LoggedInCard")
);
const AdsList = loadable(() => import("../../organisms/Ads/AdsList"));

const Wrapper = styled.div`
  > .row {
    margin-left: -12px;
    margin-right: -12px;
  }

  > .row > .col {
    padding: 0 12px;
  }
`;

// The layout default like home, products, articles or farms index page
export default (props) => {
  const { children } = props;
  const { currentUser } = useContext(SessionContext);
  const [sidebarState, setSidebarState] = useState({
    isLeftShown: false,
    isRightShown: false,
    isCenterShown: true,
    isInit: true,
  });

  const [windowSize, resetWindowSize] = useWindowSize();

  const { loading: suggestFarmloading, data: suggestFarmData } = useQuery(
    farmQueries.GET_FARMS,
    {
      variables: {
        criterias: {
          page: 1,
          pageSize: 3,
          exclusiveCreatedIdentityId: currentUser?.userIdentityId,
        },
      },
    }
  );

  const { loading: suggestUserloading, data: suggestUserData } = useQuery(
    userQueries.GET_SUGGESSTION_USERS,
    {
      variables: {
        criterias: {
          page: 1,
          pageSize: 3,
          exclusiveCreatedIdentityId: currentUser?.userIdentityId,
        },
      },
    }
  );

  const { loading: shortcutLoading, data: shortcutData } = useQuery(
    navigationQueries.GET_SHORTCUTS
  );

  useEffect(() => {
    if (windowSize.isSizeTypeChanged && !sidebarState.isInit) {
      setSidebarState({
        isLeftShown: false,
        isRightShown: false,
        isCenterShown: true,
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
      isCenterShown: false,
    });
  };

  const showRightSidebar = () => {
    setSidebarState({
      isInit: false,
      isLeftShown: false,
      isRightShown: true,
      isCenterShown: false,
    });
  };

  const showCentral = () => {
    setSidebarState({
      isInit: false,
      isLeftShown: false,
      isCenterShown: true,
      isRightShown: false,
    });
  };

  const { isLeftShown, isCenterShown, isRightShown, isInit } = sidebarState;
  return (
    <Wrapper className="container-fluid px-lg-5 mt-md-3 mt-lg-5">
      <ToggleSidebar
        className="mb-4 d-lg-none"
        showRightSidebar={showRightSidebar}
        showLeftSidebar={showLeftSidebar}
        resetSidebars={showCentral}
        isLeftShown={isLeftShown}
        isRightShown={isRightShown}
      />
      <div className="row px-lg-3">
        {isLeftShown || isInit ? (
          <div
            className={`col col-12 col-sm-12 col-md-12 col-lg-2 ${
              isLeftShown && !isInit ? "" : "d-none"
            } d-lg-block`}
          >
            <PageColumnPanel>
              <LoggedInCard />
            </PageColumnPanel>
            <PageColumnPanel>
              <Shortcut loading={shortcutLoading} data={shortcutData} />
            </PageColumnPanel>
            <PageColumnPanel>
              <Interesting />
            </PageColumnPanel>
          </div>
        ) : null}

        {isCenterShown ? (
          <div className="col col-12 col-sm-12 col-md-12 col-lg-7">
            {children}
          </div>
        ) : null}

        {isRightShown || isInit ? (
          <div
            className={`col col-12 col-sm-12 col-md-12 col-lg-3 ${
              isRightShown && !isInit ? "" : "d-none"
            } d-lg-block`}
          >
            <PageColumnPanel>
              <FarmSuggestions
                loading={suggestFarmloading}
                data={suggestFarmData}
              />
            </PageColumnPanel>
            <PageColumnPanel>
              <AdsList />
            </PageColumnPanel>
            <PageColumnPanel>
              <CommunitySuggestions />
            </PageColumnPanel>
            <PageColumnPanel>
              <AdsList />
            </PageColumnPanel>
            <PageColumnPanel>
              <ConnectionSuggestions
                loading={suggestUserloading}
                data={suggestUserData}
              />
            </PageColumnPanel>
            <PageColumnPanel>
              <AdsList />
            </PageColumnPanel>
          </div>
        ) : null}
      </div>
    </Wrapper>
  );
};
