import React, { Component } from "react";
import { VerticalList } from "../../atoms/List";
import styled from "styled-components";
import { AnchorLink } from "../../atoms/Links";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { ModuleMenuListItem } from "../../molecules/MenuList";

const IconBlock = styled.span`
  display: inline-block;
  width: 28px;
`;

export default class Shorcut extends Component {
  constructor() {
    super();

    this.state = {
      list: [
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
          text: "Associations",
          href: "/associations/",
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
      ],
    };
  }

  render() {
    const { list } = this.state;
    return (
      <VerticalList>
        {list.map((item, index) => (
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
              <span>{item.text}</span>
            </AnchorLink>
          </ModuleMenuListItem>
        ))}
      </VerticalList>
    );
  }
}
