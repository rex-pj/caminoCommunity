import React from "react";
import { VerticalList } from "../../atoms/List";
import styled from "styled-components";
import { AnchorLink } from "../../atoms/Links";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { ModuleMenuListItem } from "../../molecules/MenuList";
import Loading from "../../atoms/Loading";

const IconBlock = styled.span`
  display: inline-block;
  width: 28px;
`;

export default function (props) {
  const { data, loading } = props;
  if (loading) {
    return <Loading>Loading</Loading>;
  }

  if (!data) {
    return null;
  }

  const { shortcuts } = data;
  return (
    <VerticalList>
      {shortcuts
        ? shortcuts.map((item, index) => (
            <ModuleMenuListItem
              isActived={item.isActived}
              key={index}
              title={item.description}
              index={index}
            >
              <AnchorLink to={item.url}>
                <IconBlock>
                  <FontAwesomeIcon icon={item.icon} />
                </IconBlock>
                <span className="menu-item-text">{item.name}</span>
              </AnchorLink>
            </ModuleMenuListItem>
          ))
        : null}
    </VerticalList>
  );
}
