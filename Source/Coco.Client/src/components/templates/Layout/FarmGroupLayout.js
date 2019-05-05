import React, { Component } from "react";
// import FrameLayout from "./FrameLayout";
import styled from "styled-components";
import { PageColumnPanel } from "../../atoms/Panels";
import AdsList from "../../organisms/Ads/AdsList";
import FarmGroupInfo from "../FarmGroup/FarmGroupInfo";
import {
  FarmSuggestions,
  GroupSuggestions,
  ConnectionSuggestions
} from "../../organisms/Suggestions";
import Interesting from "../../organisms/Interesting";
import Shorcut from "../../organisms/Shortcut";

const Wrapper = styled.div`
  > .row {
    margin-left: -8px;
    margin-right: -8px;
  }

  > .row > .col {
    padding: 0 8px;
  }
`;

const Column = styled.div`
  margin-top: ${p => p.theme.size.normal};
`;

export default class extends Component {
  render() {
    const { info, children } = this.props;

    return (
      <Wrapper className="container-fluid px-lg-5">
        <div className="row px-lg-3">
          <div className="col col-4 col-sm-4 col-md-2 col-lg-2">
            <Column>
              <PageColumnPanel>
                <FarmGroupInfo info={info} />
              </PageColumnPanel>
              <PageColumnPanel>
                <Shorcut />
              </PageColumnPanel>
              <PageColumnPanel>
                <Interesting />
              </PageColumnPanel>
            </Column>
          </div>
          <div className="col col-8 col-sm-8 col-md-7 col-lg-7">{children}</div>
          <div className="col col-12 col-sm-12 col-md-3 col-lg-3">
            <Column>
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
            </Column>
          </div>
        </div>
      </Wrapper>
    );
  }
}
