import React, { useState } from "react";
import styled from "styled-components";
import DaySelector from "../DaySelector";
import { format } from "date-fns";

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

  &.success {
    border: 1px solid ${p => p.theme.color.secondary};
  }

  &.fail {
    border: 1px solid ${p => p.theme.color.dangerLight};
  }
`;

const DateTimePicker = styled(DaySelector)`
  &.success {
    border: 1px solid ${p => p.theme.color.secondary};
  }

  &.fail {
    border: 1px solid ${p => p.theme.color.dangerLight};
  }
`;

export default function(props) {
  const { value, name, disabled } = props;

  const [currentDate, updateCurrentDate] = useState(value);

  function onChanged(e) {
    const { name, primaryKey } = props;

    if (currentDate) {
      if (props.onUpdated) {
        props
          .onUpdated({
            primaryKey,
            value: currentDate.toString(),
            propertyName: name
          })
          .then(response => {})
          .catch(errors => {});
      }
    } else {
      updateCurrentDate(value);
    }
  }

  function onBlur() {}

  if (!props.disabled && !!currentDate) {
    return (
      <DateTimePicker
        name={name}
        disabled={disabled}
        value={currentDate}
        onBlur={onBlur}
        onDateChanged={date => onChanged(date)}
      />
    );
  } else if (!props.disabled && value) {
    return (
      <DateTimePicker
        name={name}
        disabled={disabled}
        onBlur={onBlur}
        onDateChanged={date => onChanged(date)}
        value={value}
      />
    );
  } else if (!props.disabled) {
    return (
      <DateTimePicker
        name={name}
        disabled={disabled}
        onBlur={onBlur}
        onDateChanged={date => onChanged(date)}
      />
    );
  } else {
    return (
      <TextLabel className={`disabled`}>
        {currentDate ? format(currentDate, "MMMM, DD YYYY") : ""}
      </TextLabel>
    );
  }
}
