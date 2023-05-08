import styled from "styled-components";
import { Link, LinkProps } from "react-router-dom";

interface RouterLinkButtonProps extends LinkProps {
  readonly size?: string;
}

const ButtonTransparent = styled(Link)<RouterLinkButtonProps>`
  background-color: transparent;
  border: 0;
  box-shadow: none;
  padding: ${(p) => {
    if (p.size === "xs") {
      return "5px 8px";
    }
    if (p.size === "sm") {
      return ".5rem .75rem";
    }
    return "10px 15px";
  }};
  border-radius: ${(p) => p.theme.borderRadius.normal};
  font-size: 1rem;
  font-weight: bold;
  box-sizing: border-box;
  outline: none;
  text-align: center;
  display: inline-block;

  :active,
  :hover,
  :focus-within {
    background-color: transparent;
    outline: none;
  }

  svg,
  path {
    color: inherit;
  }
`;

const ButtonPrimary = styled(ButtonTransparent)`
  color: ${(p) => p.theme.color.whiteText};
  background-color: ${(p) => p.theme.color.primaryBg};

  :active,
  :hover,
  :focus-within {
    background-color: ${(p) => p.theme.color.primaryBg};
  }

  :disabled {
    background-color: ${(p) => p.theme.color.primaryBg};
  }
`;

const ButtonSecondary = styled(ButtonTransparent)`
  background-color: ${(p) => p.theme.color.primaryBg};
  color: ${(p) => p.theme.color.whiteText};

  :active,
  :hover,
  :focus-within {
    background-color: ${(p) => p.theme.color.primaryBg};
    outline: none;
  }

  :disabled {
    background-color: ${(p) => p.theme.color.primaryBg};
  }
`;

const ButtonOutlineNeutral = styled(ButtonTransparent)`
  border: 1px solid ${(p) => p.theme.color.neutralBg};
  color: ${(p) => p.theme.color.neutralText};

  :active,
  :hover,
  :focus-within {
    background-color: ${(p) => p.theme.rgbaColor.cyan};
  }

  :disabled {
    background-color: ${(p) => p.theme.rgbaColor.cyan};
  }
`;

export {
  ButtonTransparent as RouterLinkButtonTransparent,
  ButtonPrimary as RouterLinkButtonPrimary,
  ButtonSecondary as RouterLinkButtonSecondary,
  ButtonOutlineNeutral as RouterLinkButtonOutlineNeutral,
};
