import styled from "styled-components";

const VirtualAnchorSecondary = styled.span`
  color: ${p => p.theme.color.neutral};
  font-weight: 600;
  font-size: ${p => p.theme.fontSize.small};
  vertical-align: middle;
  cursor: pointer;

  :hover {
    color: ${p => p.theme.color.primaryLight};
  }
`;

const LinkAnchorSecondary = styled.a`
  color: ${p => p.theme.color.neutral};
  font-weight: 600;
  font-size: ${p => p.theme.fontSize.small};
  vertical-align: middle;
  cursor: pointer;

  :hover {
    color: ${p => p.theme.color.primaryLight};
  }
`;

export { VirtualAnchorSecondary, LinkAnchorSecondary };
