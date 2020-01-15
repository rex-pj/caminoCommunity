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

const removeUserToken = () => {
  removeLocalStorage(AUTH_KEY);
};

const setUserToken = token => {
  setLocalStorage(AUTH_KEY, token);
};

const getUserToken = () => {
  return getLocalStorageByKey(AUTH_KEY);
};

const parseUserInfo = response => {
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
};

const setLogin = (userInfo, token) => {
  if (userInfo) {
    setLocalStorage(AUTH_DISPLAY_NAME, userInfo.displayName);
    setLocalStorage(AUTH_USER_HASHED_ID, userInfo.userIdentityId);
  }

  setUserToken(token);
  setLocalStorage(AUTH_LOGIN_KEY, true);
};

const logOut = () => {
  removeUserToken();
  removeLocalStorage(AUTH_LOGIN_KEY);
  removeLocalStorage(AUTH_DISPLAY_NAME);
  removeLocalStorage(AUTH_USER_HASHED_ID);
};

export default {
  removeUserToken,
  setUserToken,
  getUserToken,
  setLogin,
  logOut,
  parseUserInfo
};
