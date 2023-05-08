import styled from "styled-components";

export const PrimaryTextbox = styled.input.attrs((p) => ({
  type: p.type === "password" ? "password" : p.type,
}))`
  height: ${(p) => p.theme.size.normal};
  border: 1px solid ${(p) => p.theme.color.primaryBg};
  padding: 6px 5px;
  border-radius: ${(p) => p.theme.borderRadius.normal};
  font-size: ${(p) => p.theme.fontSize.normal};
  :focus {
    outline: 0;
  }
`;

export const SecondaryTextbox = styled(PrimaryTextbox)`
  border: 1px solid ${(p) => p.theme.color.secondaryBg};
`;

export const LightTextbox = styled(PrimaryTextbox)`
  border: 1px solid ${(p) => p.theme.color.lightBg};
`;
