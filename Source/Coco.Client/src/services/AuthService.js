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
import { authClient } from "../utils/GraphQLClient";
import { GET_LOGGED_USER, GET_FULL_USER_INFO } from "../utils/GraphQLQueries";

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
  const userHashedId = getLocalStorageByKey(AUTH_USER_HASHED_ID);

  let currentUser = {
    isLogin,
    tokenkey,
    userHashedId
  };

  await authClient
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

async function getFullUserInfo(userHashedId) {
  const tokenkey = getLocalStorageByKey(AUTH_KEY);
  const isLogin = getLocalStorageByKey(AUTH_LOGIN_KEY);
  userHashedId = userHashedId
    ? userHashedId
    : getLocalStorageByKey(AUTH_USER_HASHED_ID);

  let currentUser = {
    isLogin,
    tokenkey,
    userHashedId
  };

  await authClient
    .query({
      query: GET_FULL_USER_INFO
    })
    .then(response => {
      const { data } = response;
      const { fullUserInfo } = data;

      currentUser = {
        ...currentUser,
        ...fullUserInfo
      };
    })
    .catch(error => {
      console.log(error);
    });

  return currentUser;
}

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
  removeLocalStorage(AUTH_DISPLAY_NAME);
  removeLocalStorage(AUTH_USER_HASHED_ID);
}

export default {
  removeUserToken,
  setUserToken,
  getUserToken,
  getLoggedUserInfo,
  getFullUserInfo,
  setLogin,
  logOut
};
