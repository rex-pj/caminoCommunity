import React from "react";
import styled from "styled-components";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { VirtualAnchorNeutral } from "../../atoms/Anchors";

const Root = styled.div`
  display: inline-block;
`;

const ReactButton = styled.span`
  display: inline-block;
  font-size: ${(p) => p.theme.fontSize.small};
  line-height: 1;
  vertical-align: middle;
  cursor: pointer;
  margin-right: ${(p) => p.theme.size.exTiny};
  color: ${(p) => p.theme.color.lightText};

  svg,
  path {
    color: inherit;
  }

  &.smile:hover,
  &.smile.actived {
    color: ${(p) => p.theme.color.primaryDangerText};
  }
`;

export default (props) => {
  const { reactionNumber } = props;
  return (
    <Root className={props.className}>
      <ReactButton className="smile">
        <FontAwesomeIcon icon="smile-beam" />
      </ReactButton>
      <VirtualAnchorNeutral>{reactionNumber}</VirtualAnchorNeutral>
    </Root>
  );
};
