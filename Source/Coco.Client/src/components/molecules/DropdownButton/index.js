import React, { Component } from "react";
import styled from "styled-components";
import { Button } from "../../atoms/Buttons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import Dropdown from "./Dropdown";
import ModuleMenuListItem from "../MenuList/ModuleMenuListItem";

const DropdownGroup = styled.div`
  position: relative;
  display: inline-block;
`;

const ButtonCaret = styled(Button)`
  padding: 0;
  text-align: center;
  width: ${p => p.theme.size.small};
  height: ${p => p.theme.size.small};
`;

const DropdownList = styled(Dropdown)`
  position: absolute;
  right: 0;
  top: 100%;
  background: ${p => p.theme.color.white};
  box-shadow: ${p => p.theme.shadow.BoxShadow};
  min-width: calc(${p => p.theme.size.large} * 3);
  border-radius: ${p => p.theme.borderRadius.normal};
  border-top-right-radius: 0;
  border: 1px solid ${p => p.theme.color.exLight};

  ${ModuleMenuListItem} {
    margin-bottom: 0;
    border-bottom: 1px solid ${p => p.theme.color.exLight};

    :last-child {
      border-bottom: 0;
    }
  }

  ${ModuleMenuListItem} > a {
    padding: ${p => p.theme.size.distance};
    border-radius: 0;
    font-weight: 600;
  }
`;

export default class extends Component {
  constructor(props) {
    super(props);

    this.state = {
      isShown: false
    };

    this.currentRef = React.createRef();
  }

  componentDidMount() {
    document.addEventListener("click", this.onHide, false);
  }

  componentWillUnmount() {
    document.removeEventListener("click", this.onHide);
  }

  onHide = e => {
    if (!this.currentRef.current.contains(e.target)) {
      this.setState({
        isShown: false
      });
    }
  };

  show = () => {
    this.setState(() => {
      return {
        isShown: !this.state.isShown
      };
    });
  };

  render() {
    const { className, dropdown } = this.props;
    const { isShown } = this.state;

    return (
      <DropdownGroup className={className} ref={this.currentRef}>
        <ButtonCaret onClick={this.show}>
          <FontAwesomeIcon icon="caret-down" />
        </ButtonCaret>
        {dropdown && isShown ? (
          <DropdownList>
            {dropdown.map((item, index) => (
              <ModuleMenuListItem key={index}>
                <a href={item.url}>
                  <span>{item.name}</span>
                </a>
              </ModuleMenuListItem>
            ))}
          </DropdownList>
        ) : null}
      </DropdownGroup>
    );
  }
}
