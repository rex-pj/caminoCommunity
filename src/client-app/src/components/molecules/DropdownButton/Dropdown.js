import React from "react";
import VerticalList from "../../molecules/List/VerticalList";

const Dropdown = (props) => {
  return (
    <VerticalList className={props.className}>{props.children}</VerticalList>
  );
};

export default Dropdown;
