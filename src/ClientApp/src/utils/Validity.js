export function validateEmail(email) {
  let isValid = !!email;

  if (isValid) {
    const expression = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    isValid = expression.test(email);
  }
  return isValid;
}

export function validateLink(link) {
  let isValid = !!link;

  if (isValid) {
    const expression = /(http|https):\/\/(\w+:{0,1}\w*@)?(\S+)(:[0-9]+)?(\/|\/([\w#!:.?+=&%@!\-/]))?/;
    isValid = expression.test(link);
  }
  return isValid;
}

export function validateImageLink(link) {
  let isValid = !!link;

  if (isValid) {
    const expression = /(http)?s?:?(\/\/[^"']*\.(?:png|jpg|jpeg|gif|png|svg))/g;
    isValid = expression.test(link);
  }

  if (!isValid) {
    const expression = /data:image\/([a-zA-Z]*);base64,([^"]*)/g;
    isValid = expression.test(link);
  }

  return isValid;
}

export function checkValidity(formData, value, formName) {
  let isValid = true;

  const rule = formData[formName].validation;

  if (rule.isRequired) {
    isValid = value && value.trim() !== "" && isValid;
  }

  if (rule.isEmail) {
    isValid = validateEmail(value) && isValid;
  }

  if (rule.isImageLink) {
    isValid = validateImageLink(value) && isValid;
  }

  if (rule.isLink) {
    isValid = validateLink(value) && isValid;
  }

  if (rule.sameRefProperty) {
    isValid = value === formData[rule.sameRefProperty].value && isValid;
  }

  if (rule.isDate) {
    isValid = value && isValid;
  }

  if (rule.isNumber) {
    isValid = isNaN(value) && isValid;
  }

  if (rule.minLength) {
    isValid = value.length >= rule.minLength && isValid;
  }

  if (rule.maxLength) {
    isValid = value.length <= rule.maxLength && isValid;
  }

  return isValid;
}
