import React, { useEffect, useState } from "react";
import styled from "styled-components";
import { PanelDefault, PanelBody, PanelHeading } from "../../atoms/Panels";
import {
  ButtonOutlineLight,
  ButtonOutlineDanger,
} from "../../atoms/Buttons/OutlineButtons";

const Wrap = styled(PanelDefault)`
  position: absolute;
  bottom: calc(100% + ${(p) => p.theme.size.tiny});
  left: ${(p) => (p.left ? p.left + "px" : "auto")};
  top: ${(p) => (p.top ? p.top + "px" : "auto")};
  right: ${(p) => (p.right ? p.right + "px" : "auto")};
  background-color: ${(p) => p.theme.color.secondaryWarnBg};

  > ${PanelHeading} {
    border-bottom: 1px solid ${(p) => p.theme.rgbaColor.darkLight};
    color: ${(p) => p.theme.color.primaryWarnText};
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
      ${(p) => p.theme.color.secondaryWarnText};
  }
`;

export default function (props) {
  const [alertState, setAlertState] = useState({
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

  const handleClickTarget = (event) => {
    const { target } = props;

    var currentTarget = document.getElementById(target);
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
  const { title, className } = props;
  if (!isShown) {
    return null;
  }

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
}
