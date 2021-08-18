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
    className,
    size,
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
  if ((!pageNumber && !baseUrl) || disabled) {
    ButtonItem = (
      <ButtonOutlineLight
        size={size ? size : "sm"}
        disabled={true}
        className={className}
      >
        {children}
      </ButtonOutlineLight>
    );
  } else if (currentPage === pageNumber) {
    ButtonItem = (
      <RouterLinkButtonOutlineNeutral
        size={size ? size : "sm"}
        to={to}
        disabled={true}
        className={className}
      >
        {children}
      </RouterLinkButtonOutlineNeutral>
    );
  } else {
    ButtonItem = (
      <RouterLinkButtonOutlineNeutral
        size={size ? size : "sm"}
        to={to}
        className={className}
      >
        {children}
      </RouterLinkButtonOutlineNeutral>
    );
  }

  return <Fragment>{ButtonItem}</Fragment>;
};
