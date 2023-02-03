import React, { useState, useEffect } from "react";
import styled from "styled-components";
import DateSelector from "../DateSelector";
import { format } from "date-fns";

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

const DateTimePicker = styled(DateSelector)`
  select {
    border: 0;
    border-bottom: 1px dashed ${(p) => p.theme.color.primaryBg};
    cursor: pointer;
  }

  &.success select {
    border-bottom: 2px solid ${(p) => p.theme.color.primaryBg};
  }

  &.fail select {
    border-bottom: 2px solid ${(p) => p.theme.color.dangerBg};
  }
`;

const updateStatus = {
  success: "success",
  fail: "fail",
};

const DateTimeEditable = (props) => {
  const { value, name, disabled } = props;

  const [status, setStatus] = useState("");
  const [currentDate, setCurrentDate] = useState(null);
  let statusTimer = null;

  useEffect(() => {
    return () => clearTimeout(statusTimer);
  }, [statusTimer]);

  useEffect(() => {
    setCurrentDate(value);
  }, [setCurrentDate, value]);

  const onChanged = (e) => {
    const { name, primaryKey } = props;
    const { value: date } = e.target;
    if (date) {
      const dateTime = format(new Date(date), "MM/dd/yyyy");
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
      setCurrentDate(value);
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
  } else if (!props.disabled && !!value) {
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
        {currentDate ? format(new Date(currentDate), "MMMM, dd yyyy") : ""}
      </TextLabel>
    );
  }
};

export default DateTimeEditable;
