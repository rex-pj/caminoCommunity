import React from "react";
import styled from "styled-components";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

const ReactBar = styled.div`
  text-align: center;
`;

const ReactButton = styled.span`
  display: block;
  font-size: ${(p) => p.theme.fontSize.large};
  line-height: 1;

  &.smile > svg > path {
    color: ${(p) => p.theme.color.primaryWarnText};
  }
`;

const ReactionNumber = styled.div`
  margin: ${(p) => p.theme.size.small} 0;
  font-weight: 700;
  color: ${(p) => p.theme.color.primaryText};
  font-size: ${(p) => p.theme.fontSize.small};
`;

export default (props) => {
  const { reactionNumber } = props;
  return (
    <ReactBar>
      <ReactButton className="smile">
        <FontAwesomeIcon icon="smile-beam" />
      </ReactButton>
      <ReactionNumber>{reactionNumber}</ReactionNumber>
    </ReactBar>
  );
};
