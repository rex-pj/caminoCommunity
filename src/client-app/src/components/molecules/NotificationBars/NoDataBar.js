import React from "react";
import { NoDataBox } from "./NotificationBoxes";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { useTranslation } from "react-i18next";

const NoDataBar = (props) => {
  const { t } = useTranslation();
  return (
    <NoDataBox>
      <FontAwesomeIcon icon="ghost" className="me-2"></FontAwesomeIcon>
      <span>{props.children ? props.children : t("no_content")}</span>
    </NoDataBox>
  );
};

export default NoDataBar;
