import styled from "styled-components";

const Loading = styled.div`
  color: ${(p) => p.theme.color.neutralText};
  background-color: ${(p) => p.theme.color.lightBg};
  border: 1px solid ${(p) => p.theme.color.lightBg};
  text-align: center;
  padding: ${(p) => p.theme.size.tiny};
  width: 100%;
  border-radius: ${(p) => p.theme.borderRadius.normal};
`;

export default Loading;
