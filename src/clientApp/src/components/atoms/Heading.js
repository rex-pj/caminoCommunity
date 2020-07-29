import styled from "styled-components";

export const SecondaryHeading = styled.h2`
  font-size: ${p => p.theme.fontSize.normal};
  color: ${p => p.theme.color.neutral};
  line-height: 1;

  svg,
  path {
    color: inherit;
  }
`;

export const TertiaryHeading = styled.h3`
  font-size: ${p => p.theme.fontSize.small};
  color: ${p => p.theme.color.neutral};
  line-height: 1;

  svg,
  path {
    color: inherit;
  }
`;

export const TertiaryDarkHeading = styled.h3`
  font-size: ${p => p.theme.fontSize.small};
  color: ${p => p.theme.color.dark};
  line-height: 1;

  svg,
  path {
    color: inherit;
  }
`;

export const QuaternaryHeading = styled.h4`
  font-size: ${p => p.theme.rgbaColor.small};
  color: ${p => p.theme.color.neutral};
  line-height: 1;

  svg,
  path {
    color: inherit;
  }
`;

export const QuaternaryDarkHeading = styled.h4`
  font-size: ${p => p.theme.rgbaColor.small};
  color: ${p => p.theme.color.dark};
  line-height: 1;

  svg,
  path {
    color: inherit;
  }
`;

export const FifthHeading = styled.h5`
  font-size: ${p => p.theme.rgbaColor.small};
  color: ${p => p.theme.color.neutral};
  line-height: 1;

  svg,
  path {
    color: inherit;
  }
`;

export const FifthDarkHeading = styled.h5`
  font-size: ${p => p.theme.rgbaColor.small};
  color: ${p => p.theme.color.dark};
  line-height: 1;

  svg,
  path {
    color: inherit;
  }
`;
