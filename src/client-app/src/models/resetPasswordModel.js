let model = {
  email: {
    value: "",
    validation: {
      isEmail: true,
    },
    isValid: false,
  },
  key: {
    value: "",
    validation: {
      isRequired: true,
    },
    isValid: false,
  },
  password: {
    value: "",
    validation: {
      isRequired: true,
      minLength: 6,
    },
    isValid: false,
  },
  confirmPassword: {
    value: "",
    validation: {
      isRequired: true,
      minLength: 6,
      sameRefProperty: "password",
    },
    isValid: false,
  },
};

export default model;
