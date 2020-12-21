import styled from "styled-components";

const PrimaryTitle = styled.h1`
  font-size: ${(p) => p.theme.fontSize.medium};
  color: ${(p) => p.theme.color.neutralText};
  font-weight: 700;

  svg,
  path {
    color: inherit;
  }
`;

const secondaryTitle = styled.h2`
  font-size: ${(p) => p.theme.fontSize.normal};
  color: ${(p) => p.theme.color.neutralText};
  font-weight: 700;

  svg,
  path {
    color: inherit;
  }
`;

const TertiaryTitle = styled.h3`
  font-size: ${(p) => p.theme.fontSize.small};
  color: ${(p) => p.theme.color.neutralText};
  font-weight: 700;

  svg,
  path {
    color: inherit;
  }
`;

const QuaternaryTitle = styled.h4`
  font-size: ${(p) => p.theme.fontSize.small};
  color: ${(p) => p.theme.color.neutralText};
  font-weight: 700;

  svg,
  path {
    color: inherit;
  }
`;

export { PrimaryTitle, secondaryTitle, TertiaryTitle, QuaternaryTitle };
