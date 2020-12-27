import styled from "styled-components";

const ErrorBlock = styled.div`
  color: ${(p) => p.theme.color.secondaryWarnBg};
  background-color: ${(p) => p.theme.color.primaryWarnText};
  border: spx solid ${(p) => p.theme.color.secondaryWarnBg};
  text-align: center;
  padding: ${(p) => p.theme.size.tiny};
`;

export default ErrorBlock;
