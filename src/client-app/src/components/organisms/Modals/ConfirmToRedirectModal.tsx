import * as React from "react";
import { Fragment } from "react";
import { PanelBody, PanelFooter } from "../../molecules/Panels";
import { ButtonPrimary } from "../../atoms/Buttons/Buttons";

interface ConfirmToRedirectModalProps {
  children?: any;
  data: {
    title: string;
    children?: any;
    executeButtonName: string;
  };
  execution: {
    onSucceed: () => void;
  };
  closeModal: () => void;
}

export const ConfirmToRedirectModal = (props: ConfirmToRedirectModalProps) => {
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
};
