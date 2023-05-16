import * as React from "react";
import { ButtonOutlineLight } from "../../atoms/Buttons/OutlineButtons";
import { RouterLinkButtonOutlineNeutral } from "../../atoms/Buttons/RouterLinkButtons";

type Props = {
  children?: any;
  baseUrl?: string;
  currentPage: number;
  disabled?: boolean;
  pageQuery?: string;
  className?: string;
  size?: string;
};

const PagerBullet = (props: Props) => {
  const {
    baseUrl,
    children,
    currentPage,
    disabled,
    pageQuery,
    className,
    size,
  } = props;

  let to = `${baseUrl}${pageQuery}`;
  if (currentPage) {
    to += `&page=${currentPage + 1}`;
  }

  if ((!currentPage && !baseUrl) || disabled) {
    return (
      <ButtonOutlineLight
        size={size ? size : "sm"}
        disabled={true}
        className={className}
      >
        {children}
      </ButtonOutlineLight>
    );
  }

  return (
    <RouterLinkButtonOutlineNeutral
      size={size ? size : "sm"}
      to={to}
      className={className}
    >
      {children}
    </RouterLinkButtonOutlineNeutral>
  );
};

export default PagerBullet;
