import React, { useEffect, useState } from "react";
import styled from "styled-components";
import { PageColumnPanel } from "../../molecules/Panels";
import CommunityInfo from "../Community/CommunityInfo";
import {
  FarmSuggestions,
  CommunitySuggestions,
  ConnectionSuggestions,
} from "../../organisms/Suggestions";
import { useWindowSize } from "../../../store/hook-store/window-size-store";
import { navigationQueries } from "../../../graphql/fetching/queries";
import { useQuery } from "@apollo/client";
import Shortcut from "../../organisms/Shortcut";
import BodyLayout from "./BodyLayout";
import { Header } from "../../organisms/Containers";
import Notifications from "../../organisms/Notification/Notifications";
import Modal from "../../organisms/Modals/Modal";

const Interesting = React.lazy(() => import("../../organisms/Interesting"));
const ToggleSidebar = React.lazy(() =>
  import("../../organisms/Containers/ToggleSidebar")
);
const AdsList = React.lazy(() => import("../../organisms/Ads/AdsList"));

const Wrapper = styled.div`
  > .row {
    margin-left: -12px;
    margin-right: -12px;
  }

  > .row > .col {
    padding: 0 12px;
  }
`;

const Column = styled.div`
  margin-top: ${(p) => p.theme.size.normal};
`;

// The layout of Communitys
const CommunityLayout = (props) => {
  const { info, children, isLoading, hasData, hasError } = props;
  const [sidebarState, setSidebarState] = useState({
    isLeftShown: false,
    isRightShown: false,
    isCenterShown: true,
    isInit: true,
  });
  const [windowSize, resetWindowSize] = useWindowSize();
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
      isRightShown: false,
      isCenterShown: true,
    });
  };

  const { isLeftShown, isCenterShown, isRightShown, isInit } = sidebarState;
  return (
    <>
      <Header />
      <Wrapper className="container-fluid px-lg-5">
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
              <Column>
                <PageColumnPanel>
                  <CommunityInfo info={info} />
                </PageColumnPanel>
                <PageColumnPanel>
                  <Shortcut loading={shortcutLoading} data={shortcutData} />
                </PageColumnPanel>
                <PageColumnPanel>
                  <Interesting />
                </PageColumnPanel>
              </Column>
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
              <Column>
                <PageColumnPanel>
                  <FarmSuggestions />
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
                  <ConnectionSuggestions />
                </PageColumnPanel>
                <PageColumnPanel>
                  <AdsList />
                </PageColumnPanel>
              </Column>
            </div>
          ) : null}
        </div>
      </Wrapper>
      <Notifications />
      <Modal />
    </>
  );
};

export default CommunityLayout;
