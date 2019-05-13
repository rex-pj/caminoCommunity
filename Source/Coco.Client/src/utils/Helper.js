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

export { formatNumber, langExtract, getError };
