import React from "react";
import VerticalList from "../../atoms/List/VerticalList";

export default function (props) {
  return (
    <VerticalList className={props.className}>{props.children}</VerticalList>
  );
};
