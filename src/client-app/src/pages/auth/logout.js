import React, { useContext, useRef } from "react";
import AuthService from "../../services/authService";
import { Redirect } from "react-router-dom";
import { withRouter } from "react-router-dom";
import { useQuery } from "@apollo/client";
import LogoutPanel from "../../components/organisms/Auth/LogoutPanel";
import { userQueries } from "../../graphql/fetching/queries";
import { SessionContext } from "../../store/context/session-context";

export default withRouter((props) => {
  const { data, error } = useQuery(userQueries.LOGOUT);
  const { relogin } = useContext(SessionContext);
  const currentRef = useRef({
    renderCount: 0,
  });

  console.log(error);
  if (error) {
    return <Redirect to="/error" />;
  }

  if (data && currentRef.current.renderCount === 0) {
    const { logout } = data;
    if (logout.isSucceed) {
      const isCleared = AuthService.logOut();
      if (isCleared) {
        relogin();
        currentRef.current.renderCount += 1;
        return <Redirect to="/" />;
      }
    }
    return <Redirect to="/error" />;
  }

  return <LogoutPanel />;
});
