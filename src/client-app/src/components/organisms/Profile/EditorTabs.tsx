import React, { Fragment } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { ButtonPrimary } from "../../atoms/Buttons/Buttons";
import styled from "styled-components";
import { useTranslation } from "react-i18next";

const Tabs = styled.div`
  margin-bottom: ${(p) => p.theme.size.exTiny};
  .tabs-bar button {
    border-radius: 0;
    background-color: transparent;
    font-weight: normal;
    border-color: transparent;
    color: ${(p) => p.theme.color.primaryText};
  }
  .tabs-bar button.actived {
    color: ${(p) => p.theme.color.neutralText};
    background-color: ${(p) => p.theme.color.secondaryText};
    border-radius: ${(p) => p.theme.borderRadius.normal};
  }
`;

const EditorTabs = (props) => {
  const { editorMode, onToggleCreateMode } = props;
  const { t } = useTranslation();

  return (
    <Fragment>
      <Tabs>
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
              {t("publish_post")}
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
              {t("create_farm")}
            </span>
          </ButtonPrimary>
          <ButtonPrimary
            size="xs"
            onClick={() => onToggleCreateMode("PRODUCT")}
            className={`me-1${editorMode === "PRODUCT" ? " actived" : ""}`}
          >
            <span>
              <FontAwesomeIcon icon="carrot" className="me-1"></FontAwesomeIcon>
              {t("publish_product")}
            </span>
          </ButtonPrimary>
        </div>
      </Tabs>
    </Fragment>
  );
};

export default EditorTabs;
