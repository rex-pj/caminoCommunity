import React, { useState, useEffect } from "react";
import styled from "styled-components";
import DateSelector from "../DateSelector";
import { format } from "date-fns";

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

const DateTimePicker = styled(DateSelector)`
  select {
    border: 0;
    border-bottom: 1px dashed ${(p) => p.theme.color.primaryLight};
    cursor: pointer;
  }

  &.success select {
    border-bottom: 2px solid ${(p) => p.theme.color.primaryLight};
  }

  &.fail select {
    border-bottom: 2px solid ${(p) => p.theme.color.dangerLight};
  }
`;

const updateStatus = {
  success: "success",
  fail: "fail",
};

export default (props) => {
  const { value, name, disabled } = props;
  const [status, setStatus] = useState("");
  const [currentDate, updateCurrentDate] = useState(value);
  let statusTimer = null;

  useEffect(() => {
    return () => clearTimeout(statusTimer);
  }, [statusTimer]);

  const onChanged = (e) => {
    const { name, primaryKey } = props;
    const { value: date } = e.target;
    if (date) {
      const dateTime = format(date, "MM/DD/YYYY");
      if (props.onUpdated) {
        props
          .onUpdated({
            primaryKey,
            value: dateTime,
            propertyName: name,
          })
          .then((response) => {
            showSuccess();
          })
          .catch((errors) => {
            showError();
          });
      }
    } else {
      updateCurrentDate(value);
    }
  };

  const showError = () => {
    setStatus(updateStatus.fail);
    statusTimer = setTimeout(() => {
      setStatus("");
    }, 1000);
  };

  const showSuccess = () => {
    setStatus(updateStatus.success);
    statusTimer = setTimeout(() => {
      setStatus("");
    }, 1000);
  };

  if (!props.disabled && !!currentDate) {
    return (
      <DateTimePicker
        className={`${status}`}
        name={name}
        disabled={disabled}
        value={currentDate}
        onDateChanged={(e) => onChanged(e)}
      />
    );
  } else if (!props.disabled && value) {
    return (
      <DateTimePicker
        className={`${status}`}
        name={name}
        disabled={disabled}
        onDateChanged={(e) => onChanged(e)}
        value={value}
      />
    );
  } else if (!props.disabled) {
    return (
      <DateTimePicker
        className={`${status}`}
        name={name}
        disabled={disabled}
        onDateChanged={(e) => onChanged(e)}
      />
    );
  } else {
    return (
      <TextLabel className={`disabled`}>
        {currentDate ? format(currentDate, "MMMM, DD YYYY") : ""}
      </TextLabel>
    );
  }
};
