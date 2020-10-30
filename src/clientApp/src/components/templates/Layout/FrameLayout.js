import React, { Fragment, useContext } from "react";
import MasterLayout from "./MasterLayout";
import { Header } from "../../organisms/Containers";
import { SessionContext } from "../../../store/context/SessionContext";
import PageLoading from "../../molecules/Loading/PageLoading";

// The layout with header
export default ({ ...props }) => {
  const { component: Component } = props;
  const sessionContext = useContext(SessionContext);

  if (!!sessionContext.isLoading) {
    return <PageLoading {...props} />;
  }

  return (
    <MasterLayout
      {...props}
      component={(matchProps) => (
        <Fragment>
          <Header />
          <Component {...matchProps} />
        </Fragment>
      )}
    />
  );
};
