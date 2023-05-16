import * as React from "react";
import styled from "styled-components";
import { ButtonTransparent, ButtonProps } from "../../atoms/Buttons/Buttons";

const ButtonGroups = styled.div`
  position: relative;
`;

const Button = styled(ButtonTransparent)<ButtonProps>`
  padding: 0;
  text-align: center;
  width: ${(p) => p.theme.size.small};
  height: ${(p) => p.theme.size.small};
`;

interface ActionButtonProps {
  readonly className?: string;
  readonly children: any;
  onClick?: React.MouseEventHandler<HTMLButtonElement> | undefined;
}

const ActionButton: React.FC<ActionButtonProps> = (props) => {
  const { onClick } = props;
  return (
    <ButtonGroups className={props.className}>
      {onClick ? <Button onClick={onClick}>{props.children}</Button> : null}
    </ButtonGroups>
  );
};

export default ActionButton;
