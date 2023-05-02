import React from "react";
import MasterLayout from "../../templates/Layout/MasterLayout";
import { Header } from "../../organisms/Containers";
import { LoadingBar } from "../../molecules/NotificationBars";
import styled from "styled-components";

const LoadingBlock = styled(LoadingBar)`
  border-radius: ${(p) => p.theme.borderRadius.normal};
  margin-top: ${(p) => p.theme.size.distance};
`;

const LoadingBody = (props) => {
  return (
    <>
      <Header onSearching={props.onSearching} />
      <div className="container-fluid px-lg-5">
        <LoadingBlock />
      </div>
    </>
  );
};

const PageLoading = (...rest) => {
  const onSearching = (value) => {};

  return (
    <MasterLayout
      {...rest}
      component={<LoadingBody onSearching={onSearching} />}
    />
  );
};

export default PageLoading;
