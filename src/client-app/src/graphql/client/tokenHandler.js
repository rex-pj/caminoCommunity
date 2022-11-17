import { getAuthenticationToken } from "../../services/AuthLogic";
import AuthService from "../../services/authService";

export const getNewTokens = async () => {
  const authService = new AuthService();
  return new Promise(function (resolve, reject) {
    const { authenticationToken } = getAuthenticationToken();

    if (!authenticationToken) {
      reject({});
      return;
    }

    authService
      .refreshToken()
      .then((response) => {
        const { data } = response;
        resolve(data);
      })
      .catch((errors) => {
        reject(errors);
      });
  });
};
