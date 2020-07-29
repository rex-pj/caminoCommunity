import React, { Fragment } from "react";
import { PanelBody, PanelFooter } from "../../atoms/Panels";
import { ButtonPrimary, ButtonSecondary } from "../../atoms/Buttons/Buttons";

export default function(props) {
  const { children } = props;

  return (
    <Fragment>
      <PanelBody>{children}</PanelBody>
      <PanelFooter>
        <ButtonPrimary>Upload</ButtonPrimary>
        <ButtonSecondary onClick={() => props.closeModal()}>
          Hủy
        </ButtonSecondary>
      </PanelFooter>
    </Fragment>
  );
}
