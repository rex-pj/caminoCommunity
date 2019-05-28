import { AUTH_KEY, AUTH_LOGIN_KEY } from "../utils/AppSettings";
import {
  removeLocalStorage,
  setLocalStorage,
  getLocalStorageByKey
} from "./StorageService";

function removeUserToken() {
  removeLocalStorage(AUTH_KEY);
}

function setUserToken(token) {
  setLocalStorage(AUTH_KEY, token);
}

function getUserToken() {
  return getLocalStorageByKey(AUTH_KEY);
}

function getUserInfo() {
  const tokenkey = getLocalStorageByKey(AUTH_KEY);
  const isLogin = getLocalStorageByKey(AUTH_LOGIN_KEY);

  return {
    isLogin,
    tokenkey
  };
}

function setLogin(token) {
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
