import {
  ApolloClient,
  createHttpLink,
  InMemoryCache,
  ApolloLink,
  RequestHandler,
} from "@apollo/client";
import { setContext } from "@apollo/client/link/context";
import { getAuthenticationToken } from "../../services/AuthLogic";
import errorLink from "./errorLink";
import { apiConfig } from "../../config/api-config";

const contextLink = setContext(async (_, { headers }) => {
  const { authenticationToken } = getAuthenticationToken();
  if (authenticationToken) {
    headers = {
      ...headers,
      Authorization: authenticationToken,
    };
  }

  return {
    headers,
  };
});

const cleanTypeName = new ApolloLink((operation, forward) => {
  if (operation.variables) {
    const omitTypename = (key: any, value: any) =>
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

const httpLink: ApolloLink | RequestHandler = createHttpLink({
  uri: apiConfig.camino_graphql,
  credentials: "include",
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
      FarmModel: {
        keyFields: ["id", "pictures", ["pictureId"]],
      },
      ProductModel: {
        keyFields: ["pictures", ["pictureId"]],
      },
      ArticleModel: {
        keyFields: ["picture", ["pictureId"]],
      },
    },
  }),
  ssrMode: true,
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

export default client;
