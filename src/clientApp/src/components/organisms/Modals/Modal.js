import React, { useState, Fragment } from "react";
import styled from "styled-components";
import { PanelDefault, PanelFooter, PanelHeading } from "../../atoms/Panels";
import DefaultModal from "./DefaultModal";
import ConfirmToRedirectModal from "./ConfirmToRedirectModal";
import UpdateAvatarModal from "./UpdateAvatarModal";
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
  position: absolute;
  max-width: ${(p) => (p.size === "lg" ? "90%" : "720px")};
`;

const Scroll = styled.div`
  position: relative;
  z-index: 900;

  > ${PanelHeading} {
    border-bottom: 1px solid ${(p) => p.theme.color.light};
    font-weight: 600;
    position: relative;
  }

  ${PanelFooter} {
    border-top: 1px solid ${(p) => p.theme.color.light};
    text-align: right;

    button {
      margin-left: ${(p) => p.theme.size.tiny};
      border-radius: ${(p) => p.theme.borderRadius.large};
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
  let [state, dispatch] = useStore(true);
  const { data, options } = state;
  const { isOpen } = options;

  function closeModal() {
    dispatch("CLOSE_MODAL");
  }

  function onExecute() {}

  const onExecuteAsync = async (action, data, callbackName) => {
    return await action(data)
      .then(() => {
        dispatch(callbackName);
        closeModal();
      })
      .catch((error) => {
        dispatch("NOTIFY", {
          title: "An error occured in processing",
          mesage: "An error occured when updating, please try again!",
          type: "error",
        });
      });
  };

  if (!isOpen) {
    return null;
  }

  const { children, title } = data;
  const { type, unableClose } = options;

  let modal = null;
  if (type === "AVATAR_MODAL") {
    modal = (
      <UpdateAvatarModal
        title={title}
        data={data}
        closeModal={closeModal}
        isDisabled={isDisabled}
        setDisabled={setDisabled}
        onExecute={(action, data, callbackName) =>
          onExecuteAsync(action, data, callbackName)
        }
      >
        {children}
      </UpdateAvatarModal>
    );
  } else if (type === "CONFIRM_REDIRECT") {
    modal = (
      <ConfirmToRedirectModal
        title={title}
        data={data}
        closeModal={closeModal}
        onExecute={onExecute}
      >
        {data.message}
      </ConfirmToRedirectModal>
    );
  } else {
    modal = <DefaultModal closeModal={closeModal}>{children}</DefaultModal>;
  }

  let backdrop = null;
  if (!!showBackdrop && !unableClose) {
    backdrop = <Backdrop onClick={closeModal} />;
  } else if (!!showBackdrop) {
    backdrop = <Backdrop />;
  }

  return (
    <Fragment>
      {backdrop}
      <Root className={className}>
        <Scroll>
          <PanelHeading>
            {title}
            {!unableClose ? (
              <CloseButton onClick={closeModal}>
                <FontAwesomeIcon icon="times" />
              </CloseButton>
            ) : null}
          </PanelHeading>
          {modal}
        </Scroll>
      </Root>
    </Fragment>
  );
};
