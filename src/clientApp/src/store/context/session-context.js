import React from "react";

export const initialSession = {
  isLogin: false,
  lang: "vn",
  currentUser: {},
  isLoading: true,
  login: () => {},
  relogin: async (data) => {},
};

export const SessionContext = React.createContext(initialSession);
