import styled from "styled-components";
import { Link } from "react-router-dom";

const Button = styled(Link)`
  color: ${p => p.theme.color.white};
  background-color: ${p => p.theme.color.primary};
  padding: ${p => (p.size === "sm" ? ".5rem .75rem" : "10px 15px")};
  border-radius: ${p => p.theme.borderRadius.normal};
  border: 0;
  font-size: 1rem;
  font-weight: bold;
  box-sizing: border-box;
  outline: none;
  text-align: center;
  display: inline-block;

  :active,
  :hover,
  :focus-within {
    background-color: ${p => p.theme.color.primaryDark};
    outline: none;
  }

  :disabled {
    background-color: ${p => p.theme.color.primaryDark};
  }

  svg,
  path {
    color: inherit;
  }
`;

const ButtonCircle = styled(Button)`
  border-radius: 100%;
`;

const ButtonSecondary = styled(Button)`
  color: ${p => p.theme.color.white};
  background-color: ${p => p.theme.color.primaryLight};

  :active,
  :hover,
  :focus-within {
    background-color: ${p => p.theme.color.primaryDark};
    outline: none;
  }

  :disabled {
    background-color: ${p => p.theme.color.primaryDark};
  }
`;

const ButtonTransparent = styled(Button)`
  background-color: transparent;
  border: 0;
  box-shadow: none;

  :active,
  :hover,
  :focus-within {
    background-color: transparent;
  }
`;

const ButtonOutlinePrimary = styled(Button)`
  background: transparent;
  border: 1px solid ${p => p.theme.color.primary};
  color: ${p => p.theme.color.primary};

  :active,
  :hover,
  :focus-within {
    background-color: ${p => p.theme.rgbaColor.cyan};
    outline: none;
  }

  :disabled {
    background-color: ${p => p.theme.rgbaColor.cyan};
  }
`;

const ButtonOutlineSecondary = styled(ButtonOutlinePrimary)`
  border: 1px solid ${p => p.theme.color.light};
  color: ${p => p.theme.color.light};

  :active,
  :hover,
  :focus-within {
    background-color: ${p => p.theme.rgbaColor.lighter};
  }

  :disabled {
    background-color: ${p => p.theme.rgbaColor.lighter};
  }
`;

const ButtonOutlineNormal = styled(ButtonOutlinePrimary)`
  border: 1px solid ${p => p.theme.color.neutral};
  color: ${p => p.theme.color.neutral};

  :active,
  :hover,
  :focus-within {
    background-color: ${p => p.theme.rgbaColor.cyan};
  }

  :disabled {
    background-color: ${p => p.theme.rgbaColor.cyan};
  }
`;

const ButtonOutlineDark = styled(ButtonOutlinePrimary)`
  background: ${p => p.theme.rgbaColor.dark};
  border: 1px solid ${p => p.theme.color.light};
  color: ${p => p.theme.color.light};

  :active,
  :hover,
  :focus-within {
    background-color: ${p => p.theme.rgbaColor.darker};
  }

  :disabled {
    background-color: ${p => p.theme.rgbaColor.darker};
  }
`;

export {
  Button as RouterLinkButton,
  ButtonSecondary as RouterLinkButtonSecondary,
  ButtonCircle as RouterLinkButtonCircle,
  ButtonTransparent as RouterLinkButtonTransparent,
  ButtonOutlineSecondary as RouterLinkButtonOutlineSecondary,
  ButtonOutlineDark as RouterLinkButtonOutlineDark,
  ButtonOutlineNormal as RouterLinkButtonOutlineNormal,
  ButtonOutlinePrimary as RouterLinkButtonOutlinePrimary
};
