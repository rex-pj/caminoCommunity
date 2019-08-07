let model = {
  oldPassword: {
    value: "",
    validation: {
      isRequired: true,
      minLength: 6
    },
    isValid: false
  },
  password: {
    value: "",
    validation: {
      isRequired: true,
      minLength: 6
    },
    isValid: false
  },
  confirmPassword: {
    value: "",
    validation: {
      isRequired: true,
      minLength: 6,
      sameRefProperty: "password"
    },
    isValid: false
  }
};

export default model;
