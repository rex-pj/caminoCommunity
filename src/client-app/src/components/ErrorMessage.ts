import styled from "styled-components";

export const ValidationWarningMessage = styled.p`
  color: ${(p) => p.theme.color.warnText};
  font-size: ${(p) => p.theme.fontSize.tiny};
`;

export const ValidationDangerMessage = styled.p`
  color: ${(p) => p.theme.color.dangerText};
  font-size: ${(p) => p.theme.fontSize.tiny};
`;
