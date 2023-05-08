import { HTMLAttributes } from "react";
import styled from "styled-components";

interface ButtonGroupProps extends HTMLAttributes<HTMLDivElement> {
  readonly size?: string;
}

const ButtonGroup = styled.div<ButtonGroupProps>`
  > *:first-child {
    border-top-right-radius: 0;
    border-bottom-right-radius: 0;
  }

  > *:last-child {
    border-top-left-radius: 0;
    border-bottom-left-radius: 0;
  }

  > *:not(:last-child) {
    border-right: 0;
  }
`;

export default ButtonGroup;
