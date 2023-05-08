import * as React from "react";
import { useState, useRef } from "react";
import { Selection } from "../atoms/Selections";
import { getDaysInMonth, getYear, getMonth, getDate } from "date-fns";
import styled from "styled-components";
import { createArray, generateDate } from "../../utils/Helper";
import { useTranslation } from "react-i18next";

const Root = styled.div`
  & > * {
    margin-right: 10px;
  }
`;

interface DateSelectorProps {
  yearFrom?: number;
  yearTo?: number;
  className?: string;
  name?: string;
  value?: string;
  onDateChanged?: (e: any) => void;
  onBlur?: (e: any) => void;
  disabled?: boolean;
}

const DateSelector = (props: DateSelectorProps) => {
  const { t } = useTranslation();
  let { yearFrom, yearTo } = props;
  const { className, name, value } = props;
  yearFrom = yearFrom ? yearFrom : 1900;
  yearTo = yearTo ? yearTo : new Date().getFullYear();
  const years = createArray(yearFrom, yearTo, true);
  const [strDate, setStrDate] = useState<{
    date: string;
    month: string;
    year: string;
  }>();
  const parentRef = useRef<HTMLDivElement>();

  let birthdate = { year: "", month: "", date: "" };
  if (value) {
    const dateTime = new Date(value);
    birthdate = {
      year: getYear(dateTime).toString(),
      month: (getMonth(dateTime) + 1).toString(),
      date: getDate(dateTime).toString(),
    };
  } else if (strDate) {
    birthdate = {
      year: strDate.year.toString(),
      month: strDate.month.toString(),
      date: strDate.date.toString(),
    };
  }

  const onDateChanged = (e: React.ChangeEvent<HTMLSelectElement>) => {
    if (props.onDateChanged) {
      const date: {
        date: string;
        month: string;
        year: string;
      } = {
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

  const handleChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
    const date = {
      ...birthdate,
      [event.target.name]: event.target.value,
    };

    setStrDate(date);
    onDateChanged(event);
  };

  const handleOnBlur = (e: React.FocusEvent<HTMLSelectElement, Element>) => {
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
        <option value="">{t("date_label")}</option>
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
        <option value="">{t("month_label")}</option>
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
        <option value="">{t("year_label")}</option>
        {years.map((year) => (
          <option value={year} key={year}>
            {year}
          </option>
        ))}
      </Selection>
    </Root>
  );
};

export default DateSelector;
