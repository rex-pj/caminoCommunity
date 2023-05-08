import { Cookies } from "react-cookie";

export const setStorage = (key, value, options) => {
  const cookies = new Cookies();
  if (!options) {
    cookies.set(key, value);
  } else {
    cookies.set(key, value, options);
  }
};

export const removeStorage = (key, options) => {
  const cookies = new Cookies();
  if (!options) {
    cookies.remove(key);
  } else {
    cookies.remove(key, options);
  }
};

export const getStorageByKey = (key, options) => {
  const cookies = new Cookies();
  if (!options) {
    return cookies.get(key);
  } else {
    return cookies.get(key, options);
  }
};
