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

const LoadingBody = (props: LoadingBodyProps) => {
  return (
    <>
      <Header />
      <div className="container-fluid px-lg-5">
        <LoadingBlock />
      </div>
    </>
  );
};

interface Props {
  children?: any;
}

const PageLoading = (props: Props) => {
  const onSearching = (value: any) => {};

  return (
    <MasterLayout
      {...props}
      component={<LoadingBody onSearching={onSearching} />}
    />
  );
};

export default PageLoading;
