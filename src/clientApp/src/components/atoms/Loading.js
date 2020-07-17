import styled from "styled-components";

const Loading = styled.div`
  color: ${p => p.theme.color.primaryDark};
  background-color: ${p => p.theme.color.light};
  border: spx solid ${p => p.theme.color.light};
  text-align: center;
  padding: ${p => p.theme.size.tiny};
`;

export default Loading;
