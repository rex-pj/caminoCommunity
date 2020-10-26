import React, { Fragment } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { ButtonSecondary } from "../../atoms/Buttons/Buttons";
import styled from "styled-components";

const EditorTabs = styled.div`
  margin-bottom: ${(p) => p.theme.size.exTiny};
  .tabs-bar button {
    border-radius: ${(p) => p.theme.borderRadius.large};
    color: ${(p) => p.theme.color.light};
    background-color: transparent;
    font-weight: normal;
    border-color: transparent;
  }
  .tabs-bar button.actived {
    color: ${(p) => p.theme.color.neutral};
    background-color: ${(p) => p.theme.color.light};
  }
`;

export default function (props) {
  const { editorMode, onToggleCreateMode } = props;

  return (
    <Fragment>
      <EditorTabs>
        <div className="tabs-bar">
          <ButtonSecondary
            size="sm"
            className={`mr-1${editorMode === "ARTICLE" ? " actived" : ""}`}
            onClick={() => onToggleCreateMode("ARTICLE")}
          >
            <span>
              <FontAwesomeIcon
                icon="newspaper"
                className="mr-1"
              ></FontAwesomeIcon>
              Create Post
            </span>
          </ButtonSecondary>
          <ButtonSecondary
            size="sm"
            onClick={() => onToggleCreateMode("PRODUCT")}
            className={`mr-1${editorMode === "PRODUCT" ? " actived" : ""}`}
          >
            <span>
              <FontAwesomeIcon
                icon="apple-alt"
                className="mr-1"
              ></FontAwesomeIcon>
              Create Product
            </span>
          </ButtonSecondary>
          <ButtonSecondary
            size="sm"
            onClick={() => onToggleCreateMode("FARM")}
            className={`mr-1${editorMode === "FARM" ? " actived" : ""}`}
          >
            <span>
              <FontAwesomeIcon
                icon="warehouse"
                className="mr-1"
              ></FontAwesomeIcon>
              Create Farm
            </span>
          </ButtonSecondary>
        </div>
      </EditorTabs>
    </Fragment>
  );
}
