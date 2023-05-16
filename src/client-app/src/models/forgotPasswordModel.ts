class ForgotPasswordModel {
  email: ValidationFormControl<string>;
  [key: string]: any;
  constructor() {
    this.email = new ValidationFormControl("", {
      isValid: false,
      validation: {
        isRequired: true,
        isEmail: true,
      },
    });
  }
}

export { ForgotPasswordModel };
