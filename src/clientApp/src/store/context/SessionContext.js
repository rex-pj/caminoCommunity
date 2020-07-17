import React from "react";

export const initialSession = {
  isLogin: false,
  authenticationToken: null,
  lang: "vn",
  user: {},
  isLoading: true,
  login: () => {},
  relogin: async data => {}
};

export const SessionContext = React.createContext(initialSession);
