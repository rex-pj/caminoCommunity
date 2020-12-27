export default {
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
  thumbnail: {
    value: {
      pictureId: 0,
      fileName: "",
      contentType: "",
      base64Data: "",
    },
    isValid: true,
  },
  id: {
    value: 0,
    isValid: true,
  },
};
