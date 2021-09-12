import {
  ApolloClient,
  createHttpLink,
  InMemoryCache,
  ApolloLink,
} from "@apollo/client";
import { setContext } from "apollo-link-context";
import { getAuthenticationToken } from "../../services/authService";
import errorLink from "./errorLink";

const preloadedState = window.__APOLLO_STORE__;
// Allow the passed state to be garbage-collected
delete window.__APOLLO_STORE__;

const httpLink = createHttpLink({
  uri: process.env.REACT_APP_API_URL,
  credentials: "include",
});

const contextLink = setContext(async (_, { headers }) => {
  const { authenticationToken } = getAuthenticationToken();
  if (authenticationToken) {
    headers = {
      ...headers,
      "x-header-authentication-access-token": authenticationToken,
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

const client = new ApolloClient({
  link: ApolloLink.from([
    cleanTypeName,
    errorLink,
    contextLink.concat(httpLink),
  ]),
  cache: new InMemoryCache({
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
  }),
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
