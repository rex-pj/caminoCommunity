import { fromPromise } from "@apollo/client";
import { onError } from "@apollo/client/link/error";
import { getNewTokens } from "./tokenHandler";
import { setLogin, getAuthenticationToken } from "../../services/authService";

let isRefreshing = false;
let pendingRequests = [];

const resolvePendingRequests = () => {
  pendingRequests.map((callback) => callback());
  pendingRequests = [];
};

const refreshToken = () => {
  return getNewTokens()
    .then((refreshToken) => {
      resolvePendingRequests();
      if (refreshToken) {
        setLogin(refreshToken);
        return refreshToken.authenticationToken;
      }

      pendingRequests = [];
    })
    .catch(() => {
      pendingRequests = [];
      return;
    })
    .finally(() => {
      isRefreshing = false;
    });
};

export default onError(({ graphQLErrors, operation, forward }) => {
  if (!graphQLErrors) {
    return;
  }

  const unAuthenticationError = graphQLErrors.find(
    (x) => x.extensions && x.extensions.code === "403"
  );

  if (!unAuthenticationError) {
    return;
  }

  var tokens = getAuthenticationToken();
  if (!tokens && !tokens.refreshTokenExpiryTime) {
    return;
  }

  const refreshTokenExpiryTime = new Date(tokens.refreshTokenExpiryTime);
  if (refreshTokenExpiryTime <= new Date() || isRefreshing) {
    return;
  }

  isRefreshing = true;
  return fromPromise(refreshToken()).flatMap(() => forward(operation));
});
