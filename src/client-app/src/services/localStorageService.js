function setStorage(key, value) {
  localStorage.setItem(key, value);
}

function removeStorage(key) {
  localStorage.removeItem(key);
}

function getStorageByKey(key) {
  return localStorage.getItem(key);
}

export default { setStorage, removeStorage, getStorageByKey };
