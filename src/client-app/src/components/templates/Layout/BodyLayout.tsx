import * as React from "react";
import { useContext } from "react";
import { SessionContext } from "../../../store/context/session-context";
import {
  ErrorBar,
  LoadingBar,
  NoDataBar,
} from "../../molecules/NotificationBars";
import PageLoading from "../../molecules/Loading/PageLoading";

interface Props {
  isLoading?: boolean;
  hasData?: boolean;
  hasError?: boolean;
  children?: any;
}

// The layout with header
const BodyLayout = (props: Props) => {
  const { children, isLoading, hasData, hasError } = props;
  const { isLoading: isRequestLogin } = useContext(SessionContext);

  if (isRequestLogin) {
    return <PageLoading {...props} />;
  }

  const renderChildren = () => {
    if (isLoading) {
      return <LoadingBar />;
    }
    if (!hasData) {
      return <NoDataBar />;
    }
    if (hasError) {
      return <ErrorBar />;
    }

    return children;
  };

  return <>{renderChildren()}</>;
};

export default BodyLayout;
