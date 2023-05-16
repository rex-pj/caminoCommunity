import * as React from "react";
import { useEffect } from "react";
import styled from "styled-components";
import { PageColumnPanel } from "../../molecules/Panels";
import CommunityInfo from "../Community/CommunityInfo";
import {
  FarmSuggestions,
  CommunitySuggestions,
  ConnectionSuggestions,
} from "../../organisms/Suggestions";
import { navigationQueries } from "../../../graphql/fetching/queries";
import { useLazyQuery } from "@apollo/client";
import Shortcut from "../../organisms/Shortcut";
import BodyLayout from "./BodyLayout";
import { Header } from "../../organisms/Containers";
import Notifications from "../../organisms/Notification/Notifications";
import Modal from "../../organisms/Modals/Modal";

const Interesting = React.lazy(() => import("../../organisms/Interesting"));
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

interface Props {
  isLoading?: boolean;
  hasData?: boolean;
  hasError?: boolean;
  children?: any;
  info?: any;
}

// The layout of Communitys
const CommunityLayout = (props: Props) => {
  const { info, children, isLoading, hasData, hasError } = props;
  const [fetchShortcuts, { loading: shortcutLoading, data: shortcutData }] =
    useLazyQuery(navigationQueries.GET_SHORTCUTS);

  useEffect(() => {
    fetchShortcuts();
  }, [fetchShortcuts]);

  return (
    <>
      <Header />
      <Wrapper className="container-fluid px-lg-5">
        <div className="row px-lg-3">
          <div className="col col-12 col-sm-12 col-md-12 col-lg-2">
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
          <div className="col col-12 col-sm-12 col-md-12 col-lg-7">
            <BodyLayout
              isLoading={isLoading}
              hasData={hasData}
              hasError={hasError}
            >
              {children}
            </BodyLayout>
          </div>
        </div>
        <div className="col col-12 col-sm-12 col-md-12 col-lg-3">
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
      </Wrapper>
      <Notifications />
      <Modal />
    </>
  );
};

export default CommunityLayout;
