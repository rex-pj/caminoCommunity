import * as React from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import styled from "styled-components";
import { IconProp } from "@fortawesome/fontawesome-svg-core";

const BaseButton = styled.span`
  color: ${(p) => p.theme.color.primaryText};
  padding: 5px 8px;
  border-radius: ${(p) => p.theme.borderRadius.normal};
  border-width: 1px;
  border-style: solid;
  border-color: ${(p) => p.theme.color.neutralBg};
  font-size: 0.8rem;
  font-weight: 600;
  box-sizing: border-box;
  outline: none;
  text-align: center;
  display: inline-block;
  cursor: pointer;

  &.actived {
    color: ${(p) => p.theme.color.neutralText};
  }

  :active,
  :hover,
  :focus-within {
    background-color: ${(p) => p.theme.rgbaColor.light};
  }

  :disabled {
    background-color: ${(p) => p.theme.rgbaColor.light};
  }

  svg,
  path,
  span {
    color: inherit;
    font-size: inherit;
  }
`;

export interface DefaultButtonProps {
  className?: string;
  style?: string;
  label?: string;
  icon?: IconProp;
  onToggle: (e: DefaultButtonToggleEvent) => void;
  actived?: boolean;
}

export interface DefaultButtonToggleEvent {
  target: { value: string };
  preventDefault: () => void;
}

const DefaultButton: React.FC<DefaultButtonProps> = (props) => {
  let { className } = props;
  const onToggle = (e: React.MouseEvent<HTMLElement>) => {
    const event: DefaultButtonToggleEvent = {
      target: {
        value: props.style,
      },
      preventDefault: e.preventDefault,
    };
    props.onToggle(event);
  };

  if (!className) {
    className = "";
  }

  if (props.actived) {
    className += " actived";
  }

  return (
    <BaseButton className={className} onMouseDown={onToggle}>
      {props.label ? props.label : null}
      {props.icon ? <FontAwesomeIcon icon={props.icon} /> : null}
    </BaseButton>
  );
};

export { DefaultButton };
