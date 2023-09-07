import * as React from "react";
import { useEffect, useContext } from "react";
import { useNavigate } from "react-router-dom";
import { logOut } from "../../services/AuthLogic";
import LogoutPanel from "../../components/organisms/Auth/LogoutPanel";
import { SessionContext } from "../../store/context/session-context";
import { PromptLayout } from "../../components/templates/Layout";
import { Helmet } from "react-helmet-async";

const Logout = () => {
  const navigate = useNavigate();
  const { relogin } = useContext(SessionContext);

  useEffect(() => {
    logOut();
    relogin();
  });

  useEffect(() => {
    const timeoutId = setTimeout(() => {
      navigate("/");
    }, 1000);
    return () => {
      return clearTimeout(timeoutId);
    };
  });

  return (
    <>
      <Helmet>
        <meta name="robots" content="noindex,nofollow" />
      </Helmet>
      <PromptLayout>
        <LogoutPanel />
      </PromptLayout>
    </>
  );
};

export default Logout;
