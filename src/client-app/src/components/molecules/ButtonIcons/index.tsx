import * as React from "react";
import styled from "styled-components";
import { ButtonPrimary, ButtonSecondary } from "../../atoms/Buttons/Buttons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { IconProp } from "@fortawesome/fontawesome-svg-core";
import { ButtonHTMLAttributes } from "react";

const Text = styled.span`
  margin-left: ${(p) => p.theme.size.exTiny};
  color: inherit;
`;

interface ActionButtonProps extends ButtonHTMLAttributes<HTMLButtonElement> {
  readonly icon?: IconProp;
  readonly children: any;
  className?: string;
  size?: string;
}

const ButtonIconPrimary = (props: ActionButtonProps) => {
  const { icon, children, className, size } = props;
  return (
    <ButtonPrimary className={className} size={size}>
      {icon ? <FontAwesomeIcon icon={icon} /> : null}
      <Text>{children}</Text>
    </ButtonPrimary>
  );
};

const ButtonIconSecondary = (props: ActionButtonProps) => {
  const { icon, children, className, size } = props;
  return (
    <ButtonSecondary {...props}>
      {icon ? <FontAwesomeIcon icon={icon} /> : null}
      <Text>{children}</Text>
    </ButtonSecondary>
  );
};

export { ButtonIconPrimary, ButtonIconSecondary };
