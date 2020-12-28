import React, { Fragment } from "react";
import { PanelBody, PanelFooter } from "../../atoms/Panels";
import { ButtonPrimary, ButtonLight } from "../../atoms/Buttons/Buttons";

export default function (props) {
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
}
