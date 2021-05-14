import React, { useState, useEffect } from "react";
import styled from "styled-components";
// import { Selection } from "../../atoms/Selections";
import Select from "react-select";

const TextLabel = styled.span`
  display: inline-block;

  &.can-edit {
    border-bottom: 1px dashed ${(p) => p.theme.color.neutralBg};
    line-height: ${(p) => p.theme.size.normal};
    height: ${(p) => p.theme.size.normal};
    cursor: pointer;
    min-width: calc(${(p) => p.theme.size.large} * 2);
  }

  &.empty {
    color: ${(p) => p.theme.color.primaryDangerText};
    font-weight: 400;
  }

  &.success {
    border: 1px solid ${(p) => p.theme.color.secondaryBg};
  }

  &.fail {
    border: 1px solid ${(p) => p.theme.color.secondaryDangerBg};
  }
`;

const SelectBox = styled(Select)`
  min-width: calc(${(p) => p.theme.size.large} * 2);
  cursor: pointer;
  border: 0;
  border-bottom: 1px dashed ${(p) => p.theme.color.neutralBg};
  border-radius: 0;
  max-width: 100%;

  &.success {
    border: 1px solid ${(p) => p.theme.color.secondaryBg};
  }

  &.fail {
    border: 1px solid ${(p) => p.theme.color.secondaryDangerBg};
  }
`;

const updateStatus = {
  success: "success",
  fail: "fail",
};

export default function (props) {
  const { selections, value, name, disabled, label } = props;
  const [status, setStatus] = useState("");

  let emptyText = "---Select---";
  if (props.emptyText) {
    emptyText = props.emptyText;
  }

  useEffect(() => {
    return () => clearTimeout();
  });

  let current = null;
  if (value && selections && selections.count > 0) {
    current = selections.find(
      (item) => item.value.toString() === value.toString()
    );
  } else if (value && label) {
    current = { value, label };
  } else {
    current = { value: 0, label: "Not selected" };
  }

  const [selectedValue, updateSelectedValue] = useState({
    value: current?.value,
    label: current?.label,
  });

  function onChanged(e) {
    const { name, primaryKey } = props;

    if (selections) {
      const currentValue = selections.find((element) => {
        return element.value.toString() === e.value;
      });
      const { onUpdated } = props;
      if (onUpdated) {
        onUpdated({
          primaryKey,
          value: currentValue.value,
          propertyName: name,
        })
          .then(() => {
            updateSelectedValue(currentValue);
            showSuccess();
            resetStatus();
          })
          .catch(() => {
            if (selections) {
              const oldValue = selections.find((element) => {
                return element.value.toString() === value.toString();
              });

              updateSelectedValue(oldValue);
            } else {
              updateSelectedValue({ value: 0, label: emptyText });
            }

            showError();
            resetStatus();
          });
      }
    } else {
      updateSelectedValue({ value: 0, label: emptyText });
    }
  }

  function resetStatus(){
    setTimeout(() => {
      setStatus("");
    }, 1000);
  }

  function showError() {
    setStatus(updateStatus.fail);
  }

  function showSuccess() {
    setStatus(updateStatus.success);
  }

  if (!props.disabled && !!selectedValue) {
    const { value, label } = selectedValue;
    return (
      <SelectBox
        className={`${status}`}
        name={name}
        disabled={disabled}
        placeholder={emptyText}
        value={{
          value,
          label,
        }}
        onChange={onChanged}
        options={selections}
      />
    );
  }

  if (!props.disabled && value) {
    return (
      <SelectBox
        className={`${status}`}
        name={name}
        disabled={disabled}
        placeholder={emptyText}
        onChange={onChanged}
        value={{
          value,
          label,
        }}
        options={selections}
      />
    );
  }

  if (!props.disabled) {
    return (
      <SelectBox
        className={`${status}`}
        name={name}
        disabled={disabled}
        placeholder={emptyText}
        onChange={onChanged}
        options={selections}
      />
    );
  }

  return (
    <TextLabel className={`disabled ${status}`}>
      {selectedValue ? selectedValue.text : emptyText}
    </TextLabel>
  );
}
