import React, { Fragment } from "react";
import { ButtonOutlineLight } from "../../atoms/Buttons/OutlineButtons";
import { RouterLinkButtonOutlineNeutral } from "../../atoms/Buttons/RouterLinkButtons";

export default (props) => {
  const {
    baseUrl,
    children,
    currentPage,
    pageNumber,
    disabled,
    pageQuery,
  } = props;

  let to = `${baseUrl}${"/page/"}${pageNumber}`;
  if (baseUrl && pageNumber) {
    to = `${baseUrl}${"/page/"}${pageNumber}`;
  } else if (baseUrl) {
    to = `${baseUrl}`;
  } else if (pageNumber) {
    to = `${"/page/"}${pageNumber}`;
  } else {
    to = "/";
  }

  if (pageQuery) {
    to = `${to}${pageQuery}`;
  }

  let ButtonItem = null;
  if (!pageNumber || disabled) {
    ButtonItem = (
      <ButtonOutlineLight size="sm" disabled={true}>
        {children}
      </ButtonOutlineLight>
    );
  } else if (currentPage === pageNumber) {
    ButtonItem = (
      <RouterLinkButtonOutlineNeutral size="sm" to={to} disabled={true}>
        {children}
      </RouterLinkButtonOutlineNeutral>
    );
  } else {
    ButtonItem = (
      <RouterLinkButtonOutlineNeutral size="sm" to={to}>
        {children}
      </RouterLinkButtonOutlineNeutral>
    );
  }

  return <Fragment>{ButtonItem}</Fragment>;
};
