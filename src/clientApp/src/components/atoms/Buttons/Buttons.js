import styled from "styled-components";

const ButtonTransparent = styled.button`
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

const ButtonPrimary = styled(ButtonTransparent)`
  background-color: ${(p) => p.theme.color.primaryBg};
  color: ${(p) => p.theme.color.lightText};
  border-color: ${(p) => p.theme.color.secondaryBg};

  :active,
  :hover,
  :focus-within {
    background-color: ${(p) => p.theme.color.secondaryBg};
  }

  :disabled {
    background-color: ${(p) => p.theme.color.secondaryBg};
  }
`;

const ButtonLight = styled(ButtonTransparent)`
  background-color: ${(p) => p.theme.color.lightBg};
  border-color: ${(p) => p.theme.color.neutralBg};
  color: ${(p) => p.theme.color.primaryText};

  :active,
  :hover,
  :focus-within {
    background-color: ${(p) => p.theme.color.neutralBg};
  }

  :disabled {
    background-color: ${(p) => p.theme.color.neutralBg};
  }
`;

const ButtonAlert = styled(ButtonTransparent)`
  border-color: ${(p) => p.theme.color.primaryWarnText};
  background-color: ${(p) => p.theme.color.secondaryWarnBg};
  color: ${(p) => p.theme.color.primaryWarnText};

  :active,
  :hover,
  :focus-within {
    background-color: ${(p) => p.theme.color.primaryWarnBg};
    color: ${(p) => p.theme.color.secondaryWarnText};
  }

  :disabled {
    background-color: ${(p) => p.theme.color.primaryWarnBg};
    color: ${(p) => p.theme.color.secondaryWarnText};
  }
`;

const ButtonCircle = styled(ButtonTransparent)`
  border-radius: 100%;
`;

export {
  ButtonPrimary,
  ButtonLight,
  ButtonCircle,
  ButtonTransparent,
  ButtonAlert,
};
