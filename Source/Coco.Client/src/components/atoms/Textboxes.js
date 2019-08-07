import styled from "styled-components";

const Textbox = styled.input.attrs(p => ({
  type: p.type === "password" ? "password" : p.type
}))`
  height: ${p => p.theme.size.normal};
  border: 1px solid ${p => p.theme.color.neutral};
  padding: 6px 5px;
  border-radius: ${p => p.theme.borderRadius.normal};
  font-size: ${p => p.theme.fontSize.normal};
  :focus {
    outline: 0;
  }
`;

const TextboxSecondary = styled(Textbox)`
  border: 1px solid ${p => p.theme.color.primary};
`;

export { Textbox, TextboxSecondary };
