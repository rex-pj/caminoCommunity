import {
  AUTH_KEY,
  AUTH_USER_LANGUAGE,
  AUTH_REFRESH_TOKEN_KEY,
  AUTH_REFRESH_TOKEN_EXPIRATION_KEY,
} from "../utils/AppSettings";
import {
  removeLocalStorage,
  setLocalStorage,
  getLocalStorageByKey,
} from "./storageService";
import jwtDecode from "jwt-decode";

export const getUserToken = () => {
  return getLocalStorageByKey(AUTH_KEY);
};

export const parseUserInfo = (response) => {
  const isLogin = isTokenValid();
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

export const setLogin = (tokenData) => {
  const { authenticationToken, refreshToken, refreshTokenExpiryTime } =
    tokenData;
  setLocalStorage(AUTH_KEY, authenticationToken);
  setLocalStorage(AUTH_REFRESH_TOKEN_KEY, refreshToken);
  setLocalStorage(AUTH_REFRESH_TOKEN_EXPIRATION_KEY, refreshTokenExpiryTime);
};

export const getAuthenticationToken = () => {
  const authenticationToken = getUserToken();
  const refreshToken = getLocalStorageByKey(AUTH_REFRESH_TOKEN_KEY);
  if (!authenticationToken || !refreshToken) {
    return null;
  }

  const refreshTokenExpiryTime = getLocalStorageByKey(
    AUTH_REFRESH_TOKEN_EXPIRATION_KEY
  );
  return { authenticationToken, refreshToken, refreshTokenExpiryTime };
};

export const isTokenValid = () => {
  const token = getUserToken();
  if (!token) {
    return false;
  }
  return jwtDecode(token).exp >= Date.now() / 1000;
};

export const logOut = () => {
  removeLocalStorage(AUTH_KEY);
  removeLocalStorage(AUTH_REFRESH_TOKEN_KEY);
  removeLocalStorage(AUTH_REFRESH_TOKEN_EXPIRATION_KEY);
  return true;
};
