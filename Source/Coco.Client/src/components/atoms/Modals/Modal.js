import React, { Component, Fragment } from "react";
import styled from "styled-components";
import { PanelDefault, PanelFooter } from "../Panels";
import DefaultModal from "./DefaultModal";
import CropImageModal from "./CropImageModal";

const Root = styled(PanelDefault)`
  top: 45px;
  left: 0;
  right: 0;
  bottom: auto;
  z-index: 100;
  margin: 0 auto;
  max-width: 980px;
  max-height: 100%;
  position: absolute;
`;

const Scroll = styled.div`
  position: relative;

  ${PanelFooter} {
    border-top: 1px solid ${p => p.theme.color.light};
    text-align: right;

    button {
      margin-left: 3px;
    }
  }
`;

const Backdrop = styled.div`
  position: fixed;
  top: 0;
  bottom: 0;
  left: 0;
  right: 0;
  background-color: ${p => p.theme.rgbaColor.exDark};
  z-index: 1;
`;

export default class extends Component {
  constructor(props) {
    super(props);

    this.state = {
      shouldOpen: false,
      showBackdrop: true
    };
  }

  componentWillReceiveProps(nextProps) {
    this.setState({
      shouldOpen: nextProps.options.isOpen
    });
  }

  closeModal = () => {
    this.setState({
      shouldOpen: false
    });
  };

  onExecute = e => {
    console.log(e);
  };

  render() {
    const { shouldOpen, showBackdrop } = this.state;

    if (!shouldOpen) {
      return null;
    }

    const { className, options } = this.props;
    const { modalType, children } = options;

    return (
      <Fragment>
        <Root className={className}>
          <Scroll>
            {modalType === "crop-image" ? (
              <CropImageModal
                closeModal={this.closeModal}
                onExecute={this.onExecute}
              >
                {children}
              </CropImageModal>
            ) : (
              <DefaultModal closeModal={this.closeModal}>
                {children}
              </DefaultModal>
            )}
          </Scroll>
        </Root>
        {!!showBackdrop ? <Backdrop /> : null}
      </Fragment>
    );
  }
}
