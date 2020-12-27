import styled from "styled-components";

const PanelBody = styled.div`
  padding: ${(p) => p.theme.size.distance};
`;

const PanelHeading = styled.div`
  padding: calc(${(p) => p.theme.size.distance} - 5px)
    ${(p) => p.theme.size.distance};
`;

const PanelFooter = styled.div`
  padding: calc(${(p) => p.theme.size.distance} - 5px)
    ${(p) => p.theme.size.distance};
`;

const PanelDefault = styled.div`
  box-shadow: ${(p) => p.theme.shadow.BoxShadow};
  background-color: ${(p) => p.theme.color.whiteBg};
  border-radius: ${(p) => p.theme.borderRadius.normal};
`;

const PageColumnPanel = styled.div`
  margin: 0 0 ${(p) => p.theme.size.small} 0;
`;

export { PanelDefault, PanelBody, PanelHeading, PageColumnPanel, PanelFooter };
