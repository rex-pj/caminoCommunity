import React, { useContext } from "react";
import { SessionContext } from "../../../store/context/session-context";
import {
  ErrorBar,
  LoadingBar,
  NoDataBar,
} from "../../molecules/NotificationBars";
import PropTypes from "prop-types";
import PageLoading from "../../molecules/Loading/PageLoading";

// The layout with header
const BodyLayout = (props) => {
  const { children, isLoading, hasData, hasError } = props;
  const { isLoading: isRequestLogin } = useContext(SessionContext);

  if (!!isRequestLogin) {
    return <PageLoading {...props} />;
  }

  const renderChildren = () => {
    if (isLoading) {
      return <LoadingBar>Loading</LoadingBar>;
    }
    if (!hasData) {
      return <NoDataBar>No data</NoDataBar>;
    }
    if (hasError) {
      return <ErrorBar>Error!</ErrorBar>;
    }

    return children;
  };

  return <>{renderChildren()}</>;
};

BodyLayout.propTypes = {
  isLoading: PropTypes.bool,
  hasData: PropTypes.bool,
  hasError: PropTypes.bool,
};

BodyLayout.defaultProps = {
  isLoading: false,
  hasData: false,
  hasError: false,
};

export default BodyLayout;
