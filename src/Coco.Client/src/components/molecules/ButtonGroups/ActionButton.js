import React from "react";
import styled from "styled-components";
import { ButtonTransparent } from "../../atoms/Buttons/Buttons";

const ButtonGroups = styled.div`
  position: relative;
`;

const Button = styled(ButtonTransparent)`
  padding: 0;
  text-align: center;
  width: ${p => p.theme.size.small};
  height: ${p => p.theme.size.small};
`;

export default props => {
  return (
    <ButtonGroups className={props.className}>
      <Button>{props.children}</Button>
    </ButtonGroups>
  );
};
