import React from "react";
import styled from "styled-components";
import { HorizontalList, ListItem } from "../../atoms/List";

const Item = styled(ListItem)`
  width: ${p => `${p.percent}%`};
`;

function ListScroll(props) {
  const { list, className } = props;
  let { numberOfDisplay } = props;
  numberOfDisplay = numberOfDisplay
    ? numberOfDisplay
    : list
      ? list.length
      : 0;

  const percent = 100 / numberOfDisplay;
  return (
    <HorizontalList className={className}>
      {list
        ? list
          .filter((item, index) => {
            return index < numberOfDisplay;
          })
          .map((item, index) => {
            return (
              <Item key={index} percent={percent}>
                {item}
              </Item>
            );
          })
        : null}
    </HorizontalList>
  );
}


export default ListScroll;
