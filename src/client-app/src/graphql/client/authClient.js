import {
  ApolloClient,
  HttpLink,
  InMemoryCache,
  ApolloLink,
} from "@apollo/client";
import { setContext } from "apollo-link-context";
import { AUTH_KEY, AUTH_USER_HASHED_ID } from "../../utils/AppSettings";

const preloadedState = window.__APOLLO_STORE__;
// Allow the passed state to be garbage-collected
delete window.__APOLLO_STORE__;

const httpLink = new HttpLink({
  uri: process.env.REACT_APP_API_URL,
});

const contextLink = setContext(async (_, { headers }) => {
  const token = localStorage.getItem(AUTH_KEY);
  const userHash = localStorage.getItem(AUTH_USER_HASHED_ID);

  if (token) {
    headers = {
      ...headers,
      "x-header-authentication-token": token,
      "x-header-user-hash": userHash,
    };
  }

  return {
    headers,
  };
});

const cleanTypeName = new ApolloLink((operation, forward) => {
  if (operation.variables) {
    const omitTypename = (key, value) =>
      key === "__typename" ? undefined : value;
    operation.variables = JSON.parse(
      JSON.stringify(operation.variables),
      omitTypename
    );
  }
  return forward(operation).map((data) => {
    return data;
  });
});

const cache = new InMemoryCache({
  typePolicies: {
    UserInfoModel: {
      keyFields: ["userIdentityId"],
    },
    FeedModel: {
      keyFields: ["feedType", "id"],
    },
    SelectOption: {
      keyFields: ["text", "id"],
    },
  },
});

let client = new ApolloClient({
  link: ApolloLink.from([cleanTypeName, contextLink.concat(httpLink)]),
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
