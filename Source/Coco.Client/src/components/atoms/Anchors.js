import styled from "styled-components";

const VirtualAnchorSecondary = styled.span`
  color: ${p => p.theme.color.normal};
  font-weight: 600;
  font-size: ${p => p.theme.fontSize.small};
  vertical-align: middle;
  cursor: pointer;

  :hover {
    color: ${p => p.theme.color.secondary};
  }
`;

const LinkAnchorSecondary = styled.a`
  color: ${p => p.theme.color.normal};
  font-weight: 600;
  font-size: ${p => p.theme.fontSize.small};
  vertical-align: middle;
  cursor: pointer;

  :hover {
    color: ${p => p.theme.color.secondary};
  }
`;

export { VirtualAnchorSecondary, LinkAnchorSecondary };
