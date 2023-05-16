import { ButtonOutlineLight } from "../../atoms/Buttons/OutlineButtons";
import { RouterLinkButtonOutlineNeutral } from "../../atoms/Buttons/RouterLinkButtons";

type Props = {
  children?: any;
  baseUrl?: string;
  currentPage?: number;
  pageNumber?: number;
  disabled?: boolean;
  pageQuery?: string;
  className?: string;
  size?: string;
};

const Bullet = (props: Props) => {
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

  let to = "";
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

  let ButtonItem: any;
  if ((!pageNumber && !baseUrl) || disabled) {
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
  if (currentPage === pageNumber) {
    return (
      <RouterLinkButtonOutlineNeutral
        size={size ? size : "sm"}
        to={to}
        disabled={true}
        className={className}
      >
        {children}
      </RouterLinkButtonOutlineNeutral>
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

export default Bullet;
