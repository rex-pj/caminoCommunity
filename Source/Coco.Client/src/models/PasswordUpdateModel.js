let model = {
  currentPassword: {
    value: "",
    validation: {
      isRequired: true,
      minLength: 6
    },
    isValid: false
  },
  newPassword: {
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
      sameRefProperty: "newPassword"
    },
    isValid: false
  }
};

export default model;
