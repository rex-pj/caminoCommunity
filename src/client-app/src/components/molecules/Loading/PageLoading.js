import React, { Fragment } from "react";
import MasterLayout from "../../templates/Layout/MasterLayout";
import { Header } from "../../organisms/Containers";
import { LoadingBar } from "../../molecules/NotificationBars";
import styled from "styled-components";

const LoadingBlock = styled(LoadingBar)`
  border-radius: ${(p) => p.theme.borderRadius.normal};
  margin-top: ${(p) => p.theme.size.distance};
`;

const PageLoading = (...rest) => {
  const onSearching = (value) => {};

  return (
    <MasterLayout
      {...rest}
      component={(matchProps) => (
        <Fragment>
          <Header onSearching={onSearching} />
          <div className="container-fluid px-lg-5">
            <LoadingBlock>Loading</LoadingBlock>
          </div>
        </Fragment>
      )}
    />
  );
};

export default PageLoading;
