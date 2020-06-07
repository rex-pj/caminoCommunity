import { ApolloClient, HttpLink, InMemoryCache } from "@apollo/client";
import introspectionResult from "../GraphQLQueries/introspection-result";
import { setContext } from "apollo-link-context";
import { AUTH_KEY, AUTH_USER_HASHED_ID } from "../AppSettings";

const preloadedState = window.__APOLLO_STORE__;
// Allow the passed state to be garbage-collected
delete window.__APOLLO_STORE__;

const httpLink = new HttpLink({
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

const cache = new InMemoryCache({
  possibleTypes: introspectionResult.possibleTypes,
});

let client = new ApolloClient({
  link: contextLink.concat(httpLink),
  cache: cache,
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
