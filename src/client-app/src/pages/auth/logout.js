import React, { useContext, useEffect } from "react";
import { useHistory } from "react-router-dom";
import AuthService from "../../services/authService";
import { withRouter } from "react-router-dom";
import LogoutPanel from "../../components/organisms/Auth/LogoutPanel";
import { SessionContext } from "../../store/context/session-context";

export default withRouter(() => {
  const history = useHistory();
  const { relogin } = useContext(SessionContext);

  useEffect(() => {
    AuthService.logOut();
    relogin();
  });

  useEffect(() => {
    setTimeout(() => {
      history.push("/");
    }, 1000);
    return () => {
      return clearTimeout();
    };
  });

  return <LogoutPanel />;
});
