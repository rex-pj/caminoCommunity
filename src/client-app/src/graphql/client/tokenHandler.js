import { getLocalStorageByKey } from "../../services/storageService";
import { AUTH_KEY, AUTH_REFRESH_TOKEN_KEY } from "../../utils/AppSettings";

const refreshTokenMutation = (tokens) => {
  const { authenticationToken, refreshToken } = tokens;
  return JSON.stringify({
    variables: {
      criterias: {
        authenticationToken,
        refreshToken,
      },
    },
    query: `mutation refreshToken($criterias: RefreshTokenModelInput!) {
          refreshToken(criterias: $criterias) {
            authenticationToken
            refreshToken
            refreshTokenExpiryTime
          }
        }`,
  });
};

export const getAccessToken = () => {
  return getLocalStorageByKey(AUTH_KEY);
};

export const getRefreshToken = () => {
  return getLocalStorageByKey(AUTH_REFRESH_TOKEN_KEY);
};

export const getNewTokens = async () => {
  return new Promise(function (resolve, reject) {
    const accessToken = getAccessToken();
    const refreshToken = getRefreshToken();
    if (!accessToken || !refreshToken) {
      reject({});
      return;
    }
    fetch(process.env.REACT_APP_API_URL, {
      headers: {
        "content-type": "application/json",
        "x-header-authentication-token": accessToken,
      },
      method: "POST",
      body: refreshTokenMutation({
        refreshToken: refreshToken,
      }),
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
