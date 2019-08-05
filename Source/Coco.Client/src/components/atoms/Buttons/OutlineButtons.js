import styled from "styled-components";

const ButtonOutlinePrimary = styled.button`
  color: ${p => p.theme.color.light};
  padding: ${p =>
    p.size === "xs"
      ? "5px 8px"
      : p.size === "sm"
      ? ".5rem .75rem"
      : "10px 15px"};
  border-radius: ${p => p.theme.borderRadius.normal};
  border-width: 1px;
  border-style: solid;
  border-color: ${p => p.theme.color.primary};
  font-size: 1rem;
  font-weight: 600;
  box-sizing: border-box;
  outline: none;
  text-align: center;
  display: inline-block;

  background: transparent;
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

  svg,
  path,
  span {
    color: inherit;
  }
`;

const ButtonOutlineSecondary = styled(ButtonOutlinePrimary)`
  border-color: ${p => p.theme.color.light};
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
  border-color: ${p => p.theme.color.neutral};
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
  border-color: ${p => p.theme.color.light};
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

const ButtonOutlineDanger = styled(ButtonOutlinePrimary)`
  border-color: ${p => p.theme.color.dangerLight};
  color: ${p => p.theme.color.dangerLight};

  :active,
  :hover,
  :focus-within {
    color: ${p => p.theme.color.danger};
    background-color: transparent;
  }

  :disabled {
    color: ${p => p.theme.color.danger};
  }
`;

export {
  ButtonOutlineSecondary,
  ButtonOutlineDark,
  ButtonOutlineNormal,
  ButtonOutlinePrimary,
  ButtonOutlineDanger
};
