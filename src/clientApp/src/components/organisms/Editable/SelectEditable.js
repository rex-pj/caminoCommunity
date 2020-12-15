import React, { useState, Fragment, useEffect } from "react";
import styled from "styled-components";
import { Selection } from "../../atoms/Selections";

const TextLabel = styled.span`
  display: inline-block;

  &.can-edit {
    border-bottom: 1px dashed ${(p) => p.theme.color.neutral};
    line-height: ${(p) => p.theme.size.normal};
    height: ${(p) => p.theme.size.normal};
    cursor: pointer;
    min-width: calc(${(p) => p.theme.size.large} * 2);
  }

  &.empty {
    color: ${(p) => p.theme.color.danger};
    font-weight: 400;
  }

  &.success {
    border: 1px solid ${(p) => p.theme.color.primaryLight};
  }

  &.fail {
    border: 1px solid ${(p) => p.theme.color.dangerLight};
  }
`;

const SelectBox = styled(Selection)`
  min-width: calc(${(p) => p.theme.size.large} * 2);
  cursor: pointer;
  border: 0;
  border-bottom: 1px dashed ${(p) => p.theme.color.neutral};
  border-radius: 0;
  max-width: 100%;

  &.success {
    border: 1px solid ${(p) => p.theme.color.primaryLight};
  }

  &.fail {
    border: 1px solid ${(p) => p.theme.color.dangerLight};
  }
`;

const updateStatus = {
  success: "success",
  fail: "fail",
};

function Options(props) {
  const { selections, emptyText } = props;
  return selections ? (
    <Fragment>
      <option value={0} disabled={true}>
        {emptyText}
      </option>
      {selections.map((item) => (
        <option key={item.id} value={item.id}>
          {item.text}
        </option>
      ))}
    </Fragment>
  ) : null;
}

export default function (props) {
  const { selections, value, name, disabled, text } = props;
  const [status, setStatus] = useState("");
  let statusTimer = null;

  let emptyText = "---Select---";
  if (props.emptyText) {
    emptyText = props.emptyText;
  }

  useEffect(() => {
    return () => clearTimeout(statusTimer);
  });

  let current = null;
  if (value && selections && selections.count > 0) {
    current = selections.find(
      (item) => item.id.toString() === value.toString()
    );
  } else if (value && text) {
    current = { id: value, text: text };
  } else {
    current = { id: 0, text: "Not selected" };
  }

  const [selectedValue, updateSelectedValue] = useState({
    id: current ? current.id : null,
    text: current ? current.text : null,
  });

  function onChanged(e) {
    const { name, primaryKey } = props;
    const currentValue = selections
      ? selections.find((element) => {
          return element.id.toString() === e.target.value;
        })
      : { id: 0, text: emptyText };

    if (currentValue) {
      updateSelectedValue(currentValue);
      if (props.onUpdated) {
        props
          .onUpdated({
            primaryKey,
            value: currentValue.id,
            propertyName: name,
          })
          .then((response) => {
            showSuccess();
          })
          .catch((errors) => {
            const oldValue = selections
              ? selections.find((element) => {
                  return element.id.toString() === value.toString();
                })
              : { id: 0, text: emptyText };

            updateSelectedValue({
              id: value,
              text: oldValue.text,
            });

            showError();
          });
      }
    } else {
      updateSelectedValue({ id: 0, text: emptyText });
    }
  }

  function showError() {
    setStatus(updateStatus.fail);
    statusTimer = setTimeout(() => {
      setStatus("");
    }, 1000);
  }

  function showSuccess() {
    setStatus(updateStatus.success);
    statusTimer = setTimeout(() => {
      setStatus("");
    }, 1000);
  }

  if (!props.disabled && !!selectedValue) {
    return (
      <SelectBox
        className={`${status}`}
        name={name}
        disabled={disabled}
        placeholder={emptyText}
        value={selectedValue.id}
        onChange={onChanged}
      >
        <Options selections={selections} emptyText={emptyText} />
      </SelectBox>
    );
  } else if (!props.disabled && value) {
    return (
      <SelectBox
        className={`${status}`}
        name={name}
        disabled={disabled}
        placeholder={emptyText}
        onChange={props.onChanged}
        value={value}
      >
        <Options selections={selections} emptyText={emptyText} />
      </SelectBox>
    );
  } else if (!props.disabled) {
    return (
      <SelectBox
        className={`${status}`}
        name={name}
        disabled={disabled}
        placeholder={emptyText}
        onChange={props.onChanged}
      >
        <Options selections={selections} emptyText={emptyText} />
      </SelectBox>
    );
  } else {
    return (
      <TextLabel className={`disabled ${status}`}>
        {selectedValue ? selectedValue.text : emptyText}
      </TextLabel>
    );
  }
}
