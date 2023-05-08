import * as React from "react";
import MasterLayout from "../../templates/Layout/MasterLayout";
import { Header } from "../../organisms/Containers";
import { LoadingBar } from "../NotificationBars";
import styled from "styled-components";

const LoadingBlock = styled(LoadingBar)`
  border-radius: ${(p) => p.theme.borderRadius.normal};
  margin-top: ${(p) => p.theme.size.distance};
`;

interface LoadingBodyProps {
  onSearching: any;
}

const LoadingBody: React.FC<LoadingBodyProps> = (props) => {
  return (
    <>
      <Header onSearching={props.onSearching} />
      <div className="container-fluid px-lg-5">
        <LoadingBlock />
      </div>
    </>
  );
};

interface PageLoadingProps {
  children?: any;
}

const PageLoading: React.FC<PageLoadingProps> = (...rest) => {
  const onSearching = (value: any) => {};

  return (
    <MasterLayout
      {...rest}
      component={<LoadingBody onSearching={onSearching} />}
    />
  );
};

export default PageLoading;
