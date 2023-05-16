import * as React from "react";
import styled from "styled-components";
import { AnchorLink } from "../../atoms/Links";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

const Root = styled.ol`
  padding: 0;
  margin-bottom: 1rem;
  line-height: 1;
`;

const ListItem = styled.li`
  display: inline-block;
  font-size: ${(p) => p.theme.fontSize.small};
  color: ${(p) => p.theme.color.primaryText};
  padding: 0 0 ${(p) => p.theme.size.tiny} 0;
  min-width: ${(p) => p.theme.size.distance};
  position: relative;

  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
  max-width: 60%;

  a {
    text-decoration: none;
    color: inherit;
    font-weight: 600;
  }

  .actived {
    color: ${(p) => p.theme.color.darkText};
    border-right: 0;
  }

  svg,
  path {
    color: ${(p) => p.theme.color.darkText};
    font-size: ${(p) => p.theme.fontSize.small};
  }
`;

const Slash = styled.span`
  margin: 0 ${(p) => p.theme.size.exSmall};
  color: ${(p) => p.theme.color.secondaryText};
`;

export interface IBreadcrumbItem {
  isActived?: boolean;
  title?: string;
  url?: string;
}

type Props = {
  list: IBreadcrumbItem[];
  className?: string;
};

const Breadcrumb = (props: Props) => {
  const { list, className } = props;
  return (
    <Root className={className}>
      <ListItem>
        <AnchorLink to="/">
          <FontAwesomeIcon icon="home" />
        </AnchorLink>
      </ListItem>
      {list
        ? list.map((item, index) => {
            if (!item.isActived) {
              return (
                <ListItem title={item.title} key={index}>
                  <Slash>\</Slash>
                  <AnchorLink to={item.url}>{item.title}</AnchorLink>
                </ListItem>
              );
            }

            return (
              <ListItem key={index} className="actived" title={item.title}>
                <Slash>\</Slash>
                <span>{item.title}</span>
              </ListItem>
            );
          })
        : null}
    </Root>
  );
};

export default Breadcrumb;
