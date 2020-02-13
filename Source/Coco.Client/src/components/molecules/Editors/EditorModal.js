import React, { Fragment, useState, useEffect } from "react";
// import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import styled from "styled-components";

const Root = styled.div`
  position: absolute;
  display: inline-block;
  min-width: 125px;
  z-index: 2;
  left: 0;
  top: 0;
  bottom: 0;
  right: 0;
`;

const Modal = styled.div`
  cursor: pointer;
  font-size: 0.8rem;
  height: auto;
  position: absolute;
  min-width: 100px;
  min-height: 80px;
  border: 1px solid ${p => p.theme.color.light};
  border-radius: ${p => p.theme.borderRadius.normal};
  box-shadow: ${p => p.theme.shadow.BoxShadow};
  list-style: none;
  padding-left: 0;
  overflow: hidden;
`;

export default props => {
  const { className, isOpen } = props;
  // const [isOpen, setOpen] = useState(false);
  const currentRef = React.createRef();

  useEffect(() => {
    document.addEventListener("click", onHide, false);
    return () => {
      document.removeEventListener("click", onHide);
    };
  });

  const onHide = e => {
    if (!currentRef.current.contains(e.target)) {
      onClose();
    }
  };

  const onClose = () => {
    if (props.onClose) {
      props.onClose(false);
    }
  };

  return isOpen ? (
    <Root className={className} ref={currentRef}>
      <Modal></Modal>
      <button onClick={onClose}>Close</button>
    </Root>
  ) : (
    <Fragment></Fragment>
  );
};
