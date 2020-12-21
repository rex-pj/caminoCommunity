import React from "react";
import styled from "styled-components";
import { VerticalList } from "../../atoms/List";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { ModuleMenuListItem } from "../../molecules/MenuList";
import { FifthHeadingPrimary } from "../../atoms/Heading";
import { NavLink } from "react-router-dom";

const Root = styled.div`
  margin-bottom: 15px;
`;

const ListControl = styled(VerticalList)`
  margin: 0;
`;

const IconBlock = styled.span`
  display: inline-block;
  width: 25px;
`;

export default function (props) {
  const shortcuts = [
    {
      icon: "seedling",
      text: "Vườn anh da đen",
      href: "/farms/1",
    },
    {
      icon: "piggy-bank",
      text: "Chuồng bò ông Sáu",
      href: "/farms/1",
    },
    {
      icon: "flag",
      text: "Đây là đâu tôi là ai Farm",
      href: "/farms/1",
    },
    {
      icon: "apple-alt",
      text: "Ruộng của anh Ba",
      href: "/farms/1",
    },
    {
      icon: "fish",
      text: "Ao cá bác Tư",
      href: "/farms/1",
    },
    {
      icon: "tree",
      text: "Khu vườn hạnh phúc",
      href: "/farms/1",
    },
    {
      icon: "flag",
      text: "Đây là đâu tôi là ai Farm",
      href: "/farms/1",
    },
    {
      icon: "apple-alt",
      text: "Ruộng của anh Ba",
      href: "/farms/1",
    },
    {
      icon: "fish",
      text: "Ao cá bác Tư",
      href: "/farms/1",
    },
    {
      icon: "tree",
      text: "Khu vườn hạnh phúc",
      href: "/farms/1",
    },
  ];

  return (
    <Root>
      <FifthHeadingPrimary>Quan Tâm</FifthHeadingPrimary>
      <ListControl>
        {shortcuts.map((item, index) => (
          <ModuleMenuListItem
            isActived={item.isActived}
            key={index}
            title={item.description}
            index={index}
          >
            <NavLink to={item.href}>
              <IconBlock>
                <FontAwesomeIcon icon={item.icon} />
              </IconBlock>
              <span>{item.text}</span>
            </NavLink>
          </ModuleMenuListItem>
        ))}
      </ListControl>
    </Root>
  );
}
