function formatNumber(num) {
  return num.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1.");
}

function validateEmail(email) {
  let isValid = !!email;

  if (isValid) {
    const expression = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    isValid = expression.test(email);
  }
  return isValid;
}

function checkValidity(formData, value, rule) {
  let isValid = true;

  if (rule.isRequired) {
    isValid = value && value.trim() !== "" && isValid;
  }

  if (rule.isEmail) {
    isValid = validateEmail(value) && isValid;
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

export { formatNumber, validateEmail, checkValidity };
