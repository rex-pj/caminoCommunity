import * as React from "react";
import styled from "styled-components";
import { HorizontalList, ListItem } from "../../molecules/List";
import { ListItemProps } from "../../molecules/List/ListItem";
import { HTMLAttributes } from "react";

interface ItemProps extends ListItemProps {
  percent?: number;
}

const Item = styled(ListItem)<ItemProps>`
  width: ${(p) => `${p.percent}%`};
`;

interface ListScrollProps extends HTMLAttributes<HTMLUListElement> {
  list?: () => any[];
  numberOfDisplay?: number;
}

const ListScroll = function (props: ListScrollProps) {
  const { list, className } = props;
  const { numberOfDisplay } = props;
  const displayNumber = !numberOfDisplay && list ? list.length : 0;

  const percent = 100 / displayNumber;
  return (
    <HorizontalList className={className}>
      {list
        ? list()
            .filter((item: any, index: number) => {
              return index < displayNumber;
            })
            .map((item: any, index: number) => {
              return (
                <Item key={index} percent={percent}>
                  {item}
                </Item>
              );
            })
        : null}
    </HorizontalList>
  );
};

export default ListScroll;
