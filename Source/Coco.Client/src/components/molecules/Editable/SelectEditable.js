import React, { useState, useRef, Fragment } from "react";
import styled from "styled-components";
import { Selection } from "../../atoms/Selections";

const TextLabel = styled.span`
  display: inline-block;

  &.can-edit {
    border-bottom: 1px dashed ${p => p.theme.color.normal};
    line-height: ${p => p.theme.size.normal};
    height: ${p => p.theme.size.normal};
    cursor: pointer;
    min-width: calc(${p => p.theme.size.large} * 2);
  }

  &.empty {
    color: ${p => p.theme.color.danger};
    font-weight: 400;
  }
`;

const SelectBox = styled(Selection)`
  min-width: calc(${p => p.theme.size.large} * 2);
  cursor: pointer;
  border: 0;
  border-bottom: 1px dashed ${p => p.theme.color.normal};
  border-radius: 0;
`;

function Options(props) {
  const { selections, emptyText } = props;
  return selections ? (
    <Fragment>
      <option value={0}>{emptyText}</option>
      {selections.map(item => (
        <option key={item.id} value={item.id}>
          {item.text}
        </option>
      ))}
    </Fragment>
  ) : null;
}

export default function(props) {
  const { selections, value, name, disabled } = props;
  const [isOpen, setIsOpen] = useState(false);
  const currentRef = useRef();
  let emptyText = "---Select---";
  if (props.emptyText) {
    emptyText = props.emptyText;
  }

  const current = value
    ? selections.find(item => item.id.toString() === value.toString())
    : { id: 0, text: emptyText };

  const [selectedValue, updateSelectedValue] = useState({
    id: value ? value : null,
    text: current ? current.text : null
  });

  function openSelection() {
    setIsOpen(true);
  }

  function closeSelection() {
    setIsOpen(false);
  }

  function onBlur() {
    closeSelection();
  }

  function onChanged(e) {
    const currentValue = selections.find(function(element) {
      return element.id.toString() === e.target.value;
    });

    if (currentValue) {
      updateSelectedValue(currentValue);
    } else {
      updateSelectedValue({ id: 0, text: emptyText });
    }

    if (props.onChanged) {
      props.onChanged(e);
    }
    closeSelection();
  }

  const canSelect = !props.disabled && !!isOpen;
  if (canSelect && !!selectedValue) {
    return (
      <SelectBox
        ref={currentRef}
        name={name}
        disabled={disabled}
        placeholder={emptyText}
        value={selectedValue.id}
        onBlur={onBlur}
        autoFocus={true}
        onChange={onChanged}
      >
        <Options selections={selections} emptyText={emptyText} />
      </SelectBox>
    );
  } else if (canSelect && value) {
    return (
      <SelectBox
        ref={currentRef}
        name={name}
        disabled={disabled}
        placeholder={emptyText}
        onBlur={onBlur}
        autoFocus={true}
        onChange={props.onChanged}
        value={value}
      >
        <Options selections={selections} emptyText={emptyText} />
      </SelectBox>
    );
  } else if (canSelect) {
    return (
      <SelectBox
        ref={currentRef}
        name={name}
        disabled={disabled}
        placeholder={emptyText}
        onBlur={onBlur}
        autoFocus={true}
        onChange={props.onChanged}
      >
        <Options selections={selections} emptyText={emptyText} />
      </SelectBox>
    );
  }

  if (!selectedValue && !props.disabled) {
    return (
      <TextLabel className="can-edit empty" onClick={openSelection}>
        {selectedValue ? selectedValue.text : emptyText}
      </TextLabel>
    );
  }

  return !props.disabled ? (
    <TextLabel className="can-edit" onClick={openSelection}>
      {selectedValue ? selectedValue.text : emptyText}
    </TextLabel>
  ) : (
    <TextLabel className="disabled">
      {selectedValue ? selectedValue.text : emptyText}
    </TextLabel>
  );
}
