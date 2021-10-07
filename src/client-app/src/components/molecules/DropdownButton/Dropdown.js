import React from "react";
import VerticalList from "../../molecules/List/VerticalList";

export default (props) => {
  return (
    <VerticalList className={props.className}>{props.children}</VerticalList>
  );
};
