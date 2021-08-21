import React, { useContext, useEffect } from "react";
import AuthService from "../../services/authService";
import { Redirect } from "react-router-dom";
import { withRouter } from "react-router-dom";
import LogoutPanel from "../../components/organisms/Auth/LogoutPanel";
import { SessionContext } from "../../store/context/session-context";

export default withRouter(() => {
  const { relogin } = useContext(SessionContext);

  useEffect(() => {
    AuthService.logOut();
    relogin();
    return <Redirect to="/" />;
  });

  return <LogoutPanel />;
});
