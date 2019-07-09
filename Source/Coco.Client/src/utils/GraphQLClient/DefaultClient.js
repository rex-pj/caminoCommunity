import { ApolloClient } from "apollo-client";
import { createHttpLink } from "apollo-link-http";
import { setContext } from "apollo-link-context";
import { InMemoryCache } from "apollo-cache-inmemory";
import { AUTH_KEY, AUTH_USER_HASHED_ID } from "../AppSettings";

const httpLink = createHttpLink({
  uri: process.env.REACT_APP_PUBLIC_API_URL
});

const contextLink = setContext(async (_, { headers }) => {
  const token = localStorage.getItem(AUTH_KEY);
  const authorization = token ? token : "";
  const userHash = localStorage.getItem(AUTH_USER_HASHED_ID);

  if (authorization) {
    headers = {
      ...headers,
      authorization
    };
  }

  if (userHash) {
    headers = {
      ...headers,
      "x-header-user-hash": userHash
    };
  }
  return {
    headers
  };
});

const defaultClient = new ApolloClient({
  link: contextLink.concat(httpLink),
  cache: new InMemoryCache(),
  ssrMode: true,
  defaultOptions: {
    watchQuery: {
      fetchPolicy: "cache-and-network",
      errorPolicy: "ignore"
    },
    query: {
      fetchPolicy: "network-only",
      errorPolicy: "all"
    },
    mutate: {
      errorPolicy: "all"
    }
  }
});

export default defaultClient;
