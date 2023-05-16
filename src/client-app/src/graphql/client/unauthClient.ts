import { ApolloClient, createHttpLink, InMemoryCache } from "@apollo/client";
import { setContext } from "@apollo/client/link/context";
import { apiConfig } from "../../config/api-config";

const httpLink = createHttpLink({
  uri: apiConfig.camino_graphql,
  credentials: "include",
});

const contextLink = setContext(async (_, { headers }) => {
  return {
    headers,
  };
});

export default new ApolloClient({
  link: contextLink.concat(httpLink),
  cache: new InMemoryCache(),
  ssrMode: true,
  defaultOptions: {
    watchQuery: {
      fetchPolicy: "cache-and-network",
      errorPolicy: "ignore",
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
