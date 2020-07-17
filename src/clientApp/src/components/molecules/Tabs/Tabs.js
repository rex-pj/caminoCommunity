import React, { useState } from "react";
import styled from "styled-components";
import Tab from "./Tab";
import TabPanel from "./TabPanel";

const Root = styled.div``;

const List = styled.ul`
  list-style: none;
  padding-left: 0;
  border-bottom: 1px solid ${(p) => p.theme.color.neutral};
  margin-bottom: 0;
`;

export default (props) => {
  const { tabs, className, onTabClicked } = props;
  const [currentTabIndex, setCurrentTabIndex] = useState(0);

  const toggleTab = (e, index) => {
    setCurrentTabIndex(index);
    if (onTabClicked) {
      onTabClicked(e);
    }
  };

  return (
    <Root className={className}>
      <List className="tabs-bar">
        {tabs
          ? tabs.map((tab, index) => (
              <Tab
                key={index}
                tabOrder={index}
                totalTabs={tabs.length}
                {...tab}
                toggleTab={toggleTab}
                actived={currentTabIndex === index}
              />
            ))
          : null}
      </List>
      {tabs
        ? tabs.map((tab, index) =>
            index === currentTabIndex ? (
              <TabPanel
                key={index}
                tabOrder={index}
                tabComponent={tab.tabComponent}
              />
            ) : null
          )
        : null}
    </Root>
  );
};
