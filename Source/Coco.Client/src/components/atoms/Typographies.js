import styled from "styled-components";
const TypographySecondary = styled.p`
  color: ${p => p.theme.color.neutral};
  font-size: ${p => p.theme.rgbaColor.small};

  a {
    color: ${p => p.theme.color.neutral};
  }
`;

const TypographyPrimary = styled.p`
  color: ${p => p.theme.color.primary};
  font-size: ${p => p.theme.rgbaColor.small};
`;

const TypographyDark = styled.p`
  color: ${p => p.theme.color.dark};
  font-size: ${p => p.theme.rgbaColor.small};
`;

export { TypographySecondary, TypographyPrimary, TypographyDark };
