import React, { useContext, useRef } from "react";
import AuthService from "../../services/AuthService";
import { Redirect } from "react-router-dom";
import { withRouter } from "react-router-dom";
import { useQuery } from "@apollo/client";
import LogoutPanel from "../../components/organisms/Auth/LogoutPanel";
import { LOGOUT } from "../../utils/GraphQLQueries/queries";
import { SessionContext } from "../../store/context/SessionContext";

export default withRouter((props) => {
  const { data, error } = useQuery(LOGOUT);
  const sessionContext = useContext(SessionContext);
  const currentRef = useRef({
    renderCount: 0,
  });

  if (error) {
    return <Redirect to="/error" />;
  }

  if (data && currentRef.current.renderCount === 0) {
    const { logout } = data;
    if (logout.isSucceed) {
      const isCleared = AuthService.logOut();
      if (isCleared) {
        sessionContext.relogin();
        currentRef.current.renderCount += 1;
        return <Redirect to="/" />;
      }
    }
    return <Redirect to="/error" />;
  }

  return <LogoutPanel />;
});
