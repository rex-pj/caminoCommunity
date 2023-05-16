export const setStorage = (key: string, value: string) => {
  localStorage.setItem(key, value);
};

export const removeStorage = (key: string) => {
  localStorage.removeItem(key);
};

export const getStorageByKey = (key: string) => {
  return localStorage.getItem(key);
};
