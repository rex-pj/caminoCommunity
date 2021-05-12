import React from "react";
import styled from "styled-components";
import { ButtonTransparent } from "../../atoms/Buttons/Buttons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

const SidebarButton = styled(ButtonTransparent)`
  color: ${(p) => p.theme.color.lightText};

  &.align-end {
    float: right;
  }
`;

export default (props) => {
  const { isLeftShown, isRightShown } = props;
  const showLeftSidebar = () => {
    props.showLeftSidebar();
  };

  const showRightSidebar = () => {
    props.showRightSidebar();
  };

  const resetSidebars = () => {
    props.resetSidebars();
  };

  const className = props.className ? ` ${props.className}` : "";
  return (
    <div className={`nav container-fluid px-lg-5${className}`}>
      <div className="row">
        <div className="col col-3 col-sm-3 col-md-3">
          {isLeftShown ? (
            <SidebarButton size="xs" onClick={resetSidebars}>
              <FontAwesomeIcon icon="times" />
            </SidebarButton>
          ) : (
            <SidebarButton size="xs" onClick={showLeftSidebar}>
              <FontAwesomeIcon icon="align-left" />
            </SidebarButton>
          )}
        </div>
        <div className="col col-6 col-sm-6 col-md-6"></div>
        <div className="col col-3 col-sm-3 col-md-3">
          {isRightShown ? (
            <SidebarButton
              className="align-end"
              size="xs"
              onClick={resetSidebars}
            >
              <FontAwesomeIcon icon="times" />
            </SidebarButton>
          ) : (
            <SidebarButton
              className="align-end"
              size="xs"
              onClick={showRightSidebar}
            >
              <FontAwesomeIcon icon="align-right" />
            </SidebarButton>
          )}
        </div>
      </div>
    </div>
  );
};
