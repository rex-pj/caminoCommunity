import React, { Component, Fragment } from "react";
import styled from "styled-components";
import { PanelDefault, PanelFooter, PanelHeading } from "../../atoms/Panels";
import DefaultModal from "./DefaultModal";
import ChangeAvatarModal from "./ChangeAvatarModal";
import { connect } from "react-redux";
import { ButtonTransparent } from "../../atoms/Buttons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

const Root = styled(PanelDefault)`
  top: 45px;
  left: 0;
  right: 0;
  bottom: auto;
  z-index: 100;
  margin: 0 auto;
  position: absolute;
  max-width: ${p => (p.size === "lg" ? "90%" : "720px")};
`;

const Scroll = styled.div`
  position: relative;

  > ${PanelHeading} {
    border-bottom: 1px solid ${p => p.theme.color.light};
    font-weight: 600;
    position: relative;
  }

  ${PanelFooter} {
    border-top: 1px solid ${p => p.theme.color.light};
    text-align: right;

    button {
      margin-left: ${p => p.theme.size.tiny};
      border-radius: ${p => p.theme.borderRadius.large};
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
  background-color: ${p => p.theme.rgbaColor.exDark};
  z-index: 1;
`;

class Modal extends Component {
  constructor(props) {
    super(props);

    this.state = {
      showBackdrop: true
    };
  }

  componentWillReceiveProps(nextProps) {
    if (nextProps && nextProps.payload) {
      const { payload } = nextProps;
      this.setState(() => {
        return {
          shouldOpen: payload && !!payload.isOpen
        };
      });
    }
  }

  closeModal = () => {
    this.setState({
      shouldOpen: false
    });
  };

  render() {
    const { showBackdrop, shouldOpen } = this.state;
    const { className, payload } = this.props;

    if (!shouldOpen) {
      return null;
    }

    const { modalType, children, title } = payload;

    return (
      <Fragment>
        <Root className={className}>
          <Scroll>
            <PanelHeading>
              {title}
              <CloseButton onClick={this.closeModal}>
                <FontAwesomeIcon icon="times" />
              </CloseButton>
            </PanelHeading>
            {modalType === "change-avatar" ? (
              <ChangeAvatarModal
                title={title}
                data={payload}
                closeModal={this.closeModal}
                onExecute={this.onExecute}
              >
                {children}
              </ChangeAvatarModal>
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

const mapStateToProps = state => {
  return {
    payload: state.modalReducer.payload
  };
};

export default connect(mapStateToProps)(Modal);
