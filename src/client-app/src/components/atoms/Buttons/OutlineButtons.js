import styled from "styled-components";

const ButtonOutlinePrimary = styled.button`
  color: ${(p) => p.theme.color.primaryBg};
  border-color: ${(p) => p.theme.color.primaryText};
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
  border-color: ${(p) => p.theme.color.neutralBg};
  color: ${(p) => p.theme.color.neutralText};

  :active,
  :hover,
  :focus-within {
    background-color: ${(p) => p.theme.rgbaColor.lighter};
  }

  :disabled {
    background-color: ${(p) => p.theme.rgbaColor.lighter};
  }
`;

const ButtonOutlineDanger = styled(ButtonOutlinePrimary)`
  border-color: ${(p) => p.theme.color.secondaryDangerBg};
  color: ${(p) => p.theme.color.secondaryDangerText};

  :active,
  :hover,
  :focus-within {
    color: ${(p) => p.theme.color.primaryDangerText};
    background-color: transparent;
  }

  :disabled {
    color: ${(p) => p.theme.color.primaryDangerText};
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
