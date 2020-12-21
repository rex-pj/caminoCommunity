import styled from "styled-components";

const PrimaryTextbox = styled.input.attrs((p) => ({
  type: p.type === "password" ? "password" : p.type,
}))`
  height: ${(p) => p.theme.size.normal};
  border: 1px solid ${(p) => p.theme.color.primaryDivide};
  padding: 6px 5px;
  border-radius: ${(p) => p.theme.borderRadius.normal};
  font-size: ${(p) => p.theme.fontSize.normal};
  :focus {
    outline: 0;
  }
`;

const SecondaryTextbox = styled(PrimaryTextbox)`
  border: 1px solid ${(p) => p.theme.color.secondaryBg};
`;

export { PrimaryTextbox, SecondaryTextbox };
