const productCreationModel = {
  name: {
    value: "",
    validation: {
      isRequired: true,
    },
    isValid: false,
  },
  price: {
    value: 0,
    validation: {
      isRequired: true,
    },
    isValid: false,
  },
  description: {
    value: "",
    validation: {
      isRequired: true,
    },
    isValid: false,
  },
  categories: {
    value: [],
    validation: {
      isRequired: true,
    },
    isValid: false,
  },
  farms: {
    value: [],
    validation: {
      isRequired: true,
    },
    isValid: false,
  },
  pictures: {
    value: [],
    isValid: true,
  },
  id: {
    value: 0,
    isValid: true,
  },
  productAttributes: {
    value: [],
    isValid: true,
  },
};

export default productCreationModel;
