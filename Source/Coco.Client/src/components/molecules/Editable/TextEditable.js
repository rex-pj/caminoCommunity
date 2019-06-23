import React, { useState } from "react";
import styled from "styled-components";
import { Textbox } from "../../atoms/Textboxes";

const TextData = styled.span`
  &.can-edit {
    border-bottom: 1px dashed ${p => p.theme.color.normal};
  }

  &.empty {
    color: ${p => p.theme.color.danger};
    font-weight: 400;
  }
`;

// <Textbox value={props.value} />
export default function(props) {
  const [isOpenTextBox, setTextBoxOpen] = useState(false);
  function openTextBox() {
    setTextBoxOpen(true);
  }

  function closeTextBox() {
    setTextBoxOpen(false);
  }

  function onDataUp(e) {
    if (e.keyCode === 13 && props.onUpdated) {
      if (!!props.primaryKey && !!props.propertyName) {
        props.onUpdated({
          primaryKey: props.primaryKey,
          value: e.target.value,
          propertyName: props.propertyName
        });
      }

      setTextBoxOpen(false);
    } else if (e.keyCode === 13) {
      setTextBoxOpen(false);
    }
  }

  let emptyText = "Empty";
  if (props.emptyText) {
    emptyText = props.emptyText;
  }

  if (!props.disabled && !!isOpenTextBox && !!props.value) {
    return (
      <Textbox
        value={props.value}
        onBlur={closeTextBox}
        autoFocus={true}
        onKeyUp={onDataUp}
      />
    );
  } else if (!props.disabled && !!isOpenTextBox) {
    return (
      <Textbox onBlur={closeTextBox} autoFocus={true} onKeyUp={onDataUp} />
    );
  }

  if (!props.value && !props.disabled) {
    return (
      <TextData className="can-edit empty" onClick={openTextBox}>
        {emptyText}
      </TextData>
    );
  }

  return !props.disabled ? (
    <TextData className="can-edit" onClick={openTextBox}>
      {props.value}
    </TextData>
  ) : (
    <TextData className="disabled">{props.value}</TextData>
  );
}
