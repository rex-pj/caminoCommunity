import * as React from "react";
import { ErrorBox } from "./NotificationBoxes";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { useTranslation } from "react-i18next";
import { HTMLAttributes } from "react";

const ErrorBar: React.FC<HTMLAttributes<HTMLSpanElement>> = (props) => {
  const { t } = useTranslation();
  const { children } = props;

  return (
    <ErrorBox>
      <FontAwesomeIcon
        icon="exclamation-triangle"
        className="me-2"
      ></FontAwesomeIcon>
      <span>{children ? children : t("unexpected_error_try_again")}</span>
    </ErrorBox>
  );
};

export default ErrorBar;
