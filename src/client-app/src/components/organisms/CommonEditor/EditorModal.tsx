import * as React from "react";
import { Fragment, HTMLAttributes, useRef } from "react";
import styled from "styled-components";

const Root = styled.div`
  position: absolute;
  display: block;
  z-index: 2;
  left: 0;
  top: 0;
  bottom: 0;
  right: 0;
  margin: auto;
  min-width: 100px;
  border-radius: ${(p) => p.theme.borderRadius.normal};
  background-color: ${(p) => p.theme.rgbaColor.darker};
`;

const Container = styled.div`
  margin: auto;
  position: relative;
  z-index: 10;
`;

interface EditorModalProps extends HTMLAttributes<HTMLDivElement> {
  isOpen: boolean;
  modalBodyComponent: any;
  onClose?: (isClosed: boolean) => void | undefined;
  onAccept?: (e: any) => void | undefined;
  editorState?: any;
  currentValue?: any;
  convertImageCallback?: (e: any) => void;
  onImageValidate?: (e: any) => void;
}

const EditorModal: React.FC<EditorModalProps> = (props) => {
  const { className, isOpen, modalBodyComponent: ModalBodyComponent } = props;

  const currentRef = useRef();

  const onClose = () => {
    if (props.onClose) {
      props.onClose(false);
    }
  };

  const onAccept = (e: UIEvent) => {
    props.onAccept(e);
  };

  return (
    <Fragment>
      {isOpen ? (
        <Root className={className} ref={currentRef}>
          <Container>
            <ModalBodyComponent
              {...props}
              onAccept={onAccept}
              onClose={onClose}
            />
          </Container>
        </Root>
      ) : null}
    </Fragment>
  );
};

export default EditorModal;
