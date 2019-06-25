import React, { useState } from "react";
import styled from "styled-components";
import { Textbox } from "../../atoms/Textboxes";

const TextLabel = styled.span`
  display: inline-block;

  &.can-edit {
    border-bottom: 1px dashed ${p => p.theme.color.normal};
    line-height: ${p => p.theme.size.normal};
    height: ${p => p.theme.size.normal};
  }

  &.empty {
    color: ${p => p.theme.color.danger};
    font-weight: 400;
  }
`;

export default function(props) {
  return !props.disabled ? (
    <TextLabel className="can-edit" />
  ) : (
    <TextLabel className="disabled" />
  );
}
