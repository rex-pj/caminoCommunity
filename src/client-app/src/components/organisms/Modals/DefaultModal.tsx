import * as React from "react";
import { Fragment } from "react";
import { PanelBody, PanelFooter } from "../../molecules/Panels";
import { ButtonPrimary, ButtonLight } from "../../atoms/Buttons/Buttons";

type Props = {
  children?: any;
  closeModal: () => void;
};

const DefaultModal = (props: Props) => {
  const { children, closeModal } = props;

  return (
    <Fragment>
      <PanelBody>{children}</PanelBody>
      <PanelFooter>
        <ButtonPrimary>Upload</ButtonPrimary>
        <ButtonLight onClick={() => closeModal()}>Cancel</ButtonLight>
      </PanelFooter>
    </Fragment>
  );
};

export default DefaultModal;
