import React from "react";
import styled from "styled-components";

const VList = styled.ul`
  list-style: none;
  padding-left: 0;
  margin-bottom: 0;

  li {
    display: block;
  }
`;

const VerticalList = (props) => {
  return <VList className={props.className}>{props.children}</VList>;
};

export default VerticalList;
