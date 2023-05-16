import {
  ACCESS_TOKEN,
  USER_LANGUAGE,
  AUTH_REFRESH_TOKEN_EXPIRATION,
  AUTH_IS_REMEMBER,
} from "../utils/AppSettings";
import * as localStorageUtils from "../utils/localStorageUtils";
import * as cookieUtils from "../utils/cookieUtils";
import jwtDecode, { JwtPayload } from "jwt-decode";

export const checkRemember = () => {
  const isRemember = localStorageUtils.getStorageByKey(AUTH_IS_REMEMBER);
  return isRemember === "true";
};

export const getUserToken = (isRemember: boolean): string => {
  if (isRemember) {
    return localStorageUtils.getStorageByKey(ACCESS_TOKEN) ?? "";
  }
  return cookieUtils.getStorageByKey(ACCESS_TOKEN);
};

export interface IAuthenticationToken {
  authenticationToken: string;
  refreshTokenExpiryTime: string;
}

export const parseUserSession = (response: any) => {
  const isLogin = isTokenValid();
  const userLanguage = localStorageUtils.getStorageByKey(USER_LANGUAGE);
  const language = userLanguage ? userLanguage : "vn";

  if (!response) {
    return {
      isLogin,
      userLanguage: language,
    };
  }

  const { currentUser, userPhotos } = response;
  const avatar = userPhotos?.find((item: any) => item.photoType === "AVATAR");
  const cover = userPhotos?.find((item: any) => item.photoType === "COVER");
  return {
    currentUser: {
      ...currentUser,
      userAvatar: avatar ?? {},
      userCover: cover ?? {},
    },
    isLogin,
    lang: language,
  };
};

export const setLogin = (tokenData: any, isRember: boolean) => {
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
  localStorageUtils.setStorage(AUTH_IS_REMEMBER, isRember.toString());
};

export const getAuthenticationToken = (): IAuthenticationToken => {
  const isRemember = checkRemember();
  const authenticationToken = getUserToken(isRemember);
  if (!authenticationToken) {
    return {
      authenticationToken: "",
      refreshTokenExpiryTime: "",
    };
  }

  if (isRemember) {
    const refreshTokenExpiryTime = localStorageUtils.getStorageByKey(
      AUTH_REFRESH_TOKEN_EXPIRATION
    );
    return {
      authenticationToken,
      refreshTokenExpiryTime: refreshTokenExpiryTime ?? "",
    };
  }

  const refreshTokenExpiryTime = cookieUtils.getStorageByKey(
    AUTH_REFRESH_TOKEN_EXPIRATION
  );
  return {
    authenticationToken,
    refreshTokenExpiryTime: refreshTokenExpiryTime ?? "",
  };
};

export const isTokenValid = () => {
  const isRemember = checkRemember();
  const token = getUserToken(isRemember);
  if (!token) {
    return false;
  }

  const decoded: JwtPayload = jwtDecode(token);
  if (!decoded || !decoded.exp) {
    return false;
  }
  return decoded.exp >= Date.now() / 1000;
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
