import React, { useContext, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { logOut } from "../../services/authService";
import LogoutPanel from "../../components/organisms/Auth/LogoutPanel";
import { SessionContext } from "../../store/context/session-context";
import { PromptLayout } from "../../components/templates/Layout";

export default () => {
  const navigate = useNavigate();
  const { relogin } = useContext(SessionContext);

  useEffect(() => {
    logOut();
    relogin();
  });

  useEffect(() => {
    setTimeout(() => {
      navigate("/");
    }, 1000);
    return () => {
      return clearTimeout();
    };
  });

  return (
    <PromptLayout>
      <LogoutPanel />
    </PromptLayout>
  );
};
