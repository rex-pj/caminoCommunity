import React, { Component } from "react";
import { Selection } from "../atoms/Selections";
import { getDaysInMonth, getYear, getMonth, getDate } from "date-fns";
import styled from "styled-components";

const Root = styled.div`
  & > * {
    margin-right: 10px;
  }
`;

export default class extends Component {
  state = {
    year: "",
    month: "",
    date: ""
  };

  constructor(props) {
    super(props);

    let { yearFrom, yearTo } = props;
    yearFrom = yearFrom ? yearFrom : 1900;
    yearTo = yearTo ? yearTo : new Date().getFullYear();
    this.years = this.createArray(yearFrom, yearTo);

    const birthdate = props.value;
    if (birthdate) {
      this.state = {
        year: getYear(birthdate),
        month: getMonth(birthdate) + 1,
        date: getDate(birthdate)
      };
    }
  }

  onDateChanged = () => {
    if (this.props.onDateChanged) {
      this.props.onDateChanged(this.getDate());
    }
  };

  componentWillReceiveProps(nextProps) {
    if (this.props.value !== nextProps.value) {
      const { value } = nextProps;
      this.updateDate(value);
    }
  }

  updateDate = dateTime => {
    this.setState({
      year: getYear(dateTime),
      month: getMonth(dateTime) + 1,
      date: getDate(dateTime)
    });
  };

  createArray(from, to) {
    const arr = [];
    for (let index = from; index <= to; index++) {
      arr.push(index);
    }
    return arr;
  }

  getDate() {
    const { date, month, year } = this.state;

    if (date && month && year) {
      return new Date(Number(year), Number(month) - 1, Number(date));
    }
    return null;
  }

  handleChange = (name, event) => {
    this.setState({ [name]: event.target.value }, this.onDateChanged);
  };

  handleOnBlur = () => {
    this.props.onBlur();
  };

  render() {
    const { date, month, year } = this.state;
    const { className } = this.props;

    const daysInMonth =
      year && month
        ? getDaysInMonth(new Date(Number(year), Number(month) - 1))
        : 31;

    return (
      <Root className={className}>
        <Selection
          value={date}
          onChange={e => this.handleChange("date", e)}
          name="date"
          onBlur={this.handleOnBlur}
        >
          <option value="">Ngày</option>
          {this.createArray(1, daysInMonth).map(day => (
            <option value={day} key={day}>
              {day}
            </option>
          ))}
        </Selection>
        <Selection
          value={month}
          onChange={e => this.handleChange("month", e)}
          name="month"
          onBlur={this.handleOnBlur}
        >
          <option value="">Tháng</option>
          {this.createArray(1, 12).map(month => (
            <option value={month} key={month}>
              {month}
            </option>
          ))}
        </Selection>
        <Selection
          value={year}
          onChange={e => this.handleChange("year", e)}
          name="year"
          onBlur={this.handleOnBlur}
        >
          <option value="">Năm</option>
          {this.years.map(year => (
            <option value={year} key={year}>
              {year}
            </option>
          ))}
        </Selection>
      </Root>
    );
  }
}
