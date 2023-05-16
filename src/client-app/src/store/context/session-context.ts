import { ApolloQueryResult } from "@apollo/client";
import * as React from "react";

interface LoginSesstion {
  isLogin: boolean;
  lang: string;
  currentUser?: {
    userIdentityId?: string;
  };
  isLoading?: boolean;
  login?: (data: any) => void;
  relogin: (data?: any) => Promise<any>;
}

export const initialSession: LoginSesstion = {
  isLogin: false,
  lang: "vn",
  currentUser: {},
  isLoading: true,
  login: () => {},
  relogin: () => Promise.resolve(),
};

export const SessionContext = React.createContext(initialSession);
