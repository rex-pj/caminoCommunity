import React from "react";
import styled from "styled-components";
import { HorizontalList } from "../../atoms/List";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

const MenubarItem = styled.li`
  height: ${p => p.theme.size.medium};
  text-align: center;
  padding: 2px 0 0 0;
  line-height: 1;
`;

const Content = styled.div`
  margin-top: 3px;
  font-size: ${p => p.theme.fontSize.small};
  font-weight: 600;
  color: inherit;
`;

const ListBar = styled(HorizontalList)`
  height: ${p => p.theme.size.medium};

  ${MenubarItem} {
    width: ${p => `${p.percent}%`};
    text-align: center;
    border-left: 1px solid ${p => p.theme.rgbaColor.dark};
    color: ${p => p.theme.color.secondary};
  }

  ${MenubarItem}.first {
    border-left: 0;
  }

  svg {
    font-size: ${p => p.theme.fontSize.tiny};
    line-height: 1;
    vertical-align: initial;
    color: inherit;

    path {
      color: inherit;
    }
  }
`;

export default function (props) {
  const { menuList } = props;
  return (
    <ListBar percent={menuList ? 100 / menuList.length : 0}>
      {menuList
        ? menuList.map((item, index) => (
          <MenubarItem
            key={index}
            title={item.description}
            className={index === 0 ? "first" : null}
          >
            <FontAwesomeIcon icon={item.icon} />
            <Content>{item.text}</Content>
          </MenubarItem>
        ))
        : null}
    </ListBar>
  );
};
