import React, { useState, Fragment } from "react";
import styled from "styled-components";
import { PanelDefault, PanelFooter, PanelHeading } from "../../atoms/Panels";
import { ButtonTransparent } from "../../atoms/Buttons/Buttons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { useStore } from "../../../store/hook-store";

const Root = styled(PanelDefault)`
  top: 45px;
  left: 0;
  right: 0;
  bottom: auto;
  z-index: 100;
  margin: 0 auto;
  position: ${(p) => (p.position === "fixed" ? "fixed" : "absolute")};
  max-width: ${(p) => (p.size === "lg" ? "90%" : "720px")};
`;

const Scroll = styled.div`
  position: relative;
  z-index: 900;

  > ${PanelHeading} {
    border-bottom: 1px solid ${(p) => p.theme.color.secondaryDivide};
    font-weight: 600;
    position: relative;
  }

  ${PanelFooter} {
    border-top: 1px solid ${(p) => p.theme.color.secondaryDivide};
    text-align: right;

    button {
      margin-left: ${(p) => p.theme.size.tiny};
      border-radius: ${(p) => p.theme.borderRadius.normal};
    }
  }
`;

const CloseButton = styled(ButtonTransparent)`
  position: absolute;
  right: 0;
  top: 0;
  bottom: 0;
  margin: auto;
`;

const Backdrop = styled.div`
  position: fixed;
  top: 0;
  bottom: 0;
  left: 0;
  right: 0;
  background-color: ${(p) => p.theme.rgbaColor.darker};
  z-index: 99;
`;

export default ({ ...props }) => {
  const [showBackdrop] = useState(true);
  const [isDisabled, setDisabled] = useState(true);
  const { className } = props;
  const [state, dispatch] = useStore(true);
  const { data, execution, options } = state;
  const { isOpen, position } = options;

  function closeModal() {
    dispatch("CLOSE_MODAL");
  }

  if (!isOpen) {
    return null;
  }

  const { children, title } = data;
  const { unableClose, innerModal: InnerModal } = options;
  const renderBackdrop = () => {
    if (!!showBackdrop && !unableClose) {
      return <Backdrop onClick={closeModal} />;
    } else if (!!showBackdrop) {
      return <Backdrop />;
    }
  };

  return (
    <Fragment>
      {renderBackdrop()}
      <Root className={className} position={position}>
        <Scroll>
          <PanelHeading>
            {title}
            {!unableClose ? (
              <CloseButton onClick={closeModal}>
                <FontAwesomeIcon icon="times" />
              </CloseButton>
            ) : null}
          </PanelHeading>
          <InnerModal
            data={data}
            execution={execution}
            closeModal={closeModal}
            isDisabled={isDisabled}
            setDisabled={setDisabled}
          >
            {children}
          </InnerModal>
        </Scroll>
      </Root>
    </Fragment>
  );
};
