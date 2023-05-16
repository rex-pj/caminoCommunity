import * as React from "react";
import { VerticalList } from "../../molecules/List";
import styled from "styled-components";
import { AnchorLink } from "../../atoms/Links";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { ModuleMenuListItem } from "../../molecules/MenuList";
import { LoadingBar } from "../../molecules/NotificationBars";
import { IconProp } from "@fortawesome/fontawesome-svg-core";

const IconBlock = styled.span`
  display: inline-block;
  width: 28px;
`;

interface Props {
  data?: {
    shortcuts?: {
      isActived: boolean;
      id: number;
      description: string;
      text: string;
      url: string;
      icon: IconProp;
      name: string;
    }[];
  };
  loading?: boolean;
}

const Shortcuts = (props: Props) => {
  const { data, loading } = props;
  if (loading) {
    return <LoadingBar />;
  }

  if (!data) {
    return null;
  }

  const { shortcuts } = data;
  return (
    <VerticalList className="row">
      {shortcuts
        ? shortcuts.map((item, index) => (
            <ModuleMenuListItem
              className="col-4 col-sm-12"
              isActived={item.isActived}
              key={item.id}
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
};

export default Shortcuts;
