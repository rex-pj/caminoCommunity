import React from "react";
import styled from "styled-components";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { VirtualAnchorSecondary } from "../../atoms/Anchors";

const ReactButton = styled.span`
  display: inline-block;
  font-size: ${p => p.theme.fontSize.large};
  line-height: 1;
  vertical-align: middle;
  cursor: pointer;
  margin-right: ${p => p.theme.size.exSmall};
  color: ${p => p.theme.color.normal};

  svg,
  path {
    color: inherit;
  }

  &.smile:hover,
  &.smile.actived {
    color: ${p => p.theme.color.warning};
  }

  &.confused:hover,
  &.confused.actived {
    color: ${p => p.theme.color.purple};
  }
`;

export default function (props) {
  const { reactionNumber } = props;
  return (
    <div className={props.className}>
      <ReactButton className="smile">
        <FontAwesomeIcon icon="smile-beam" />
      </ReactButton>
      <ReactButton className="confused">
        <FontAwesomeIcon icon="frown" />
      </ReactButton>
      <VirtualAnchorSecondary>{reactionNumber}</VirtualAnchorSecondary>
    </div>
  );
};
