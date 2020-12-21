import React from "react";
import { VerticalList } from "../../atoms/List";
import styled from "styled-components";
import { AnchorLink } from "../../atoms/Links";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { ModuleMenuListItem } from "../../molecules/MenuList";

const IconBlock = styled.span`
  display: inline-block;
  width: 28px;
`;

export default function (props) {
  const shortcuts = [
    {
      icon: "list-alt",
      text: "Newsfeed",
      href: "/",
    },
    {
      icon: "warehouse",
      text: "Farms",
      href: "/farms/",
    },
    {
      icon: "flag",
      text: "Communities",
      href: "/communities/",
    },
    {
      icon: "apple-alt",
      text: "Products",
      href: "/products/",
    },
    {
      icon: "book",
      text: "Articles",
      href: "/articles/",
    },
  ];

  return (
    <VerticalList>
      {shortcuts.map((item, index) => (
        <ModuleMenuListItem
          isActived={item.isActived}
          key={index}
          title={item.description}
          index={index}
        >
          <AnchorLink to={item.href}>
            <IconBlock>
              <FontAwesomeIcon icon={item.icon} />
            </IconBlock>
            <span className="menu-item-text">{item.text}</span>
          </AnchorLink>
        </ModuleMenuListItem>
      ))}
    </VerticalList>
  );
}
