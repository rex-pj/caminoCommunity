import React, { useState } from "react";
import styled from "styled-components";
import { Textbox } from "../../atoms/Textboxes";

const TextLabel = styled.span`
  display: inline-block;

  &.can-edit {
    border-bottom: 1px dashed ${p => p.theme.color.neutral};
    line-height: ${p => p.theme.size.normal};
    height: ${p => p.theme.size.normal};
  }

  &.empty {
    color: ${p => p.theme.color.danger};
    font-weight: 400;
  }
`;

const TextEditing = styled(Textbox)`
  border: 0;
  border-bottom: 1px dashed ${p => p.theme.color.neutral};
  border-radius: 0;
`;

export default function(props) {
  const [isOpen, setIsOpen] = useState(false);
  const [value, setValue] = useState(props.value ? props.value : "");

  function openTextBox() {
    setIsOpen(true);
  }

  function closeTextBox() {
    setIsOpen(false);
  }

  async function onDataUp(e) {
    const currentValue = props.value ? props.value : "";

    if (e.keyCode === 13 && props.onUpdated) {
      if (
        !!props.primaryKey &&
        !!props.name &&
        currentValue !== e.target.value
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
            setValue(currentValue);
            console.log(errors);
          });
      }

      closeTextBox();
    } else if (e.keyCode === 13) {
      closeTextBox();
      setValue(value);
    } else if (e.keyCode === 27) {
      closeTextBox();
      setValue(currentValue);
    }
  }

  function onBlur() {
    closeTextBox();
    setValue(value);
  }

  function onChange(e) {
    setValue(e.target.value);
  }

  let emptyText = "Empty";
  if (props.emptyText) {
    emptyText = props.emptyText;
  }

  if (!props.disabled && !!isOpen) {
    return (
      <TextEditing
        name={props.name}
        value={value ? value : ""}
        onBlur={onBlur}
        autoFocus={true}
        onKeyUp={onDataUp}
        onChange={onChange}
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