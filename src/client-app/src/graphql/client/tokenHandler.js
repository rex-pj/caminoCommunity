import { getAuthenticationToken } from "../../services/AuthLogic";
import { apiConfig } from "../../config/api-config";

const refreshTokenMutation = (tokens) => {
  return JSON.stringify({
    query: `mutation refreshToken {
          refreshToken {
            authenticationToken
            refreshTokenExpiryTime
          }
        }`,
  });
};

export const getNewTokens = async () => {
  return new Promise(function (resolve, reject) {
    const { authenticationToken } = getAuthenticationToken();

    // TODO get back || !refreshToken
    if (!authenticationToken) {
      reject({});
      return;
    }

    fetch(apiConfig.camino_graphql, {
      headers: {
        "content-type": "application/json",
        "x-header-authentication-access-token": authenticationToken,
      },
      credentials: "include",
      method: "POST",
      body: refreshTokenMutation(),
    })
      .then((result) => {
        result
          .text()
          .then(JSON.parse)
          .then((response) => {
            const {
              data: { refreshToken: refreshTokenResponse },
              errors,
            } = response;

            if (!errors && refreshTokenResponse) {
              resolve(refreshTokenResponse);
            } else if (errors) {
              reject(errors);
            }
          })
          .catch((errors) => {
            reject(errors);
          });
      })
      .catch((errors) => {
        reject(errors);
      });
  });
};
