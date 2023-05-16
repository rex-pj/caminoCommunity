import * as React from "react";
import { useState, useEffect } from "react";
import styled from "styled-components";
import Select from "react-select";
import { DefaultTFuncReturn } from "i18next";

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
    color: ${(p) => p.theme.color.warnText};
    font-weight: 400;
  }

  &.success {
    border: 1px solid ${(p) => p.theme.color.primaryBg};
  }

  &.fail {
    border: 1px solid ${(p) => p.theme.color.dangerText};
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
    border: 1px solid ${(p) => p.theme.color.primaryBg};
  }

  &.fail {
    border: 1px solid ${(p) => p.theme.color.dangerBg};
  }
`;

const updateStatus = {
  success: "success",
  fail: "fail",
};

interface SelectEditableProps {
  value?: any;
  name: string;
  primaryKey: string;
  disabled?: boolean;
  label?: string;
  selections: any[];
  emptyText?: string | DefaultTFuncReturn;
  onUpdated?: (e: {
    primaryKey: string;
    value: any;
    propertyName: string;
  }) => Promise<any>;
}

const SelectEditable = (props: SelectEditableProps) => {
  const { selections, value, name, disabled, label } = props;
  const [status, setStatus] = useState("");
  let statusTimer: any = null;

  let emptyText = "---Select---";
  if (props.emptyText) {
    emptyText = props.emptyText;
  }

  useEffect(() => {
    return () => clearTimeout(statusTimer);
  });

  let current: any = null;
  if (value && selections && selections.length > 0) {
    current = selections.find(
      (item) => item.value.toString() === value.toString()
    );
  } else if (value && label) {
    current = { value, label };
  } else {
    current = { value: 0, label: "Not selected" };
  }

  const [selectedValue, updateSelectedValue] = useState<{
    value?: any;
    label?: string;
    text?: string;
  }>({
    value: current?.value,
    label: current?.label,
  });

  function onChanged(e: any) {
    const { name, primaryKey } = props;

    if (selections) {
      const currentValue = selections.find((element) => {
        return element.value.toString() === e.value;
      });

      if (props.onUpdated) {
        props
          .onUpdated({
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

  function resetStatus() {
    statusTimer = setTimeout(() => {
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
        isDisabled={disabled}
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
        isDisabled={disabled}
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
        isDisabled={disabled}
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
};

export default SelectEditable;
