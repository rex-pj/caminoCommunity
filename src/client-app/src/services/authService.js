import {
  ACCESS_TOKEN,
  USER_LANGUAGE,
  AUTH_REFRESH_TOKEN_EXPIRATION,
  AUTH_IS_REMEMBER,
} from "../utils/AppSettings";
import storageService from "./localStorageService";
import cookieService from "./cookieService";
import jwtDecode from "jwt-decode";

export const checkRemember = () => {
  const isRemember = storageService.getStorageByKey(AUTH_IS_REMEMBER);
  return isRemember === true || isRemember === "true";
};

export const getUserToken = (isRemember) => {
  if (isRemember) {
    return storageService.getStorageByKey(ACCESS_TOKEN);
  }
  return cookieService.getStorageByKey(ACCESS_TOKEN, { path: "/" });
};

export const parseUserInfo = (response) => {
  const isLogin = isTokenValid();
  let userLanguage = storageService.getStorageByKey(USER_LANGUAGE);

  if (!response) {
    return {
      isLogin,
      userLanguage: userLanguage ? userLanguage : "vn",
    };
  }

  const { currentUser, userPhotos } = response;
  if (!userPhotos || userPhotos.length === 0) {
    return {
      ...currentUser,
      userAvatar: {},
      userCover: {},
      isLogin,
      userLanguage,
    };
  }

  const avatar = userPhotos.find((item) => item.photoType === "AVATAR");
  const cover = userPhotos.find((item) => item.photoType === "COVER");
  return {
    ...currentUser,
    userAvatar: avatar ? avatar : {},
    userCover: cover ? cover : {},
    isLogin,
    userLanguage,
  };
};

export const setLogin = (tokenData, isRember) => {
  logOut();
  const { authenticationToken, refreshTokenExpiryTime } = tokenData;
  if (isRember) {
    storageService.setStorage(ACCESS_TOKEN, authenticationToken);
    storageService.setStorage(
      AUTH_REFRESH_TOKEN_EXPIRATION,
      refreshTokenExpiryTime
    );
  } else {
    cookieService.setStorage(ACCESS_TOKEN, authenticationToken, {
      secure: true,
      sameSite: "strict",
      path: "/",
    });
    cookieService.setStorage(
      AUTH_REFRESH_TOKEN_EXPIRATION,
      refreshTokenExpiryTime,
      {
        secure: true,
        sameSite: "strict",
        path: "/",
      }
    );
  }
  storageService.setStorage(AUTH_IS_REMEMBER, !!isRember);
};

export const getAuthenticationToken = () => {
  const isRemember = checkRemember();
  const authenticationToken = getUserToken(isRemember);
  if (isRemember) {
    if (!authenticationToken) {
      return {};
    }

    const refreshTokenExpiryTime = storageService.getStorageByKey(
      AUTH_REFRESH_TOKEN_EXPIRATION
    );
    return { authenticationToken, refreshTokenExpiryTime };
  } else {
    if (!authenticationToken) {
      return {};
    }

    const refreshTokenExpiryTime = cookieService.getStorageByKey(
      AUTH_REFRESH_TOKEN_EXPIRATION
    );
    return { authenticationToken, refreshTokenExpiryTime };
  }
};

export const isTokenValid = () => {
  const isRemember = checkRemember();
  const token = getUserToken(isRemember);
  if (!token) {
    return false;
  }
  return jwtDecode(token).exp >= Date.now() / 1000;
};

export const logOut = () => {
  const isRemember = checkRemember();
  if (isRemember) {
    storageService.removeStorage(ACCESS_TOKEN);
    storageService.removeStorage(AUTH_REFRESH_TOKEN_EXPIRATION);
  } else {
    cookieService.removeStorage(ACCESS_TOKEN, { path: "/" });
    cookieService.removeStorage(AUTH_REFRESH_TOKEN_EXPIRATION, {
      path: "/",
    });
  }
};
