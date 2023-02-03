import React, { useContext, useEffect } from "react";
import styled from "styled-components";
import { PageColumnPanel } from "../../molecules/Panels";
import { farmQueries, userQueries } from "../../../graphql/fetching/queries";
import { useLazyQuery } from "@apollo/client";
import {
  FarmSuggestions,
  // CommunitySuggestions,
  ConnectionSuggestions,
} from "../../organisms/Suggestions";
import { SessionContext } from "../../../store/context/session-context";
import BodyLayout from "./BodyLayout";
import { Header } from "../../organisms/Containers";
import Notifications from "../../organisms/Notification/Notifications";
import Modal from "../../organisms/Modals/Modal";

// const AdsList = React.lazy(() => import("../../organisms/Ads/AdsList"));

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
  const [fetchFarms, { loading: suggestFarmloading, data: suggestFarmData }] =
    useLazyQuery(farmQueries.GET_FARMS);

  const [
    fetchSuggestions,
    { loading: suggestUserloading, data: suggestUserData },
  ] = useLazyQuery(userQueries.GET_SUGGESSTION_USERS);

  useEffect(() => {
    const currentUserId = currentUser?.userIdentityId;
    fetchFarms({
      variables: {
        criterias: {
          page: 1,
          pageSize: 3,
          exclusiveUserIdentityId: currentUserId,
        },
      },
    });

    fetchSuggestions({
      variables: {
        criterias: {
          page: 1,
          pageSize: 3,
          exclusiveUserIdentityId: currentUserId,
        },
      },
    });
  }, [fetchFarms, fetchSuggestions, currentUser?.userIdentityId]);

  return (
    <>
      <Header />
      <Wrapper className="container-fluid px-lg-5 mt-md-3 mt-lg-5">
        <div className="row px-lg-3 gx-3">
          <div className="col col-12 col-sm-12 col-md-12 col-lg-9">
            <BodyLayout
              isLoading={isLoading}
              hasData={hasData}
              hasError={hasError}
            >
              {children}
            </BodyLayout>
          </div>

          <div className="col col-12 col-sm-12 col-md-12 col-lg-3">
            <PageColumnPanel>
              <FarmSuggestions
                loading={suggestFarmloading}
                data={suggestFarmData}
              />
            </PageColumnPanel>
            {/* <PageColumnPanel>
              <AdsList />
            </PageColumnPanel>
            <PageColumnPanel>
              <CommunitySuggestions />
            </PageColumnPanel>
            <PageColumnPanel>
              <AdsList />
            </PageColumnPanel> */}
            <PageColumnPanel>
              <ConnectionSuggestions
                loading={suggestUserloading}
                data={suggestUserData}
              />
            </PageColumnPanel>
            {/* <PageColumnPanel>
              <AdsList />
            </PageColumnPanel> */}
          </div>
        </div>
      </Wrapper>
      <Notifications />
      <Modal />
    </>
  );
};

export default DefaultLayout;
