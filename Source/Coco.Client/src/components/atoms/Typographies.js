import styled from "styled-components";
const TypographySecondary = styled.p`
  color: ${p => p.theme.color.normal};
  font-size: ${p => p.theme.fontSize.exSmall};

  a {
    color: ${p => p.theme.color.normal};
  }
`;

const TypographyPrimary = styled.p`
  color: ${p => p.theme.color.primary};
  font-size: ${p => p.theme.fontSize.exSmall};
`;

const TypographyDark = styled.p`
  color: ${p => p.theme.color.dark};
  font-size: ${p => p.theme.fontSize.exSmall};
`;

export { TypographySecondary, TypographyPrimary, TypographyDark };
