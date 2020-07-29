export default {
  src: {
    value: "",
    validation: {
      isRequired: true,
      isImageLink: true,
    },
    isValid: false,
  },
  width: {
    value: "auto",
    validation: {
      isRequired: false,
    },
    isValid: true,
  },
  height: {
    value: "auto",
    validation: {
      isRequired: false,
    },
    isValid: true,
  },
  alt: {
    value: "",
    validation: {
      isRequired: false,
    },
    isValid: true,
  },
};
