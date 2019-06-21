import styled from "styled-components";

const ErrorBlock = styled.div`
  color: ${p => p.theme.color.warning};
  background-color: ${p => p.theme.color.warningLight};
  border: spx solid ${p => p.theme.color.warningLight};
  text-align: center;
  padding: ${p => p.theme.size.tiny};
`;

export default ErrorBlock;
