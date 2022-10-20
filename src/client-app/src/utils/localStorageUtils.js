export const setStorage = (key, value) => {
  localStorage.setItem(key, value);
};

export const removeStorage = (key) => {
  localStorage.removeItem(key);
};

export const getStorageByKey = (key) => {
  return localStorage.getItem(key);
};
