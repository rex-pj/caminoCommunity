import {
  AUTH_KEY,
  AUTH_LOGIN_KEY,
  AUTH_DISPLAY_NAME,
  AUTH_USER_HASHED_ID,
  AUTH_USER_LANGUAGE
} from "../utils/AppSettings";
import {
  removeLocalStorage,
  setLocalStorage,
  getLocalStorageByKey
} from "./StorageService";
import { identityClient } from "../utils/GraphQLClient";
import { GET_LOGGED_USER } from "../utils/GraphQLQueries";

function removeUserToken() {
  removeLocalStorage(AUTH_KEY);
}

function setUserToken(token) {
  setLocalStorage(AUTH_KEY, token);
}

function getUserToken() {
  return getLocalStorageByKey(AUTH_KEY);
}

async function getLoggedUserInfo() {
  const tokenkey = getLocalStorageByKey(AUTH_KEY);
  const isLogin = getLocalStorageByKey(AUTH_LOGIN_KEY);
  const userIdentityId = getLocalStorageByKey(AUTH_USER_HASHED_ID);
  let userLanguage = getLocalStorageByKey(AUTH_USER_LANGUAGE);

  userLanguage = userLanguage ? userLanguage : "vn";

  let currentUser = {
    isLogin,
    tokenkey,
    userIdentityId,
    userLanguage
  };

  await identityClient
    .query({
      query: GET_LOGGED_USER
    })
    .then(response => {
      const { data } = response;
      const { loggedUser } = data;

      currentUser = {
        ...currentUser,
        ...loggedUser
      };
    })
    .catch(error => {
      console.log(error);
    });

  return currentUser;
}

function parseUserInfo(response) {
  const isLogin = getLocalStorageByKey(AUTH_LOGIN_KEY);
  let userLanguage = getLocalStorageByKey(AUTH_USER_LANGUAGE);

  userLanguage = userLanguage ? userLanguage : "vn";

  let currentUser = {
    isLogin,
    userLanguage
  };

  if (response) {
    const { loggedUser } = response;

    currentUser = {
      ...currentUser,
      ...loggedUser
    };
  }

  return currentUser;
}

const setLogin = (userInfo, token) => {
  if (userInfo) {
    setLocalStorage(AUTH_DISPLAY_NAME, userInfo.displayName);
    setLocalStorage(AUTH_USER_HASHED_ID, userInfo.userIdentityId);
  }

  setUserToken(token);
  setLocalStorage(AUTH_LOGIN_KEY, true);
};

function logOut() {
  removeUserToken();
  removeLocalStorage(AUTH_LOGIN_KEY);
  removeLocalStorage(AUTH_DISPLAY_NAME);
  removeLocalStorage(AUTH_USER_HASHED_ID);
}

export default {
  removeUserToken,
  setUserToken,
  getUserToken,
  getLoggedUserInfo,
  setLogin,
  logOut,
  parseUserInfo
};
