import React from "react";
import styled from "styled-components";
import { ButtonPrimary } from "../../atoms/Buttons/Buttons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

const Text = styled.span`
  margin-left: ${(p) => p.theme.size.exTiny};
  color: inherit;
`;

function ButtonIconPrimary(props) {
  const { icon, children, className, size } = props;
  return (
    <ButtonPrimary className={className} size={size}>
      <FontAwesomeIcon icon={icon} />
      <Text>{children}</Text>
    </ButtonPrimary>
  );
}

export { ButtonIconPrimary };
