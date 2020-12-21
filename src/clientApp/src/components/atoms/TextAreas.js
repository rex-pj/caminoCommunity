import styled from "styled-components";

const TextAreaNeutral = styled.textarea`
  border: 1px solid ${(p) => p.theme.color.neutralBg};
  min-height: ${(p) => p.theme.size.normal};
  padding: 6px 5px;
  border-radius: ${(p) => p.theme.borderRadius.normal};
  font-size: ${(p) => p.theme.fontSize.normal};
  :focus {
    outline: 0;
  }
`;

export { TextAreaNeutral };
