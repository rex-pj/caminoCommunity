import Alerts from "./LangData/Alerts";

export function langExtract(langData: any, key: string, lang: string) {
  return langData && langData[key] && langData[key][lang]
    ? langData[key][lang]
    : null;
}

export function getError(key: string, lang: string) {
  return (
    langExtract(Alerts, key, lang) &&
    langExtract(Alerts, "ErrorOccurredTryRefeshInputAgain", lang)
  );
}

export const createArray = (from: number, to: number, isInvert?: boolean) => {
  const arr = [];

  if (isInvert) {
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

export const generateDate = (data: {
  date: string;
  month: string;
  year: string;
}) => {
  const { date, month, year } = data;

  if (date && month && year) {
    return new Date(Number(year), Number(month) - 1, Number(date));
  }
  return null;
};

export const base64toFile = (base64: string, filename: string) => {
  const arr = base64.split(",");
  const mime = arr[0].match(/:(.*?);/)[1];
  const byteString = atob(arr[1]);
  let length = byteString.length;
  let unitBuffers = new Uint8Array(length);

  while (length--) {
    unitBuffers[length] = byteString.charCodeAt(length);
  }

  return new File([unitBuffers], filename, { type: mime });
};

export const fileToBase64 = async (file: File) => {
  return new Promise((resolve, reject) => {
    const reader = new FileReader();

    reader.onloadend = () => {
      return resolve(reader.result);
    };

    reader.readAsDataURL(file);
    reader.onerror = (error) => reject(error);
  });
};

export const base64toBlob = (dataURI: string, mineType: string) => {
  const urlSplitted = dataURI.split(",");
  let byteString = null;
  if (urlSplitted.length > 1) {
    byteString = atob(dataURI.split(",")[1]);
  } else {
    byteString = atob(dataURI);
  }

  const buffers = new ArrayBuffer(byteString.length);
  let unitBuffers = new Uint8Array(buffers);

  for (let i = 0; i < byteString.length; i++) {
    unitBuffers[i] = byteString.charCodeAt(i);
  }

  mineType = mineType ? mineType : "image/jpeg";
  return new Blob([buffers], { type: mineType });
};

export const getParameters = (
  urlParams: string | string[][] | Record<string, string> | URLSearchParams
) => {
  const parameters = new URLSearchParams(urlParams);
  let obj: {
    [index: string]: any;
  };

  for (const key of parameters.keys()) {
    if (parameters.getAll(key).length > 1) {
      obj[key] = parameters.getAll(key);
    } else {
      obj[key] = parameters.get(key);
    }
  }

  return obj;
};

export const generateQueryParameters = (parameters: {
  [index: string]: any;
}) => {
  const urlParams = [];
  for (const parameter in parameters) {
    const paramValue = parameters[parameter];
    if (paramValue) {
      const value = encodeURIComponent(parameters[parameter]);
      urlParams.push(`${encodeURIComponent(parameter)}=${value}`);
    }
  }

  return urlParams.join("&");
};
