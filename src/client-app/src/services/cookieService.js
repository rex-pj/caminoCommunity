import { Cookies } from "react-cookie";

function setStorage(key, value, options) {
  const cookies = new Cookies();
  if (!options) {
    cookies.set(key, value);
  } else {
    cookies.set(key, value, options);
  }
}

function removeStorage(key, options) {
  const cookies = new Cookies();
  if (!options) {
    cookies.remove(key);
  } else {
    cookies.remove(key, options);
  }
}

function getStorageByKey(key, options) {
  const cookies = new Cookies();
  if (!options) {
    return cookies.get(key);
  } else {
    return cookies.get(key, options);
  }
}

export default { setStorage, removeStorage, getStorageByKey };
