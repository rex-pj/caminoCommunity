import React from "react";
import styled from "styled-components";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { AnchorLink } from "../../atoms/Links";

const FontLink = styled(FontAwesomeIcon)`
  vertical-align: middle;
  cursor: pointer;
  margin-right: ${(p) => p.theme.size.exTiny};

  svg,
  path {
    color: ${(p) => p.theme.color.lightText};
  }
`;

const Root = styled.div`
  display: inline-block;
  color: ${(p) => p.theme.color.lightText};

  :hover {
    color: ${(p) => p.theme.color.neutralText};
  }

  :hover svg,
  :hover path {
    color: ${(p) => p.theme.color.neutralText};
  }
`;

const Link = styled(AnchorLink)`
  font-weight: 600;
  font-size: ${(p) => p.theme.fontSize.small};
  vertical-align: middle;
  cursor: pointer;
  color: inherit;
  :hover {
    color: inherit;
  }
`;

export const FontButtonItem = (props) => {
  const { icon, title, dynamicText } = props;
  return (
    <Root className={props.className}>
      <FontLink icon={icon} />
      <Link to="#">
        {dynamicText} {title}
      </Link>
    </Root>
  );
};
