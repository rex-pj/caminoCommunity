import React, { Fragment } from "react";
import styled from "styled-components";
import { PanelBody, PanelFooter } from "../../atoms/Panels";
import { AnchorButtonPrimary } from "../../atoms/Buttons/AnchorButtons";

const AcceptButton = styled(AnchorButtonPrimary)`
  border-radius: ${(p) => p.theme.borderRadius.normal};
`;

export default function (props) {
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
