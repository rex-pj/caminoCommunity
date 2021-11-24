import React from "react";
import styled from "styled-components";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { AnchorLink } from "../../atoms/Links";

const Root = styled.div`
  display: inline-block;
  color: ${(p) => p.theme.color.primaryText};
  text-align: center;
  min-width: ${(p) => p.theme.size.normal};
  cursor: pointer;

  :hover {
    color: ${(p) => p.theme.color.secondaryText};
  }

  :hover svg,
  :hover path {
    color: inherit;
  }
`;

const FontLink = styled(FontAwesomeIcon)`
  vertical-align: middle;
  margin-right: ${(p) => (p.hasText ? p.theme.size.exTiny : 0)};

  svg,
  path {
    color: ${(p) => p.theme.color.primaryText};
  }
`;

const Link = styled(AnchorLink)`
  font-weight: 600;
  font-size: ${(p) => p.theme.fontSize.tiny};
  vertical-align: middle;
  color: inherit;
  :hover {
    color: inherit;
  }
`;

export const FontButtonItem = (props) => {
  const { icon, title, dynamicText } = props;
  const hasText = !!dynamicText || !!title;
  return (
    <Root className={props.className}>
      <FontLink icon={icon} hasText={hasText} />
      {hasText ? (
        <Link to="#">
          {dynamicText} {title}
        </Link>
      ) : null}
    </Root>
  );
};
