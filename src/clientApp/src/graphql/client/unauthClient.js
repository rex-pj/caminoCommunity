import { ApolloClient, HttpLink, InMemoryCache } from "@apollo/client";
import { setContext } from "apollo-link-context";

const httpLink = new HttpLink({
  uri: process.env.REACT_APP_API_URL,
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
