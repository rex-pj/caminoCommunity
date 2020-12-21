import React from "react";
import styled from "styled-components";
import { ButtonPrimary } from "../../atoms/Buttons/Buttons";

const TabButton = styled(ButtonPrimary)`
  border-bottom-right-radius: 0;
  border-bottom-left-radius: 0;
  font-weight: normal;
  border: 0;
  color: ${(p) => p.theme.color.darkText};
  background-color: ${(p) => p.theme.color.neutralBg};

  :hover {
    background-color: ${(p) => p.theme.color.neutralBg};
  }
`;

const Item = styled.li`
  display: inline-block;

  &.actived ${TabButton} {
    color: ${(p) => p.theme.color.neutralText};
    background-color: ${(p) => p.theme.color.neutralBg};
  }

  ${TabButton} {
    border-top-right-radius: ${(p) =>
      p.tabOrder === 0 ? "0px" : p.theme.borderRadius.normal};
    border-top-left-radius: ${(p) =>
      p.tabOrder === p.totalTabs - 1 ? "0px" : p.theme.borderRadius.normal};
  }
`;

export default (props) => {
  const { className, tabOrder, totalTabs, actived } = props;

  const toggleTab = (e) => {
    if (props.toggleTab) {
      props.toggleTab(e, tabOrder);
    }
  };

  const activedClass = `${className ? className : ""}${
    !!actived ? " actived" : ""
  }`;
  return (
    <Item className={activedClass} tabOrder={tabOrder} totalTabs={totalTabs}>
      <TabButton type="button" size="sm" onClick={toggleTab}>
        {props.title}
      </TabButton>
    </Item>
  );
};
