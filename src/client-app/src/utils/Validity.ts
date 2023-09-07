export function validateEmail(email: string) {
  if (!email) {
    return false;
  }
  const expression = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
  return expression.test(email);
}

export function validatePhoneNumber(phoneNumber: string) {
  if (!phoneNumber) {
    return false;
  }
  const expression = /^[+]*[(]{0,1}[0-9]{1,4}[)]{0,1}[-\s\./0-9]*$/g;
  return expression.test(phoneNumber);
}

export function validateLink(link: string) {
  let isValid = !!link;

  if (isValid) {
    const expression = /(http|https):\/\/(\w+:{0,1}\w*@)?(\S+)(:[0-9]+)?(\/|\/([\w#!:.?+=&%@!\-/]))?/;
    isValid = expression.test(link);
  }
  return isValid;
}

export function validateImageLink(link: string) {
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

export function checkValidity(formData: any, value: any, formName: string) {
  let isValid = true;

  const rule = formData[formName].validation;

  if (!rule) {
    return isValid;
  }

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
