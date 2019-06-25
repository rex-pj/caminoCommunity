import React, { useState, useEffect } from "react";
import styled from "styled-components";

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
  const { selections } = props;
  const [selected, updateSelected] = useState({
    id: "",
    value: ""
  });

  useEffect(function() {
    if (selections) {
      const firstSelection = selections[0];
      updateSelected({
        id: firstSelection.id,
        value: firstSelection.value
      });
    }
  });

  let emptyText = "Empty";
  if (props.emptyText) {
    emptyText = props.emptyText;
  }

  if ((!selected || !selected.value) && !props.disabled) {
    return <TextLabel className="can-edit empty">{emptyText}</TextLabel>;
  }

  return !props.disabled ? (
    <TextLabel className="can-edit">{selected.value}</TextLabel>
  ) : (
    <TextLabel className="disabled">{selected.value}</TextLabel>
  );
}
