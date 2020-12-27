function setLocalStorage(key, value) {
  localStorage.setItem(key, value);
}

function removeLocalStorage(key) {
  localStorage.removeItem(key);
}

function clearLocalStorage() {
  localStorage.clear();
}

function getLocalStorageByKey(key) {
  return localStorage.getItem(key);
}

export {
  setLocalStorage,
  removeLocalStorage,
  clearLocalStorage,
  getLocalStorageByKey
};
