import React from "react";
import styled from "styled-components";
import { PanelDefault, PanelBody } from "../Panels";

const Root = styled(PanelDefault)`
  position: absolute;
  top: 50%;
  bottom: 50%;
  left: 0;
  right: 0;
  margin: auto;
`;

export default function(props) {
  const { isOpen } = props;
  return !!isOpen ? (
    <Root className={props.className}>
      <PanelBody>{props.children}</PanelBody>
    </Root>
  ) : null;
}
