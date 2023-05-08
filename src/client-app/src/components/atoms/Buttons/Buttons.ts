import { ButtonHTMLAttributes } from "react";
import styled from "styled-components";

export interface ButtonProps extends ButtonHTMLAttributes<HTMLButtonElement> {
  readonly size?: string;
}

export const ButtonTransparent = styled.button<ButtonProps>`
  color: ${(p) => p.theme.color.darkText};
  border-radius: ${(p) => p.theme.borderRadius.normal};
  padding: ${(p) =>
    p.size === "xs"
      ? "5px 8px"
      : p.size === "sm"
      ? ".5rem .75rem"
      : "10px 15px"};
  background-color: transparent;
  border-width: 1px;
  border-style: solid;
  border-color: transparent;
  box-shadow: none;
  font-size: 1rem;
  font-weight: bold;
  box-sizing: border-box;
  outline: none;
  text-align: center;
  display: inline-block;
  :active,
  :hover,
  :focus-within {
    outline: none;
    background-color: transparent;
  }

  svg,
  path,
  span {
    color: inherit;
  }
`;

export const ButtonPrimary = styled(ButtonTransparent)<ButtonProps>`
  background-color: ${(p) => p.theme.color.primaryBg};
  color: ${(p) => p.theme.color.neutralText};
  border-color: ${(p) => p.theme.color.primaryBg};

  :active,
  :hover,
  :focus-within {
    background-color: ${(p) => p.theme.color.primaryBg};
  }

  :disabled {
    background-color: ${(p) => p.theme.color.primaryBg};
  }
`;

export const ButtonSecondary = styled(ButtonTransparent)`
  background-color: ${(p) => p.theme.color.secondaryBg};
  color: ${(p) => p.theme.color.neutralText};
  border-color: ${(p) => p.theme.color.secondaryBg};

  :active,
  :hover,
  :focus-within {
    background-color: ${(p) => p.theme.color.secondaryBg};
    color: ${(p) => p.theme.color.lightText};
  }

  :disabled {
    color: ${(p) => p.theme.color.darkText};
  }
`;

export const ButtonLight = styled(ButtonTransparent)`
  background-color: ${(p) => p.theme.color.lightBg};
  border-color: ${(p) => p.theme.color.neutralBg};
  color: ${(p) => p.theme.color.darkText};

  :active,
  :hover,
  :focus-within {
    background-color: ${(p) => p.theme.color.neutralBg};
  }

  :disabled {
    background-color: ${(p) => p.theme.color.neutralBg};
  }
`;

export const ButtonAlert = styled(ButtonTransparent)`
  border-color: ${(p) => p.theme.color.warnBg};
  background-color: ${(p) => p.theme.color.warnBg};
  color: ${(p) => p.theme.color.neutralText};

  :active,
  :hover,
  :focus-within {
    color: ${(p) => p.theme.color.whiteText};
  }

  :disabled {
    color: ${(p) => p.theme.color.whiteText};
  }
`;

export const ButtonCircleLight = styled(ButtonLight)`
  border-radius: 100%;
`;
