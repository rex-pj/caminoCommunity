import React from "react";
import styled from "styled-components";

const Item = styled.li`
  line-height: 1;
`;

const ListItem = (props) => {
  const activeClass = props.isActived ? "actived" : null;
  const firstClass = props.index === 0 ? "first" : null;
  let className = props.className;
  if (firstClass) {
    className = `${className}${" "}${firstClass}`;
  }
  if (activeClass) {
    className = `${className}${" "}${activeClass}`;
  }
  return <Item className={className}>{props.children}</Item>;
};

export default ListItem;
