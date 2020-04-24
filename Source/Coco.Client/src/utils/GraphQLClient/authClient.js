import { ApolloClient } from "apollo-client";
import { createHttpLink } from "apollo-link-http";
import { setContext } from "apollo-link-context";
import { InMemoryCache } from "apollo-cache-inmemory";
import { AUTH_KEY, AUTH_USER_HASHED_ID } from "../AppSettings";

const preloadedState = window.__APOLLO_STORE__;
// Allow the passed state to be garbage-collected
delete window.__APOLLO_STORE__;

const httpLink = createHttpLink({
  uri: process.env.REACT_APP_AUTH_API_URL,
});

const contextLink = setContext(async (_, { headers }) => {
  const token = localStorage.getItem(AUTH_KEY);
  const userHash = localStorage.getItem(AUTH_USER_HASHED_ID);

  if (token) {
    headers = {
      ...headers,
      authorization: token,
      "x-header-user-hash": userHash,
    };
  }

  return {
    headers,
  };
});

let client = new ApolloClient({
  link: contextLink.concat(httpLink),
  cache: new InMemoryCache(),
  ssrMode: true,
  initialState: preloadedState,
  defaultOptions: {
    watchQuery: {
      fetchPolicy: "cache-and-network",
    },
    query: {
      fetchPolicy: "network-only",
      errorPolicy: "all",
    },
    mutate: {
      errorPolicy: "all",
    },
  },
});

// Tell react-snap how to save state
window.snapSaveState = () => ({
  __APOLLO_STORE__: client.store.getCache(),
});

export default client;
