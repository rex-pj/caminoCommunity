import React, { useContext } from "react";
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

// The layout default like home, products, articles or farms index page
export default (props) => {
  const { children } = props;
  const { currentUser } = useContext(SessionContext);

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

  return (
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
        <div className="col col-8 col-sm-8 col-md-7 col-lg-7">{children}</div>
        <div className="col col-12 col-sm-12 col-md-3 col-lg-3">
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
      </div>
    </Wrapper>
  );
};
