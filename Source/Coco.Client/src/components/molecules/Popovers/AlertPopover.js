import React, { Component } from "react";
import styled from "styled-components";
import { PanelDefault, PanelBody, PanelHeading } from "../../atoms/Panels";
import {
  ButtonOutlineNormal,
  ButtonOutlineDanger
} from "../../atoms/Buttons/OutlineButtons";

const Wrap = styled(PanelDefault)`
  position: absolute;
  bottom: calc(100% + ${p => p.theme.size.tiny});
  left: ${p => (p.left ? p.left + "px" : "auto")};
  top: ${p => (p.top ? p.top + "px" : "auto")};
  right: ${p => (p.right ? p.right + "px" : "auto")};
  bottom: ${p => (p.bottom ? p.bottom + "px" : "auto")};
  background-color: ${p => p.theme.color.warningLight};

  > ${PanelHeading} {
    border-bottom: 1px solid ${p => p.theme.rgbaColor.darkLight};
    color: ${p => p.theme.color.warning};
  }

  ::after {
    content: " ";
    display: block;
    position: absolute;
    left: ${p => p.theme.size.distance};
    top: 100%;
    width: 0;
    height: 0;
    border-left: ${p => p.theme.size.tiny} solid transparent;
    border-right: ${p => p.theme.size.tiny} solid transparent;
    border-top: ${p => p.theme.size.tiny} solid
      ${p => p.theme.color.warningLight};
  }
`;

class AlertPopover extends Component {
  constructor(props) {
    super(props);

    this.state = {
      isShown: false,
      left: null,
      top: null
    };
  }

  componentDidMount() {
    document.addEventListener("click", this.handleClickTarget);
  }

  componentWillUnmount() {
    document.removeEventListener("click", this.handleClickTarget);
  }

  handleClickTarget = event => {
    const { target } = this.props;

    var currentTarget = document.getElementById(target);
    if (currentTarget && currentTarget.contains(event.target)) {
      const { isShown } = this.state;
      this.setState(() => {
        return {
          isShown: !isShown,
          left: currentTarget.offsetLeft
        };
      });

      if (!isShown && this.props.onClose) {
        this.props.onClose();
      }

      if (isShown && this.props.onOpen) {
        this.props.onOpen();
      }
    }
  };

  onClose = () => {
    this.setState(() => {
      return {
        isShown: false
      };
    });
    if (this.props.onClose) {
      this.props.onClose();
    }
  };

  onExecute = () => {
    if (this.props.onExecute) {
      this.props.onExecute();
    }
  };

  render() {
    const { isShown, left } = this.state;
    const { title } = this.props;
    if (!isShown) {
      return null;
    }

    return (
      <Wrap left={left} bottom="100%">
        <PanelHeading>{title}</PanelHeading>
        <PanelBody>
          <ButtonOutlineNormal size="sm" onClick={this.onClose}>
            Không
          </ButtonOutlineNormal>
          <ButtonOutlineDanger size="sm" onClick={this.onExecute}>
            Đồng Ý
          </ButtonOutlineDanger>
        </PanelBody>
      </Wrap>
    );
  }
}

export default AlertPopover;
