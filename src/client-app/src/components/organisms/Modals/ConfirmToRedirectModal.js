import React, { Fragment } from "react";
import { PanelBody, PanelFooter } from "../../molecules/Panels";
import { ButtonPrimary } from "../../atoms/Buttons/Buttons";

export default function (props) {
  const { children, data, execution } = props;
  const { executeButtonName } = data;
  const { onSucceed } = execution;

  const onRedirect = () => {
    onSucceed();
    props.closeModal();
  };

  return (
    <Fragment>
      {children ? <PanelBody>{children}</PanelBody> : null}
      <PanelFooter>
        <ButtonPrimary onClick={onRedirect}>{executeButtonName}</ButtonPrimary>
      </PanelFooter>
    </Fragment>
  );
}
