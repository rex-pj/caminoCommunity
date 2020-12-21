import styled from "styled-components";

const Loading = styled.div`
  color: ${(p) => p.theme.color.primaryText};
  background-color: ${(p) => p.theme.color.lightBg};
  border: spx solid ${(p) => p.theme.color.neutralText};
  text-align: center;
  padding: ${(p) => p.theme.size.tiny};
`;

export default Loading;
