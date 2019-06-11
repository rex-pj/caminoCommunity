import React from "react";
import styled from "styled-components";

const Item = styled.li`
  line-height: 1;
`;

export default function (props) {
  const activeClass = props.isActived ? "actived" : null;
  const firstClass = props.index === 0 ? "first" : null;
  return (
    <Item
      className={`${props.className}${" "}${firstClass}${" "}${activeClass}`}
    >
      {props.children}
    </Item>
  );
};
