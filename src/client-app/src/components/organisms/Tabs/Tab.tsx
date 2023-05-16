import * as React from "react";
import { LiHTMLAttributes } from "react";
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

interface ItemProps {
  tabOrder: number;
  totalTabs: number;
}

const Item = styled.li<ItemProps>`
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

interface TabProps extends LiHTMLAttributes<HTMLLIElement> {
  tabOrder: number;
  totalTabs: number;
  actived?: boolean;
  toggleTab?: (
    e: React.MouseEvent<HTMLButtonElement>,
    tabOrder: number
  ) => void;
}

const Tab: React.FC<TabProps> = (props) => {
  const { className, tabOrder, totalTabs, actived } = props;

  const toggleTab = (e: React.MouseEvent<HTMLButtonElement>) => {
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

export { Tab };
