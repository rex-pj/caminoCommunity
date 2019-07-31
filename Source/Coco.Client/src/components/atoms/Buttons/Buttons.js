import styled from "styled-components";

const ButtonPrimary = styled.button`
  color: ${p => p.theme.color.light};
  background-color: ${p => p.theme.color.primary};
  padding: ${p => (p.size === "sm" ? ".5rem .75rem" : "10px 15px")};
  border-radius: ${p => p.theme.borderRadius.normal};
  border-width: 1px;
  border-style: solid;
  border-color: ${p => p.theme.color.primary};
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

const ButtonSecondary = styled(ButtonPrimary)`
  border-color: ${p => p.theme.color.neutral};
  background-color: ${p => p.theme.color.light};
  color: ${p => p.theme.color.neutral};

  :active,
  :hover,
  :focus-within {
    background-color: ${p => p.theme.rgbaColor.lighter};
  }

  :disabled {
    background-color: ${p => p.theme.rgbaColor.lighter};
  }
`;

const ButtonAlert = styled(ButtonPrimary)`
  border-color: ${p => p.theme.color.warning};
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

const ButtonCircle = styled(ButtonPrimary)`
  border-radius: 100%;
`;

const ButtonTransparent = styled(ButtonPrimary)`
  color: ${p => p.theme.color.neutral};
  background-color: transparent;
  border: 1px solid transparent;
  box-shadow: none;

  :active,
  :hover,
  :focus-within {
    background-color: transparent;
  }
`;

export {
  ButtonPrimary,
  ButtonSecondary,
  ButtonCircle,
  ButtonTransparent,
  ButtonAlert
};
