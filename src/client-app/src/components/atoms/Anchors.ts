import styled from "styled-components";

const VirtualAnchorNeutral = styled.span`
  color: inherit;
  font-weight: 600;
  font-size: ${(p) => p.theme.fontSize.small};
  vertical-align: middle;
  cursor: pointer;

  :hover {
    color: ${(p) => p.theme.color.darkText};
  }
`;

export { VirtualAnchorNeutral };
