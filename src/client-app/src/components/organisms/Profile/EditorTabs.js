import React, { Fragment } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { ButtonPrimary } from "../../atoms/Buttons/Buttons";
import styled from "styled-components";

const EditorTabs = styled.div`
  margin-bottom: ${(p) => p.theme.size.exTiny};
  .tabs-bar button {
    border-radius: 0;
    background-color: transparent;
    font-weight: normal;
    border-color: transparent;
    color: ${(p) => p.theme.color.secondaryText};
    border-bottom: 3px solid transparent;
  }
  .tabs-bar button.actived {
    color: ${(p) => p.theme.color.primaryText};
  }
`;

export default function (props) {
  const { editorMode, onToggleCreateMode } = props;

  return (
    <Fragment>
      <EditorTabs>
        <div className="tabs-bar">
          <ButtonPrimary
            size="xs"
            className={`me-1${editorMode === "ARTICLE" ? " actived" : ""}`}
            onClick={() => onToggleCreateMode("ARTICLE")}
          >
            <span>
              <FontAwesomeIcon
                icon="list-alt"
                className="me-1"
              ></FontAwesomeIcon>
              Create Post
            </span>
          </ButtonPrimary>
          <ButtonPrimary
            size="xs"
            onClick={() => onToggleCreateMode("FARM")}
            className={`me-1${editorMode === "FARM" ? " actived" : ""}`}
          >
            <span>
              <FontAwesomeIcon
                icon="tractor"
                className="me-1"
              ></FontAwesomeIcon>
              Create Farm
            </span>
          </ButtonPrimary>
          <ButtonPrimary
            size="xs"
            onClick={() => onToggleCreateMode("PRODUCT")}
            className={`me-1${editorMode === "PRODUCT" ? " actived" : ""}`}
          >
            <span>
              <FontAwesomeIcon icon="carrot" className="me-1"></FontAwesomeIcon>
              Create Product
            </span>
          </ButtonPrimary>
        </div>
      </EditorTabs>
    </Fragment>
  );
}
