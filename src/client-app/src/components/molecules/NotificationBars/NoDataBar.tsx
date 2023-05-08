import * as React from "react";
import { NoDataBox } from "./NotificationBoxes";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { useTranslation } from "react-i18next";
import { HTMLAttributes } from "react";

const NoDataBar: React.FC<HTMLAttributes<HTMLSpanElement>> = (props) => {
  const { t } = useTranslation();
  const { children } = props;
  return (
    <NoDataBox>
      <FontAwesomeIcon icon="ghost" className="me-2"></FontAwesomeIcon>
      <span>{children ? children : t("no_content")}</span>
    </NoDataBox>
  );
};

export default NoDataBar;
