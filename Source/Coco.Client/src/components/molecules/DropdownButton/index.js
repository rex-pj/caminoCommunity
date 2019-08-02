import React, { Component } from "react";
import styled from "styled-components";
import { Link } from "react-router-dom";
import { ButtonTransparent } from "../../atoms/Buttons/Buttons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import Dropdown from "./Dropdown";
import ModuleMenuListItem from "../MenuList/ModuleMenuListItem";

const DropdownGroup = styled.div`
  position: relative;
  display: inline-block;
  z-index: 2;
`;

const ButtonCaret = styled(ButtonTransparent)`
  padding: 0;
  text-align: center;
  width: ${p => p.theme.size.small};
  height: ${p => p.theme.size.small};
`;

const DropdownList = styled(Dropdown)`
  position: absolute;
  right: 0;
  top: calc(100% + ${p => p.theme.size.exTiny});
  background: ${p => p.theme.color.white};
  box-shadow: ${p => p.theme.shadow.BoxShadow};
  min-width: calc(${p => p.theme.size.large} * 3);
  border-radius: ${p => p.theme.borderRadius.normal};
  padding: ${p => p.theme.size.exTiny} 0;

  ${ModuleMenuListItem} {
    margin-bottom: 0;
    border-bottom: 1px solid ${p => p.theme.color.lighter};

    :last-child {
      border-bottom: 0;
    }
  }

  ${ModuleMenuListItem} > a {
    padding: ${p => p.theme.size.distance};
    border-radius: 0;
    font-weight: 600;
  }

  :after {
    position: absolute;
    top: -${p => p.theme.size.exTiny};
    right: ${p => p.theme.size.exTiny};
    content: " ";
    width: 0;
    height: 0;
    border-left: ${p => p.theme.size.exTiny} solid transparent;
    border-right: ${p => p.theme.size.exTiny} solid transparent;
    border-bottom: ${p => p.theme.size.exTiny} solid ${p => p.theme.color.white};
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
    const { className, dropdown, icon } = this.props;
    const { isShown } = this.state;

    return (
      <DropdownGroup className={className} ref={this.currentRef}>
        <ButtonCaret onClick={this.show}>
          <FontAwesomeIcon icon={icon ? icon : "caret-down"} />
        </ButtonCaret>
        {dropdown && isShown ? (
          <DropdownList>
            {dropdown.map((item, index) => (
              <ModuleMenuListItem key={index}>
                {!!item.isNav ? (
                  <Link to={item.url}>
                    <span>{item.name}</span>
                  </Link>
                ) : (
                  <a href={item.url}>
                    <span>{item.name}</span>
                  </a>
                )}
              </ModuleMenuListItem>
            ))}
          </DropdownList>
        ) : null}
      </DropdownGroup>
    );
  }
}
