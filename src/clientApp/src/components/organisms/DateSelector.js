import React, { useState, useRef } from "react";
import { Selection } from "../atoms/Selections";
import { getDaysInMonth, getYear, getMonth, getDate } from "date-fns";
import styled from "styled-components";
import { createArray, generateDate } from "../../utils/Helper";

const Root = styled.div`
  & > * {
    margin-right: 10px;
  }
`;

export default (props) => {
  let { yearFrom, yearTo } = props;
  const { className, name, value } = props;
  yearFrom = yearFrom ? yearFrom : 1900;
  yearTo = yearTo ? yearTo : new Date().getFullYear();
  const years = createArray(yearFrom, yearTo, true);
  const [strDate, setStrDate] = useState();
  const parentRef = useRef();

  let birthdate = { year: "", month: "", date: "" };
  if (value) {
    birthdate = {
      year: getYear(value),
      month: getMonth(value) + 1,
      date: getDate(value),
    };
  } else if (strDate) {
    birthdate = {
      year: strDate.year,
      month: strDate.month,
      date: strDate.date,
    };
  }

  const onDateChanged = (e) => {
    if (props.onDateChanged) {
      const date = {
        ...birthdate,
        [e.target.name]: e.target.value,
      };
      const dateFormatted = generateDate(date);
      props.onDateChanged({
        ...e,
        strDate: date,
        target: {
          ...e.target,
          name,
          value: dateFormatted,
          classList: parentRef.current.classList,
        },
      });
    }
  };

  const handleChange = (event) => {
    const date = {
      ...birthdate,
      [event.target.name]: event.target.value,
    };

    setStrDate(date);
    onDateChanged(event);
  };

  const handleOnBlur = (e) => {
    if (props.onBlur) {
      props.onBlur({
        ...e,
        target: {
          ...e.target,
          name,
          classList: parentRef.current.classList,
        },
      });
    }
  };

  const { date, month, year } = birthdate;
  const daysInMonth =
    year && month
      ? getDaysInMonth(new Date(Number(year), Number(month) - 1))
      : 31;

  return (
    <Root className={className} ref={parentRef}>
      <Selection
        value={date}
        onChange={handleChange}
        name="date"
        onBlur={handleOnBlur}
      >
        <option value="">Date</option>
        {createArray(1, daysInMonth).map((day) => (
          <option value={day} key={day}>
            {day}
          </option>
        ))}
      </Selection>
      <Selection
        value={month}
        onChange={handleChange}
        name="month"
        onBlur={handleOnBlur}
      >
        <option value="">Month</option>
        {createArray(1, 12).map((month) => (
          <option value={month} key={month}>
            {month}
          </option>
        ))}
      </Selection>
      <Selection
        value={year}
        onChange={handleChange}
        name="year"
        onBlur={handleOnBlur}
      >
        <option value="">Year</option>
        {years.map((year) => (
          <option value={year} key={year}>
            {year}
          </option>
        ))}
      </Selection>
    </Root>
  );
};
