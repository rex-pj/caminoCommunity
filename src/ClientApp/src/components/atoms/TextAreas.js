import styled from "styled-components";

const TextArea = styled.textarea`
  min-height: ${p => p.theme.size.normal};
  border: 1px solid ${p => p.theme.color.neutral};
  padding: 6px 5px;
  border-radius: ${p => p.theme.borderRadius.normal};
  font-size: ${p => p.theme.fontSize.normal};
  :focus {
    outline: 0;
  }
`;

const TextAreaSecondary = styled(TextArea)`
  border: 1px solid ${p => p.theme.color.primary};
`;

export { TextArea, TextAreaSecondary };
