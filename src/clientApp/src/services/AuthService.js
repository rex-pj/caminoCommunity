import {
  AUTH_KEY,
  AUTH_LOGIN_KEY,
  AUTH_USER_HASHED_ID,
  AUTH_USER_LANGUAGE,
} from "../utils/AppSettings";
import {
  removeLocalStorage,
  setLocalStorage,
  getLocalStorageByKey,
} from "./StorageService";

const removeUserToken = () => {
  removeLocalStorage(AUTH_KEY);
};

const setUserToken = (token) => {
  setLocalStorage(AUTH_KEY, token);
};

const getUserToken = () => {
  return getLocalStorageByKey(AUTH_KEY);
};

const parseUserInfo = (response) => {
  const isLogin = getLocalStorageByKey(AUTH_LOGIN_KEY);
  let userLanguage = getLocalStorageByKey(AUTH_USER_LANGUAGE);

  userLanguage = userLanguage ? userLanguage : "vn";

  if (response) {
    const { currentUser, userPhotos } = response;

    let userAvatar = {};
    let userCover = {};
    if (userPhotos && userPhotos.length > 0) {
      const avatar = userPhotos.find((item) => item.photoType === "AVATAR");
      if (avatar) {
        userAvatar = avatar;
      }
      const cover = userPhotos.find((item) => item.photoType === "COVER");
      if (cover) {
        userCover = cover;
      }
    }
    return {
      ...currentUser,
      userAvatar,
      userCover,
      isLogin,
      userLanguage,
    };
  }
  return {
    isLogin,
    userLanguage,
  };
};

const setLogin = (userInfo, token) => {
  if (userInfo) {
    setLocalStorage(AUTH_USER_HASHED_ID, userInfo.userIdentityId);
  }

  setUserToken(token);
  setLocalStorage(AUTH_LOGIN_KEY, true);
};

const logOut = () => {
  removeUserToken();
  removeLocalStorage(AUTH_LOGIN_KEY);
  removeLocalStorage(AUTH_USER_HASHED_ID);
  return true;
};

export default {
  removeUserToken,
  setUserToken,
  getUserToken,
  setLogin,
  logOut,
  parseUserInfo,
};
