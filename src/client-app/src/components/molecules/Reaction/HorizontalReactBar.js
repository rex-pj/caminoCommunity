import React from "react";
import styled from "styled-components";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { VirtualAnchorNeutral } from "../../atoms/Anchors";

const ReactButton = styled.span`
  display: inline-block;
  font-size: ${(p) => p.theme.fontSize.large};
  line-height: 1;
  vertical-align: middle;
  cursor: pointer;
  margin-right: ${(p) => p.theme.size.exSmall};
  color: ${(p) => p.theme.color.neutralText};

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
    <div className={props.className}>
      <ReactButton className="smile">
        <FontAwesomeIcon icon="smile-beam" />
      </ReactButton>
      <VirtualAnchorNeutral>{reactionNumber}</VirtualAnchorNeutral>
    </div>
  );
};
