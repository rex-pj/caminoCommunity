import * as React from "react";
import { Fragment } from "react";

type Props = {
  tabComponent: any;
  tabOrder: number;
};

const TabPanel = (props: Props) => {
  const { tabComponent: TabComponent } = props;
  if (!TabComponent) {
    return <Fragment />;
  }
  return <TabComponent {...props} />;
};

export default TabPanel;
