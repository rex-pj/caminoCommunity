import React, { Component, Fragment } from "react";
import styled from "styled-components";
import { PanelDefault, PanelFooter, PanelHeading } from "../../atoms/Panels";
import DefaultModal from "./DefaultModal";
import CropImageModal from "./CropImageModal";
import { modalPushData } from "../../../store/commands";
import { connect } from "react-redux";

const Root = styled(PanelDefault)`
  top: 45px;
  left: 0;
  right: 0;
  bottom: auto;
  z-index: 100;
  margin: 0 auto;
  max-width: 980px;
  position: absolute;
`;

const Scroll = styled.div`
  position: relative;

  ${PanelHeading} {
    border-bottom: 1px solid ${p => p.theme.color.light};
    font-weight: 600;
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

  onExecute = e => {
    if (this.props.onExecute) {
      this.props.onExecute(e);
    }
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
            <PanelHeading>{title}</PanelHeading>
            {modalType === "crop-image" ? (
              <CropImageModal
                title={title}
                data={payload}
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

const mapDispatchToProps = dispatch => {
  return {
    onExecute: data => {
      modalPushData(dispatch, data);
    }
  };
};

const mapStateToProps = state => {
  return {
    payload: state.modalReducer.payload
  };
};

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(Modal);
