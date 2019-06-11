import React from "react";
import styled from "styled-components";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

const ReactBar = styled.div`
  text-align: center;
`;

const ReactButton = styled.span`
  display: block;
  font-size: ${p => p.theme.fontSize.exLarge};
  line-height: 1;

  &.smile > svg > path {
    color: ${p => p.theme.color.warning};
  }

  &.confused > svg > path {
    color: ${p => p.theme.color.purple};
  }
`;

const ReactionNumber = styled.div`
  margin: ${p => p.theme.size.small} 0;
  font-weight: 700;
  color: ${p => p.theme.color.primary};
  font-size: ${p => p.theme.fontSize.exSmall};
`;

export default props => {
  const { reactionNumber } = props;
  return (
    <ReactBar>
      <ReactButton className="smile">
        <FontAwesomeIcon icon="smile-beam" />
      </ReactButton>
      <ReactionNumber>{reactionNumber}</ReactionNumber>
      <ReactButton className="confused">
        <FontAwesomeIcon icon="frown" />
      </ReactButton>
    </ReactBar>
  );
};
