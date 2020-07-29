import React from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import styled from "styled-components";

const BaseButton = styled.span`
  color: ${p => p.theme.color.primaryLight};
  padding: 5px 8px;
  border-radius: ${p => p.theme.borderRadius.normal};
  border-width: 1px;
  border-style: solid;
  border-color: ${p => p.theme.color.neutral};
  font-size: 0.8rem;
  font-weight: 600;
  box-sizing: border-box;
  outline: none;
  text-align: center;
  display: inline-block;
  cursor: pointer;

  &.actived {
    color: ${p => p.theme.color.neutral};
  }

  :active,
  :hover,
  :focus-within {
    background-color: ${p => p.theme.rgbaColor.light};
  }

  :disabled {
    background-color: ${p => p.theme.rgbaColor.light};
  }

  svg,
  path,
  span {
    color: inherit;
    font-size: inherit;
  }
`;

export const DefaultButton = props => {
  let { className } = props;
  const onToggle = e => {
    const event = {
      target: {
        value: props.style
      },
      preventDefault: e.preventDefault
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
