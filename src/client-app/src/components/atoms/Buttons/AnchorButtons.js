import styled from "styled-components";

const ButtonTransparent = styled.a`
  background-color: transparent;
  border: 0;
  box-shadow: none;
  padding: ${(p) => (p.size === "sm" ? ".5rem .75rem" : "10px 15px")};
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
  }

  svg,
  path {
    color: inherit;
  }
`;

const ButtonPrimary = styled(ButtonTransparent)`
  color: ${(p) => p.theme.color.whiteText};
  background-color: ${(p) => p.theme.color.primaryBg};
  border-radius: ${(p) => p.theme.borderRadius.normal};
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

export { ButtonPrimary as AnchorButtonPrimary };
