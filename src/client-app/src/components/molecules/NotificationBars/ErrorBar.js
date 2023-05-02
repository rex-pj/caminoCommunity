import React from "react";
import { ErrorBox } from "./NotificationBoxes";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { useTranslation } from "react-i18next";

const ErrorBar = (props) => {
  const { t } = useTranslation();

  return (
    <ErrorBox>
      <FontAwesomeIcon
        icon="exclamation-triangle"
        className="me-2"
      ></FontAwesomeIcon>
      <span>
        {props.children ? props.children : t("unexpected_error_try_again")}
      </span>
    </ErrorBox>
  );
};

export default ErrorBar;
