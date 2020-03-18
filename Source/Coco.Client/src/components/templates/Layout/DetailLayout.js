import React from "react";
import FrameLayout from "./FrameLayout";
import styled from "styled-components";
import { PageColumnPanel } from "../../atoms/Panels";
import AdsList from "../../organisms/Ads/AdsList";
import AuthorCard from "../../organisms/ProfileCard/AuthorCard";
import {
  FarmSuggestions,
  GroupSuggestions,
  ConnectionSuggestions
} from "../../organisms/Suggestions";

const Wrapper = styled.div`
  margin-top: 30px;
  > .row {
    margin-left: -10px;
    margin-right: -10px;
  }

  > .row > .col {
    padding: 0 10px;
  }
`;

export default ({ component: Component, ...rest }) => {
  return (
    <FrameLayout
      {...rest}
      component={matchProps => (
        <Wrapper className="container-fluid px-lg-5">
          <div className="row px-lg-3">
            <div className="col col-4 col-sm-4 col-md-2 col-lg-2">
              <PageColumnPanel>
                <AuthorCard />
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
                <GroupSuggestions />
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
