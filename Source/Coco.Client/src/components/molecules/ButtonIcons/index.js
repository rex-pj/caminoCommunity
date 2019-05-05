import React from "react";
import styled from "styled-components";
import {
  ButtonOutlineSecondary,
  ButtonOutlinePrimary,
  ButtonOutlineNormal
} from "../../atoms/Buttons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

const Text = styled.span`
  margin-left: ${p => p.theme.size.exTiny};
  color: inherit;
`;

function ButtonIconOutline(props) {
  const { icon, children, className, size } = props;
  return (
    <ButtonOutlinePrimary className={className} size={size}>
      <FontAwesomeIcon icon={icon} />
      <Text>{children}</Text>
    </ButtonOutlinePrimary>
  );
}

function ButtonIconOutlineSecondary(props) {
  const { icon, children, className, size } = props;
  return (
    <ButtonOutlineSecondary className={className} size={size}>
      <FontAwesomeIcon icon={icon} />
      <Text>{children}</Text>
    </ButtonOutlineSecondary>
  );
}

function ButtonIconOutlineNormal(props) {
  const { icon, children, className, size } = props;
  return (
    <ButtonOutlineNormal className={className} size={size}>
      <FontAwesomeIcon icon={icon} />
      <Text>{children}</Text>
    </ButtonOutlineNormal>
  );
}

export { ButtonIconOutlineSecondary, ButtonIconOutlineNormal, ButtonIconOutline };
