import React from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import styled from "styled-components";

const Wrap = styled.span`
  display: block;
  padding: 0;
  text-align: center;
  background-color: ${(p) => p.theme.color.lightBg};
  border-radius: ${(p) => p.theme.borderRadius.normal};
  font-size: ${(p) => p.theme.fontSize.giant};
  color: ${(p) => p.theme.color.neutralText};
  position: relative;

  svg {
    position: absolute;
    top: 0;
    bottom: 0;
    margin: auto;
    left: 0;
    right: 0;
  }

  svg,
  path {
    color: inherit;
  }
`;

function NoImage(props) {
  return (
    <Wrap className={props.className}>
      <FontAwesomeIcon icon="image" />
    </Wrap>
  );
}

export default NoImage;
