let model = {
  lastname: {
    value: "",
    validation: {
      isRequired: true
    },
    isValid: false
  },
  firstname: {
    value: "",
    validation: {
      isRequired: true
    },
    isValid: false
  },
  email: {
    value: "",
    validation: {
      isEmail: true
    },
    isValid: false
  },
  password: {
    value: "",
    validation: {
      isRequired: true
    },
    isValid: false
  },
  confirmPassword: {
    value: "",
    validation: {
      isRequired: true,
      sameRefProperty: "password"
    },
    isValid: false
  },
  genderId: {
    value: 1,
    validation: {
      isRequired: true
    },
    isValid: true
  },
  birthDate: {
    value: new Date(),
    validation: {
      isDate: true
    },
    isValid: false
  }
};

export default model;
