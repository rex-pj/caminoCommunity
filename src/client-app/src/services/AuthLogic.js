import {
  ACCESS_TOKEN,
  USER_LANGUAGE,
  AUTH_REFRESH_TOKEN_EXPIRATION,
  AUTH_IS_REMEMBER,
} from "../utils/AppSettings";
import * as localStorageUtils from "../utils/localStorageUtils";
import * as cookieUtils from "../utils/cookieUtils";
import jwtDecode from "jwt-decode";

export const checkRemember = () => {
  const isRemember = localStorageUtils.getStorageByKey(AUTH_IS_REMEMBER);
  return isRemember === true || isRemember === "true";
};

export const getUserToken = (isRemember) => {
  if (isRemember) {
    return localStorageUtils.getStorageByKey(ACCESS_TOKEN);
  }
  return cookieUtils.getStorageByKey(ACCESS_TOKEN, { path: "/" });
};

export const parseUserInfo = (response) => {
  const isLogin = isTokenValid();
  let userLanguage = localStorageUtils.getStorageByKey(USER_LANGUAGE);

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
    localStorageUtils.setStorage(ACCESS_TOKEN, authenticationToken);
    localStorageUtils.setStorage(
      AUTH_REFRESH_TOKEN_EXPIRATION,
      refreshTokenExpiryTime
    );
  } else {
    cookieUtils.setStorage(ACCESS_TOKEN, authenticationToken, {
      secure: true,
      sameSite: "strict",
      path: "/",
    });
    cookieUtils.setStorage(
      AUTH_REFRESH_TOKEN_EXPIRATION,
      refreshTokenExpiryTime,
      {
        secure: true,
        sameSite: "strict",
        path: "/",
      }
    );
  }
  localStorageUtils.setStorage(AUTH_IS_REMEMBER, !!isRember);
};

export const getAuthenticationToken = () => {
  const isRemember = checkRemember();
  const authenticationToken = getUserToken(isRemember);
  if (isRemember) {
    if (!authenticationToken) {
      return {};
    }

    const refreshTokenExpiryTime = localStorageUtils.getStorageByKey(
      AUTH_REFRESH_TOKEN_EXPIRATION
    );
    return { authenticationToken, refreshTokenExpiryTime };
  } else {
    if (!authenticationToken) {
      return {};
    }

    const refreshTokenExpiryTime = cookieUtils.getStorageByKey(
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
    localStorageUtils.removeStorage(ACCESS_TOKEN);
    localStorageUtils.removeStorage(AUTH_REFRESH_TOKEN_EXPIRATION);
  } else {
    cookieUtils.removeStorage(ACCESS_TOKEN, { path: "/" });
    cookieUtils.removeStorage(AUTH_REFRESH_TOKEN_EXPIRATION, {
      path: "/",
    });
  }
};
