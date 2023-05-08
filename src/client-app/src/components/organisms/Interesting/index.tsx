import * as React from "react";
import styled from "styled-components";
import { VerticalList } from "../../molecules/List";
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
      text: "Sixth brother orchard",
      href: "/farms/1",
    },
    {
      icon: "piggy-bank",
      text: "Eighth sister byre",
      href: "/farms/1",
    },
    {
      icon: "flag",
      text: "Seventh sister Durian farm",
      href: "/farms/1",
    },
    {
      icon: "apple-alt",
      text: "Third brother rice field",
      href: "/farms/1",
    },
    {
      icon: "fish",
      text: "Fourth uncle fish pond",
      href: "/farms/1",
    },
    {
      icon: "tree",
      text: "Happy garden",
      href: "/farms/1",
    },
    {
      icon: "flag",
      text: "Ninth brother Garden",
      href: "/farms/1",
    },
    {
      icon: "apple-alt",
      text: "Baro's field",
      href: "/farms/1",
    },
    {
      icon: "fish",
      text: "Fish pond of Ninth sister",
      href: "/farms/1",
    },
    {
      icon: "tree",
      text: "David's garden",
      href: "/farms/1",
    },
  ];

  return (
    <Root>
      <FifthHeadingPrimary>Interested in</FifthHeadingPrimary>
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
