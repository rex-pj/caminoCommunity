import React from "react";
import loadable from "@loadable/component";
import FrameLayout from "./FrameLayout";
import styled from "styled-components";
import { PageColumnPanel } from "../../atoms/Panels";
import {
  FarmSuggestions,
  AssociationSuggestions,
  ConnectionSuggestions,
} from "../../organisms/Suggestions";

const Shortcut = loadable(() => import("../../organisms/Shortcut"));
const Interesting = loadable(() => import("../../organisms/Interesting"));
const LoggedInCard = loadable(() =>
  import("../../organisms/ProfileCard/LoggedInCard")
);
const AdsList = loadable(() => import("../../organisms/Ads/AdsList"));

const Wrapper = styled.div`
  margin-top: 30px;
  > .row {
    margin-left: -12px;
    margin-right: -12px;
  }

  > .row > .col {
    padding: 0 12px;
  }
`;

export default ({ component: Component, ...rest }) => {
  return (
    <FrameLayout
      {...rest}
      component={(matchProps) => (
        <Wrapper className="container-fluid px-lg-5">
          <div className="row px-lg-3">
            <div className="col col-4 col-sm-4 col-md-2 col-lg-2">
              <PageColumnPanel>
                <LoggedInCard />
              </PageColumnPanel>
              <PageColumnPanel>
                <Shortcut />
              </PageColumnPanel>
              <PageColumnPanel>
                <Interesting />
              </PageColumnPanel>
            </div>
            <div className="col col-8 col-sm-8 col-md-7 col-lg-7">
              <Component {...matchProps} />
            </div>
            <div className="col col-12 col-sm-12 col-md-3 col-lg-3">
              <PageColumnPanel>
                <FarmSuggestions />
              </PageColumnPanel>
              <PageColumnPanel>
                <AdsList />
              </PageColumnPanel>
              <PageColumnPanel>
                <AssociationSuggestions />
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
            </div>
          </div>
        </Wrapper>
      )}
    />
  );
};
