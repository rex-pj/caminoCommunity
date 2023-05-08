interface EditorLinkModel {
  url: {
    value: string;
    validation: {
      readonly isRequired: boolean;
      readonly isLink: boolean;
    };
    isValid: boolean;
  };
  title: {
    value: string;
    validation: {
      isRequired: boolean;
    };
    isValid: boolean;
  };
  [key: string]: any;
}

const DefaultEditorLink: EditorLinkModel = {
  url: {
    value: "",
    validation: {
      isRequired: true,
      isLink: true,
    },
    isValid: true,
  },
  title: {
    value: "",
    validation: {
      isRequired: false,
    },
    isValid: true,
  },
};

export { EditorLinkModel, DefaultEditorLink };
