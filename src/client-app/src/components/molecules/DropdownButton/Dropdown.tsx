import * as React from "react";
import VerticalList from "../../molecules/List/VerticalList";

interface DropdownProps {
  readonly children: any;
  className?: string;
}

const Dropdown: React.FC<DropdownProps> = (props) => {
  return (
    <VerticalList className={props.className}>{props.children}</VerticalList>
  );
};

export default Dropdown;
