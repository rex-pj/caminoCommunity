import React, { Fragment } from "react";
import { ButtonOutlineSecondary } from "../../atoms/Buttons";
import {
  RouterLinkButtonOutlineNormal,
  RouterLinkButtonOutlineSecondary
} from "../../atoms/RouterLinkButtons";

export default props => {
  const {
    baseUrl,
    children,
    currentPage,
    pageNumber,
    disabled,
    pageQuery
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
      <ButtonOutlineSecondary size="sm" disabled={true}>
        {children}
      </ButtonOutlineSecondary>
    );
  } else if (currentPage === pageNumber) {
    ButtonItem = (
      <RouterLinkButtonOutlineSecondary size="sm" to={to} disabled={true}>
        {children}
      </RouterLinkButtonOutlineSecondary>
    );
  } else {
    ButtonItem = (
      <RouterLinkButtonOutlineNormal size="sm" to={to}>
        {children}
      </RouterLinkButtonOutlineNormal>
    );
  }

  return <Fragment>{ButtonItem}</Fragment>;
};
