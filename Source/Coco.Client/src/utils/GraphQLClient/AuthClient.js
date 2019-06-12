import { ApolloClient } from "apollo-client";
import { createHttpLink } from "apollo-link-http";
import { setContext } from "apollo-link-context";
import { InMemoryCache } from "apollo-cache-inmemory";
import { AUTH_KEY, AUTH_USER_HASHED_ID } from "../AppSettings";

const preloadedState = window.__APOLLO_STORE__;
// Allow the passed state to be garbage-collected
delete window.__APOLLO_STORE__;

const authHttpLink = createHttpLink({
  uri: process.env.REACT_APP_AUTH_API_URL
});

const authLink = setContext(async (_, { headers }) => {
  const token = localStorage.getItem(AUTH_KEY);
  const authorization = token ? token : "";
  const userHash = localStorage.getItem(AUTH_USER_HASHED_ID);
  return {
    headers: {
      ...headers,
      authorization,
      "x-header-user-hash": userHash
    }
  };
});

let authClient = new ApolloClient({
  link: authLink.concat(authHttpLink),
  cache: new InMemoryCache(),
  ssrMode: true,
  initialState: preloadedState,
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

// Tell react-snap how to save state
window.snapSaveState = () => ({
  __APOLLO_STORE__: authClient.store.getCache()
});

export default authClient;
