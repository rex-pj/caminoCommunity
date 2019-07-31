import styled from "styled-components";

const SecondaryHeading = styled.h2`
  font-size: ${p => p.theme.fontSize.normal};
  color: ${p => p.theme.color.neutral};
  line-height: 1;

  svg,
  path {
    color: inherit;
  }
`;

const TertiaryHeading = styled.h3`
  font-size: ${p => p.theme.fontSize.small};
  color: ${p => p.theme.color.neutral};
  line-height: 1;

  svg,
  path {
    color: inherit;
  }
`;

const TertiaryDarkHeading = styled.h3`
  font-size: ${p => p.theme.fontSize.small};
  color: ${p => p.theme.color.dark};
  line-height: 1;

  svg,
  path {
    color: inherit;
  }
`;

const QuaternaryHeading = styled.h4`
  font-size: ${p => p.theme.rgbaColor.small};
  color: ${p => p.theme.color.neutral};
  line-height: 1;

  svg,
  path {
    color: inherit;
  }
`;

const QuaternaryDarkHeading = styled.h4`
  font-size: ${p => p.theme.rgbaColor.small};
  color: ${p => p.theme.color.dark};
  line-height: 1;

  svg,
  path {
    color: inherit;
  }
`;

export {
  SecondaryHeading,
  TertiaryHeading,
  TertiaryDarkHeading,
  QuaternaryHeading,
  QuaternaryDarkHeading
};
