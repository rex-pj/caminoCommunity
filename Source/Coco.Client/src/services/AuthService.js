import {
  AUTH_KEY,
  AUTH_LOGIN_KEY,
  AUTH_DISPLAY_NAME,
  AUTH_USER_HASHED_ID
} from "../utils/AppSettings";
import {
  removeLocalStorage,
  setLocalStorage,
  getLocalStorageByKey
} from "./StorageService";
// import { authorizedClient } from "../utils/GraphQL/GraphQLClient";
import { GET_LOGGED_USER } from "../utils/GraphQL/GraphQLQueries";

function removeUserToken() {
  removeLocalStorage(AUTH_KEY);
}

function setUserToken(token) {
  setLocalStorage(AUTH_KEY, token);
}

function getUserToken() {
  return getLocalStorageByKey(AUTH_KEY);
}

const getUserInfo = async () => {
  const tokenkey = getLocalStorageByKey(AUTH_KEY);
  const isLogin = getLocalStorageByKey(AUTH_LOGIN_KEY);
  const displayName = getLocalStorageByKey(AUTH_DISPLAY_NAME);
  const userHashedId = getLocalStorageByKey(AUTH_USER_HASHED_ID);

  // await authorizedClient
  //   .query({
  //     query: GET_LOGGED_USER
  //   })
  //   .then(result => {
  //     console.log(result);
  //   })
  //   .catch(error => {
  //     console.log(error);
  //   });

  return {
    isLogin,
    tokenkey,
    displayName,
    userHashedId
  };
};

function setLogin(userInfo, token) {
  if (userInfo) {
    setLocalStorage(AUTH_DISPLAY_NAME, userInfo.displayName);
    setLocalStorage(AUTH_USER_HASHED_ID, userInfo.userHashedId);
  }

  setUserToken(token);
  setLocalStorage(AUTH_LOGIN_KEY, true);
}

function logOut() {
  removeUserToken();
  removeLocalStorage(AUTH_LOGIN_KEY);
}

export {
  removeUserToken,
  setUserToken,
  getUserToken,
  getUserInfo,
  setLogin,
  logOut
};
