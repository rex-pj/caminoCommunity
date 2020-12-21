import styled from "styled-components";

const LabelPrimary = styled.label`
  font-size: ${(p) => p.theme.fontSize.normal};
  color: ${(p) => p.theme.color.secondaryText};
`;

const LabelSecondary = styled(LabelPrimary)`
  color: ${(p) => p.theme.color.secondaryText};
`;

const LabelNormal = styled(LabelPrimary)`
  color: ${(p) => p.theme.color.neutralText};
`;

const LabelLight = styled(LabelPrimary)`
  color: ${(p) => p.theme.color.neutralText};
`;

const LabelDark = styled(LabelPrimary)`
  color: ${(p) => p.theme.color.darkText};
`;

export { LabelPrimary, LabelSecondary, LabelNormal, LabelLight, LabelDark };
