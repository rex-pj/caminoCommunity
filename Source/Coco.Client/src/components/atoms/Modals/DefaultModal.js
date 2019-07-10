import React, { Fragment } from "react";
import { PanelBody, PanelFooter } from "../Panels";
import { Button, ButtonSecondary } from "../Buttons";

export default function(props) {
  const { children } = props;

  return (
    <Fragment>
      <PanelBody>{children}</PanelBody>
      <PanelFooter>
        <Button>Upload</Button>
        <ButtonSecondary onClick={() => props.closeModal()}>
          Hủy
        </ButtonSecondary>
      </PanelFooter>
    </Fragment>
  );
}
