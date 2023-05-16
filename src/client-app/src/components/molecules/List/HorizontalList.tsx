import * as React from "react";
import { HTMLAttributes } from "react";
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

interface Props extends HTMLAttributes<HTMLUListElement> {
  percent?: number;
}

const HorizontalList = (props: Props) => {
  return <HList className={props.className}>{props.children}</HList>;
};

export default HorizontalList;
