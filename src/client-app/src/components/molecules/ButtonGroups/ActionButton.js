import React from "react";
import styled from "styled-components";
import { ButtonTransparent } from "../../atoms/Buttons/Buttons";

const ButtonGroups = styled.div`
  position: relative;
`;

const Button = styled(ButtonTransparent)`
  padding: 0;
  text-align: center;
  width: ${(p) => p.theme.size.small};
  height: ${(p) => p.theme.size.small};
`;

const ActionButton = (props) => {
  const { onClick } = props;
  return (
    <ButtonGroups className={props.className}>
      {onClick ? <Button onClick={onClick}>{props.children}</Button> : null}
    </ButtonGroups>
  );
};

export default ActionButton;
