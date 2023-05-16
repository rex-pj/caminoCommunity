import * as React from "react";
import { HTMLAttributes } from "react";
import styled from "styled-components";

const VList = styled.ul`
  list-style: none;
  padding-left: 0;
  margin-bottom: 0;

  li {
    display: block;
  }
`;

const VerticalList: React.FC<HTMLAttributes<HTMLUListElement>> = (props) => {
  return <VList className={props.className}>{props.children}</VList>;
};

export default VerticalList;
