import React from "react";
import styled from "styled-components";
import { ImageRound } from "../../atoms/Images";

const Root = styled.span`
  border-radius: ${p => p.theme.borderRadius.normal};
  text-align: center;
  display: block;
`;

export default function (props) {
  return (
    <Root className={props.className}>
      <ImageRound
        src={props.src}
        alt={props.alt}
        width={props.width}
        height={props.height}
      />
    </Root>
  );
} 
