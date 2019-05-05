import styled from "styled-components";

const LabelPrimary = styled.label`
  font-size: ${p => p.theme.fontSize.normal};
  color: ${p => p.theme.color.secondary};
`;

const LabelSecondary = styled(LabelPrimary)`
  color: ${p => p.theme.color.secondary};
`;

const LabelNormal = styled(LabelPrimary)`
  color: ${p => p.theme.color.normal};
`;

const LabelLight = styled(LabelPrimary)`
  color: ${p => p.theme.color.light};
`;

const LabelDark = styled(LabelPrimary)`
  color: ${p => p.theme.color.dark};
`;

export { LabelPrimary, LabelSecondary, LabelNormal, LabelLight, LabelDark };
