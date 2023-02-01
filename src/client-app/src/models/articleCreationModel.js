const articleCreationModel = {
  name: {
    value: "",
    validation: {
      isRequired: true,
    },
    isValid: false,
  },
  content: {
    value: "",
    validation: {
      isRequired: true,
    },
    isValid: false,
  },
  articleCategoryId: {
    value: 0,
    validation: {
      isRequired: true,
    },
    isValid: false,
  },
  articleCategoryName: {
    value: "",
    isValid: true,
  },
  picture: {
    value: null,
    isValid: true,
  },
  id: {
    value: 0,
    isValid: true,
  },
};

export default articleCreationModel;
