import React from "react";
import styled from "styled-components";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { AnchorLink } from "../../atoms/Links";

const FontLink = styled(FontAwesomeIcon)`
  color: ${(p) => p.theme.color.neutralText};
  vertical-align: middle;
  cursor: pointer;
  margin-right: ${(p) => p.theme.size.exTiny};

  svg,
  path {
    color: inherit;
  }
`;

const Root = styled.div`
  display: inline-block;

  :hover path {
    color: ${(p) => p.theme.color.secondaryText};
  }
`;

const Link = styled(AnchorLink)`
  color: ${(p) => p.theme.color.neutralText};
  font-weight: 600;
  font-size: ${(p) => p.theme.fontSize.small};
  vertical-align: middle;
  cursor: pointer;

  :hover {
    color: ${(p) => p.theme.color.secondaryText};
  }
`;

export const FontButtonItem = (props) => {
  const { icon, title, dynamicText } = props;
  return (
    <Root>
      <FontLink icon={icon} />
      <Link to="#">
        {dynamicText} {title}
      </Link>
    </Root>
  );
};
