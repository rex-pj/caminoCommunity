import { Cookies } from "react-cookie";
import { CookieSetOptions, CookieGetOptions } from "universal-cookie";

export const setStorage = (
  key: string,
  value: string,
  options?: CookieSetOptions
) => {
  const cookies = new Cookies();
  if (!options) {
    cookies.set(key, value);
  } else {
    cookies.set(key, value, options);
  }
};

export const removeStorage = (key: string, options?: CookieSetOptions) => {
  const cookies = new Cookies();
  if (!options) {
    cookies.remove(key);
  } else {
    cookies.remove(key, options);
  }
};

export const getStorageByKey = (key: string, options?: CookieGetOptions) => {
  const cookies = new Cookies();
  if (!options) {
    return cookies.get(key);
  }

  return cookies.get(key, options);
};
