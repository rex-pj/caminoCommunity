import * as React from "react";
import { useRef, useEffect, useState } from "react";
import styled from "styled-components";
import { Link } from "react-router-dom";
import { ButtonPrimary } from "../../atoms/Buttons/Buttons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import Dropdown from "./Dropdown";
import ModuleMenuListItem from "../MenuList/ModuleMenuListItem";
import { IconProp } from "@fortawesome/fontawesome-svg-core";

const DropdownGroup = styled.div`
  position: relative;
  display: inline-block;
  z-index: 2;
`;

const ButtonCaret = styled(ButtonPrimary)`
  padding: 0;
  text-align: center;
  width: ${(p) => p.theme.size.small};
  height: ${(p) => p.theme.size.small};
`;

const DropdownList = styled(Dropdown)`
  position: absolute;
  right: 0;
  top: calc(100% + ${(p) => p.theme.size.exTiny});
  background: ${(p) => p.theme.color.whiteBg};
  box-shadow: ${(p) => p.theme.shadow.BoxShadow};
  min-width: calc(${(p) => p.theme.size.large} * 3);
  border-radius: ${(p) => p.theme.borderRadius.normal};
  padding: ${(p) => p.theme.size.exTiny} 0;

  ${ModuleMenuListItem} {
    margin-bottom: 0;
    border-bottom: 1px solid ${(p) => p.theme.color.neutralBg};

    a:hover {
      background-color: ${(p) => p.theme.color.neutralBg};
    }
    :last-child {
      border-bottom: 0;
    }
  }

  ${ModuleMenuListItem} > a {
    padding: ${(p) => p.theme.size.distance};
    border-radius: 0;
    font-weight: 600;
  }

  :after {
    position: absolute;
    top: -${(p) => p.theme.size.exTiny};
    right: ${(p) => p.theme.size.exTiny};
    content: " ";
    width: 0;
    height: 0;
    border-left: ${(p) => p.theme.size.exTiny} solid transparent;
    border-right: ${(p) => p.theme.size.exTiny} solid transparent;
    border-bottom: ${(p) => p.theme.size.exTiny} solid
      ${(p) => p.theme.color.whiteBg};
  }
`;

interface DropdownButtonProps {
  readonly icon?: IconProp;
  readonly dropdown: any[];
  className?: string;
}

const DropdownButton: React.FC<DropdownButtonProps> = (props) => {
  const [isShown, setShow] = useState(false);
  const currentRef = useRef<HTMLDivElement>();
  const { className, dropdown, icon } = props;

  useEffect(() => {
    document.addEventListener("click", onHide, false);
    return () => {
      document.removeEventListener("click", onHide);
    };
  });

  const onHide = (e: any) => {
    if (currentRef.current && !currentRef.current.contains(e.target)) {
      setShow(false);
    }
  };

  const show = () => {
    setShow(!isShown);
  };

  return (
    <DropdownGroup className={className} ref={currentRef}>
      <ButtonCaret onClick={show}>
        <FontAwesomeIcon icon={icon ? icon : "caret-down"} />
      </ButtonCaret>
      {dropdown && isShown ? (
        <DropdownList>
          {dropdown.map((item: any, index: number) => (
            <ModuleMenuListItem key={index}>
              {item.isNav ? (
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
};

export default DropdownButton;
