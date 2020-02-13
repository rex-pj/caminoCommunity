function validateEmail(email) {
  let isValid = !!email;

  if (isValid) {
    const expression = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    isValid = expression.test(email);
  }
  return isValid;
}

function validateLink(link) {
  let isValid = !!link;

  if (isValid) {
    const expression = /[-a-zA-Z0-9@:%_+.~#?&//=]{2,256}\.[a-z]{2,4}\b(\/[-a-zA-Z0-9@:%_+.~#?&//=]*)?/gi;
    isValid = expression.test(link);
  }
  return isValid;
}

function checkValidity(formData, value, formName) {
  let isValid = true;

  const rule = formData[formName].validation;

  if (rule.isRequired) {
    isValid = value && value.trim() !== "" && isValid;
  }

  if (rule.isEmail) {
    isValid = validateEmail(value) && isValid;
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

  if (rule.minLength) {
    isValid = value.length >= rule.minLength && isValid;
  }

  if (rule.maxLength) {
    isValid = value.length <= rule.maxLength && isValid;
  }

  return isValid;
}

export { validateEmail, checkValidity };
