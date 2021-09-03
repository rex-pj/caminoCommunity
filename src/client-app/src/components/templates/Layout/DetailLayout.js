import React, { useContext, useEffect, useState } from "react";
import styled from "styled-components";
import { farmQueries, userQueries } from "../../../graphql/fetching/queries";
import { PageColumnPanel } from "../../atoms/Panels";
import AdsList from "../../organisms/Ads/AdsList";
import AuthorCard from "../../organisms/ProfileCard/AuthorCard";
import {
  FarmSuggestions,
  CommunitySuggestions,
  ConnectionSuggestions,
} from "../../organisms/Suggestions";
import { useQuery } from "@apollo/client";
import { SessionContext } from "../../../store/context/session-context";
import { useWindowSize } from "../../../store/hook-store/window-size-store";
import loadable from "@loadable/component";

const ToggleSidebar = loadable(() =>
  import("../../organisms/Containers/ToggleSidebar")
);
const Wrapper = styled.div`
  > .row {
    margin-left: -12px;
    margin-right: -12px;
  }

  > .row > .col {
    padding: 0 12px;
  }
`;

// The layout or article or farm detail page
export default (props) => {
  const { currentUser } = useContext(SessionContext);

  const [sidebarState, setSidebarState] = useState({
    isLeftShown: true,
    isCenterShown: true,
    isRightShown: true,
    isInit: true,
  });

  const { loading: suggestFarmloading, data: suggestFarmdata } = useQuery(
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

  const [windowSize, resetWindowSize] = useWindowSize();

  useEffect(() => {
    if (windowSize.isSizeTypeChanged && !sidebarState.isInit) {
      setSidebarState({
        isLeftShown: true,
        isCenterShown: true,
        isRightShown: true,
        isInit: true,
      });

      resetWindowSize();
    }
  }, [setSidebarState, windowSize, resetWindowSize, sidebarState.isInit]);

  const showLeftSidebar = () => {
    setSidebarState({
      isInit: false,
      isLeftShown: true,
      isCenterShown: false,
      isRightShown: false,
    });
  };

  const showRightSidebar = () => {
    setSidebarState({
      isInit: false,
      isLeftShown: false,
      isCenterShown: false,
      isRightShown: true,
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
  const { author, children } = props;
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
              <AuthorCard author={author} />
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
                data={suggestFarmdata}
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
