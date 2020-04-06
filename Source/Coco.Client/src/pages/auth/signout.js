import React, { useContext } from "react";
import AuthService from "../../services/AuthService";
import { withRouter } from "react-router-dom";
import { useQuery } from "@apollo/react-hooks";
import SignOutPanel from "../../components/organisms/Auth/SignOutPanel";
import { SIGNOUT } from "../../utils/GraphQLQueries";
import { SessionContext } from "../../store/context/SessionContext";

export default withRouter((props) => {
  const { data, loading, error } = useQuery(SIGNOUT);
  const { history } = props;
  const sessionContext = useContext(SessionContext);

  if (loading) {
    return <SignOutPanel />;
  }

  if (error) {
    history.push("/error");
  }

  async function logout() {
    const isCleared = AuthService.logOut();
    if (isCleared) {
      await sessionContext.relogin();
      history.push("/");
    }
  }

  if (data) {
    const { signout } = data;
    if (signout && !!signout.isSucceed) {
      logout();
    } else {
      history.push("/error");
    }
  }

  return <SignOutPanel />;
});
