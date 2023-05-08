import { ButtonHTMLAttributes } from "react";
import styled from "styled-components";

interface OutlineButtonProps extends ButtonHTMLAttributes<HTMLButtonElement> {
  readonly size?: string;
}

const ButtonOutlinePrimary = styled.button<OutlineButtonProps>`
  color: ${(p) => p.theme.color.primaryBg};
  border-color: ${(p) => p.theme.color.primaryBg};
  border-width: 1px;
  border-style: solid;
  padding: ${(p) =>
    p.size === "xs"
      ? "5px 8px"
      : p.size === "sm"
      ? ".5rem .75rem"
      : "10px 15px"};
  border-radius: ${(p) => p.theme.borderRadius.normal};
  font-size: 1rem;
  font-weight: 600;
  box-sizing: border-box;
  outline: none;
  text-align: center;
  display: inline-block;
  background: transparent;

  :active,
  :hover,
  :focus-within {
    background-color: ${(p) => p.theme.rgbaColor.cyan};
    outline: none;
  }

  :disabled {
    background-color: ${(p) => p.theme.rgbaColor.cyan};
  }

  svg,
  path,
  span {
    color: inherit;
  }
`;

const ButtonOutlineLight = styled(ButtonOutlinePrimary)`
  border-color: ${(p) => p.theme.color.secondaryBg};
  color: ${(p) => p.theme.color.secondaryText};

  :active,
  :hover,
  :focus-within {
    background-color: ${(p) => p.theme.color.neutralBg};
  }

  :disabled {
    background-color: ${(p) => p.theme.color.neutralBg};
  }
`;

const ButtonOutlineDanger = styled(ButtonOutlinePrimary)`
  border-color: ${(p) => p.theme.color.dangerBg};
  color: ${(p) => p.theme.color.dangerText};

  :active,
  :hover,
  :focus-within {
    background-color: ${(p) => p.theme.color.warnText};
  }

  :disabled {
    background-color: ${(p) => p.theme.color.warnText};
  }
`;

const ButtonOutlineCircleLight = styled(ButtonOutlineLight)`
  border-radius: 100%;
`;

export {
  ButtonOutlinePrimary,
  ButtonOutlineLight,
  ButtonOutlineCircleLight,
  ButtonOutlineDanger,
};
