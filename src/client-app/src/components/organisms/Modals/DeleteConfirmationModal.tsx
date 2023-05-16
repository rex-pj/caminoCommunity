import * as React from "react";
import { Fragment } from "react";
import { PanelBody, PanelFooter } from "../../molecules/Panels";
import { ButtonPrimary, ButtonLight } from "../../atoms/Buttons/Buttons";

type Props = {
  children?: any;
  data: {
    id: any;
    title: string;
    children?: any;
    executeButtonName: string;
  };
  execution: {
    onDelete: (id: any) => void;
  };
  closeModal: () => void;
};

const DeleteConfirmationModal = (props: Props) => {
  const { children } = props;

  const onDeletting = () => {
    const { data, execution } = props;
    const { id } = data;
    const { onDelete } = execution;
    onDelete(id);
    props.closeModal();
  };

  return (
    <Fragment>
      <PanelBody>{children}</PanelBody>
      <PanelFooter>
        <ButtonPrimary onClick={onDeletting} size="xs">
          Delete
        </ButtonPrimary>
        <ButtonLight onClick={() => props.closeModal()} size="xs">
          Cancel
        </ButtonLight>
      </PanelFooter>
    </Fragment>
  );
};

export default DeleteConfirmationModal;
