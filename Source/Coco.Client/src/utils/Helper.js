import Alerts from "./LangData/Alerts";

function formatNumber(num) {
  return num.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1.");
}

function langExtract(langData, key, lang) {
  return langData && langData[key] && langData[key][lang]
    ? langData[key][lang]
    : null;
}

function getError(key, lang) {
  return (
    langExtract(Alerts, key, lang) &&
    langExtract(Alerts, "ErrorOccurredTryRefeshInputAgain", lang)
  );
}

const createArray = (from, to, isInvert) => {
  const arr = [];

  if (!!isInvert) {
    for (let index = to; index >= from; index--) {
      arr.push(index);
    }
  } else {
    for (let index = from; index <= to; index++) {
      arr.push(index);
    }
  }

  return arr;
};

const generateDate = data => {
  const { date, month, year } = data;

  if (date && month && year) {
    return new Date(Number(year), Number(month) - 1, Number(date));
  }
  return null;
};

export { formatNumber, langExtract, getError, createArray, generateDate };
