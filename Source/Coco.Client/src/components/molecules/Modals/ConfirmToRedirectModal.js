import React, { Fragment } from "react";
import styled from "styled-components";
import { PanelBody, PanelFooter } from "../../atoms/Panels";
import { AnchorButton } from "../../atoms/AnchorButtons";

const AcceptButton = styled(AnchorButton)`
  border-radius: ${p => p.theme.borderRadius.large};
`;

export default function(props) {
  const { children, data } = props;
  const { executeButtonName, executeUrl } = data;

  return (
    <Fragment>
      {children ? <PanelBody>{children}</PanelBody> : null}
      <PanelFooter>
        <AcceptButton href={executeUrl}>{executeButtonName}</AcceptButton>
      </PanelFooter>
    </Fragment>
  );
}
