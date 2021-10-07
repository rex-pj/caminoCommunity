import styled from "styled-components";

export const ErrorBar = styled.div`
  color: ${(p) => p.theme.color.secondaryWarnBg};
  background-color: ${(p) => p.theme.color.primaryWarnText};
  border: 1px solid ${(p) => p.theme.color.secondaryWarnBg};
  text-align: center;
  padding: ${(p) => p.theme.size.tiny};
`;

export const LoadingBar = styled.div`
  color: ${(p) => p.theme.color.neutralText};
  background-color: ${(p) => p.theme.color.lightBg};
  border: 1px solid ${(p) => p.theme.color.lightBg};
  text-align: center;
  padding: ${(p) => p.theme.size.tiny};
  width: 100%;
  border-radius: ${(p) => p.theme.borderRadius.normal};
`;

export const NoDataBar = styled.div`
  color: ${(p) => p.theme.color.lightBg};
  background-color: ${(p) => p.theme.color.neutralText};
  border: 1px solid ${(p) => p.theme.color.lightBg};
  text-align: center;
  padding: ${(p) => p.theme.size.tiny};
`;
