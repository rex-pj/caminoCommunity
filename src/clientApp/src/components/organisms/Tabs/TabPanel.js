import React, { Fragment } from "react";

export default (props) => {
  const { tabComponent: TabComponent } = props;
  if (!TabComponent) {
    return <Fragment />;
  }
  return <TabComponent {...props} />;
};
