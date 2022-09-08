import React, { useContext, useState, useEffect } from "react";
import styled from "styled-components";
import { PageColumnPanel } from "../../molecules/Panels";
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
import BodyLayout from "./BodyLayout";
import { Header } from "../../organisms/Containers";
import Notifications from "../../organisms/Notification/Notifications";
import Modal from "../../organisms/Modals/Modal";

const ToggleSidebar = React.lazy(() =>
  import("../../organisms/Containers/ToggleSidebar")
);
const Shortcut = React.lazy(() => import("../../organisms/Shortcut"));
const Interesting = React.lazy(() => import("../../organisms/Interesting"));
const LoggedInCard = React.lazy(() =>
  import("../../organisms/ProfileCard/LoggedInCard")
);
const AdsList = React.lazy(() => import("../../organisms/Ads/AdsList"));

const Wrapper = styled.div`
  > .row {
    margin-left: -12px;
    margin-right: -12px;
  }
`;

// The layout default like home, products, articles or farms index page
const DefaultLayout = (props) => {
  const { children, isLoading, hasData, hasError } = props;
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
          exclusiveUserIdentityId: currentUser?.userIdentityId,
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
          exclusiveUserIdentityId: currentUser?.userIdentityId,
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
    <>
      <Header />
      <Wrapper className="container-fluid px-lg-5 mt-md-3 mt-lg-5">
        <ToggleSidebar
          className="mb-4 d-lg-none"
          showRightSidebar={showRightSidebar}
          showLeftSidebar={showLeftSidebar}
          resetSidebars={showCentral}
          isLeftShown={isLeftShown}
          isRightShown={isRightShown}
        />
        <div className="row px-lg-3 gx-3">
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
              <BodyLayout
                isLoading={isLoading}
                hasData={hasData}
                hasError={hasError}
              >
                {children}
              </BodyLayout>
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
      <Notifications />
      <Modal />
    </>
  );
};

export default DefaultLayout;
