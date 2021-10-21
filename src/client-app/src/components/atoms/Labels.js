import styled from "styled-components";

export const LabelPrimary = styled.label`
  font-size: ${(p) => p.theme.fontSize.normal};
  color: ${(p) => p.theme.color.primaryText};
`;

export const LabelSecondary = styled(LabelPrimary)`
  color: ${(p) => p.theme.color.secondaryText};
`;

export const LabelNormal = styled(LabelPrimary)`
  color: ${(p) => p.theme.color.neutralText};
`;

export const LabelLight = styled(LabelPrimary)`
  color: ${(p) => p.theme.color.lightText};
`;

export const LabelDark = styled(LabelPrimary)`
  color: ${(p) => p.theme.color.darkText};
`;
