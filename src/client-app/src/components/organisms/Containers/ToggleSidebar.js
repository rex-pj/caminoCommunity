import React, { useState } from "react";
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
  const [isLeftShown, setLeftShown] = useState();
  const [isRightShown, setRightShown] = useState();
  const showLeftSidebar = () => {
    setLeftShown(true);
    setRightShown(false);
    props.showLeftSidebar();
  };

  const showRightSidebar = () => {
    setRightShown(true);
    setLeftShown(false);
    props.showRightSidebar();
  };

  const showCentral = () => {
    setRightShown(false);
    setLeftShown(false);
    props.showCentral();
  };

  const className = props.className ? ` ${props.className}` : "";
  return (
    <div className={`nav container-fluid px-lg-5${className}`}>
      <div className="row">
        <div className="col col-3 col-sm-3 col-md-3">
          {!isLeftShown ? (
            <SidebarButton size="xs" onClick={showLeftSidebar}>
              <FontAwesomeIcon icon="align-left" />
            </SidebarButton>
          ) : (
            <SidebarButton size="xs" onClick={showCentral}>
              <FontAwesomeIcon icon="times" />
            </SidebarButton>
          )}
        </div>
        <div className="col col-6 col-sm-6 col-md-6"></div>
        <div className="col col-3 col-sm-3 col-md-3">
          {!isRightShown ? (
            <SidebarButton
              className="align-end"
              size="xs"
              onClick={showRightSidebar}
            >
              <FontAwesomeIcon icon="align-right" />
            </SidebarButton>
          ) : (
            <SidebarButton
              className="align-end"
              size="xs"
              onClick={showCentral}
            >
              <FontAwesomeIcon icon="times" />
            </SidebarButton>
          )}
        </div>
      </div>
    </div>
  );
};
