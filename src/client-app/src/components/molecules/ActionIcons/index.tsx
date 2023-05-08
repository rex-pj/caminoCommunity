import * as React from "react";
import styled from "styled-components";
import {
  FontAwesomeIcon,
  FontAwesomeIconProps,
} from "@fortawesome/react-fontawesome";
import { AnchorLink } from "../../atoms/Links";
import { IconProp } from "@fortawesome/fontawesome-svg-core";

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

interface FontLinkProps extends FontAwesomeIconProps {
  readonly hastext: string;
}

const FontLink = styled(FontAwesomeIcon)<FontLinkProps>`
  vertical-align: middle;
  margin-right: ${(p) => (p.hastext !== "false" ? p.theme.size.exTiny : 0)};

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

export interface ActionIconProps extends FontAwesomeIconProps {
  readonly dynamicText?: string;
}

const FontButtonItem: React.FC<ActionIconProps> = (props) => {
  const { icon, title, dynamicText, className } = props;
  const hasText = !!dynamicText || !!title;
  return (
    <Root className={className}>
      <FontLink icon={icon} hastext={hasText.toString()} />
      {hasText ? (
        <Link to="#">
          {dynamicText} {title}
        </Link>
      ) : null}
    </Root>
  );
};

export { FontButtonItem };
