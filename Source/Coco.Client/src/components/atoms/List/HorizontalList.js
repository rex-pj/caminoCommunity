import React from "react";
import styled from "styled-components";

const HList = styled.ul`
  list-style: none;
  padding-left: 0;
  margin-bottom: 0;

  li {
    display: inline-block;
    vertical-align: middle;
  }
`;

export default props => {
  return <HList className={props.className}>{props.children}</HList>;
};
