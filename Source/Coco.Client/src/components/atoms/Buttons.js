import styled from "styled-components";

const Button = styled.button`
  color: ${p => p.theme.color.light};
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
  path,
  span {
    color: inherit;
  }
`;

const ButtonSecondary = styled(Button)`
  border: 1px solid ${p => p.theme.color.normal};
  background-color: ${p => p.theme.color.light};
  color: ${p => p.theme.color.normal};

  :active,
  :hover,
  :focus-within {
    background-color: ${p => p.theme.rgbaColor.moreLight};
  }

  :disabled {
    background-color: ${p => p.theme.rgbaColor.moreLight};
  }
`;

const ButtonAlert = styled(Button)`
  border: 1px solid ${p => p.theme.color.warning};
  background-color: ${p => p.theme.color.warningLight};
  color: ${p => p.theme.color.warning};

  :active,
  :hover,
  :focus-within {
    background-color: ${p => p.theme.color.warning};
    color: ${p => p.theme.color.warningLight};
  }

  :disabled {
    background-color: ${p => p.theme.color.warning};
    color: ${p => p.theme.color.warningLight};
  }
`;

const ButtonCircle = styled(Button)`
  border-radius: 100%;
`;

const ButtonTransparent = styled(Button)`
  color: ${p => p.theme.color.normal};
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
    background-color: ${p => p.theme.rgbaColor.cyanMoreLight};
    outline: none;
  }

  :disabled {
    background-color: ${p => p.theme.rgbaColor.cyanMoreLight};
  }
`;

const ButtonOutlineSecondary = styled(ButtonOutlinePrimary)`
  border: 1px solid ${p => p.theme.color.light};
  color: ${p => p.theme.color.light};

  :active,
  :hover,
  :focus-within {
    background-color: ${p => p.theme.rgbaColor.moreLight};
  }

  :disabled {
    background-color: ${p => p.theme.rgbaColor.moreLight};
  }
`;

const ButtonOutlineNormal = styled(ButtonOutlinePrimary)`
  border: 1px solid ${p => p.theme.color.normal};
  color: ${p => p.theme.color.normal};

  :active,
  :hover,
  :focus-within {
    background-color: ${p => p.theme.rgbaColor.cyanMoreLight};
  }

  :disabled {
    background-color: ${p => p.theme.rgbaColor.cyanMoreLight};
  }
`;

const ButtonOutlineDark = styled(ButtonOutlinePrimary)`
  background: ${p => p.theme.rgbaColor.moreDark};
  border: 1px solid ${p => p.theme.color.light};
  color: ${p => p.theme.color.light};

  :active,
  :hover,
  :focus-within {
    background-color: ${p => p.theme.rgbaColor.exDark};
  }

  :disabled {
    background-color: ${p => p.theme.rgbaColor.exDark};
  }
`;

const ButtonOutlineDanger = styled(ButtonOutlinePrimary)`
  border: 1px solid ${p => p.theme.color.dangerLight};
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
  Button,
  ButtonSecondary,
  ButtonCircle,
  ButtonTransparent,
  ButtonOutlineSecondary,
  ButtonOutlineDark,
  ButtonOutlineNormal,
  ButtonOutlinePrimary,
  ButtonAlert,
  ButtonOutlineDanger
};
