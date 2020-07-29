import React from "react";

export default props => {
  const { tabComponent: TabComponent } = props;
  return <TabComponent {...props} />;
};
