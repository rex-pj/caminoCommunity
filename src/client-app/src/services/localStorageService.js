const setStorage = (key, value) => {
  localStorage.setItem(key, value);
};

const removeStorage = (key) => {
  localStorage.removeItem(key);
};

const getStorageByKey = (key) => {
  return localStorage.getItem(key);
};

export default { setStorage, removeStorage, getStorageByKey };
