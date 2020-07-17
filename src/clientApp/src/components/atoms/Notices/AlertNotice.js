import styled from "styled-components";

const PrimaryNotice = styled.p`
  font-size: ${p => p.theme.fontSize.small};
  color: ${p => p.theme.color.warningLight};

  svg,
  path {
    color: inherit;
  }
`;

export { PrimaryNotice };
