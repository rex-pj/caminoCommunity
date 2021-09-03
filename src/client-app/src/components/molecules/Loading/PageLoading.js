import React, { Fragment } from "react";
import MasterLayout from "../../templates/Layout/MasterLayout";
import { Header } from "../../organisms/Containers";
import Loading from "../../atoms/Loading";
import styled from "styled-components";

const LoadingBlock = styled(Loading)`
  border-radius: ${(p) => p.theme.borderRadius.normal};
  margin-top: ${(p) => p.theme.size.distance};
`;

export default (...rest) => {
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
