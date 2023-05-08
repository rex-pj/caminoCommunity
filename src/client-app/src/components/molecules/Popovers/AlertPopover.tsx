import * as React from "react";
import { HTMLAttributes, useEffect, useState } from "react";
import styled from "styled-components";
import { PanelDefault, PanelBody, PanelHeading } from "../../molecules/Panels";
import {
  ButtonOutlineLight,
  ButtonOutlineDanger,
} from "../../atoms/Buttons/OutlineButtons";

interface WrapProps extends HTMLAttributes<HTMLDivElement> {
  top?: number | string;
  left?: number | string;
  bottom?: number | string;
  right?: number | string;
}

const Wrap = styled(PanelDefault)<WrapProps>`
  position: absolute;
  bottom: calc(100% + ${(p) => p.theme.size.tiny});
  left: ${(p) => (p.left ? p.left + "px" : "auto")};
  top: ${(p) => (p.top ? p.top + "px" : "auto")};
  right: ${(p) => (p.right ? p.right + "px" : "auto")};
  background-color: ${(p) => p.theme.color.warnBg};

  > ${PanelHeading} {
    border-bottom: 1px solid ${(p) => p.theme.rgbaColor.darkLight};
    color: ${(p) => p.theme.color.neutralText};
  }

  ::after {
    content: " ";
    display: block;
    position: absolute;
    left: ${(p) => p.theme.size.distance};
    top: 100%;
    width: 0;
    height: 0;
    border-left: ${(p) => p.theme.size.tiny} solid transparent;
    border-right: ${(p) => p.theme.size.tiny} solid transparent;
    border-top: ${(p) => p.theme.size.tiny} solid
      ${(p) => p.theme.color.warnText};
  }
`;

interface AlertPopoverProps {
  target: any;
  onClose: () => void | undefined;
  onOpen: () => void | undefined;
  onExecute: () => void | undefined;
  isShown: boolean;
  title?: string;
  className?: string;
}

const AlertPopover: React.FC<AlertPopoverProps> = (props) => {
  const [alertState, setAlertState] = useState<{
    isShown: boolean;
    left?: number | string;
    top?: number | string;
  }>({
    isShown: false,
    left: null,
    top: null,
  });

  useEffect(() => {
    document.addEventListener("click", handleClickTarget, false);
    return () => {
      document.removeEventListener("click", handleClickTarget);
    };
  });

  const handleClickTarget = (event: { target: any }) => {
    const { target } = props;

    const currentTarget = document.getElementById(target);
    if (currentTarget && currentTarget.contains(event.target)) {
      const { isShown } = alertState;
      setAlertState({
        isShown: !isShown,
        left: currentTarget.offsetLeft,
      });

      if (!isShown && props.onClose) {
        props.onClose();
      }

      if (isShown && props.onOpen) {
        props.onOpen();
      }
    }
  };

  const onClose = () => {
    setAlertState({
      isShown: false,
    });
    if (props.onClose) {
      props.onClose();
    }
  };

  const onExecute = () => {
    if (props.onExecute) {
      props.onExecute();
    }
  };

  const { isShown, left } = alertState;
  if (!isShown) {
    return null;
  }

  const { title, className } = props;
  return (
    <Wrap left={left} bottom="100%" className={className}>
      <PanelHeading>{title}</PanelHeading>
      <PanelBody>
        <ButtonOutlineLight size="sm" onClick={onClose}>
          Không
        </ButtonOutlineLight>
        <ButtonOutlineDanger size="sm" onClick={onExecute}>
          Đồng Ý
        </ButtonOutlineDanger>
      </PanelBody>
    </Wrap>
  );
};

export default AlertPopover;
