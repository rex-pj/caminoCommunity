import React, { useState } from "react";
import styled from "styled-components";
import { Textbox } from "../../atoms/Textboxes";

const TextLabel = styled.span`
  &.can-edit {
    border-bottom: 1px dashed ${p => p.theme.color.normal};
  }

  &.empty {
    color: ${p => p.theme.color.danger};
    font-weight: 400;
  }
`;

const TextEditing = styled(Textbox)`
  border: 0;
  border-bottom: 1px dashed ${p => p.theme.color.normal};
  border-radius: 0;
`;

export default function(props) {
  const [isOpenTextBox, setTextBoxOpen] = useState(false);
  const [value, setValue] = useState(props.value);
  function openTextBox() {
    setTextBoxOpen(true);
  }

  function closeTextBox() {
    setTextBoxOpen(false);
  }

  async function onDataUp(e) {
    const currentValue = props.value ? props.value : "";

    if (e.keyCode === 13 && props.onUpdated) {
      if (
        !!props.primaryKey &&
        !!props.name &&
        e.target.value !== currentValue
      ) {
        await props
          .onUpdated({
            primaryKey: props.primaryKey,
            value: e.target.value,
            propertyName: props.name
          })
          .then(function(response) {
            const { data } = response;
            const { updateUserInfoItem } = data;
            const { result } = updateUserInfoItem;
            setValue(result.value);
          })
          .catch(function(errors) {
            console.log(errors);
          });
      }

      closeTextBox();
    } else if (e.keyCode === 13) {
      closeTextBox();
      setValue(value);
    }
  }

  function onBlur() {
    closeTextBox();
    setValue(value);
  }

  function onChanged(e) {
    setValue(e.target.value);
  }

  let emptyText = "Empty";
  if (props.emptyText) {
    emptyText = props.emptyText;
  }

  if (!props.disabled && !!isOpenTextBox && !!value) {
    return (
      <TextEditing
        value={value}
        onBlur={onBlur}
        autoFocus={true}
        onKeyUp={onDataUp}
        onChange={onChanged}
      />
    );
  } else if (!props.disabled && !!isOpenTextBox) {
    return (
      <TextEditing
        onBlur={onBlur}
        autoFocus={true}
        onKeyUp={onDataUp}
        onChange={props.onChanged}
      />
    );
  }

  if (!value && !props.disabled) {
    return (
      <TextLabel className="can-edit empty" onClick={openTextBox}>
        {emptyText}
      </TextLabel>
    );
  }

  return !props.disabled ? (
    <TextLabel className="can-edit" onClick={openTextBox}>
      {value}
    </TextLabel>
  ) : (
    <TextLabel className="disabled">{value}</TextLabel>
  );
}
