import { ValidationFormControl } from "./formControls";

class ResetPasswordModel {
  email: ValidationFormControl<string>;
  key: ValidationFormControl<string>;
  password: ValidationFormControl<string>;
  confirmPassword: ValidationFormControl<string>;
  [index: string]: any;
  constructor() {
    this.email = new ValidationFormControl("", {
      validation: {
        isEmail: true,
      },
      isValid: false,
    });

    this.key = new ValidationFormControl("", {
      validation: {
        isEmail: true,
      },
      isValid: false,
    });

    this.password = new ValidationFormControl("", {
      validation: {
        isRequired: true,
        minLength: 6,
      },
      isValid: false,
    });

    this.confirmPassword = new ValidationFormControl("", {
      validation: {
        isRequired: true,
        minLength: 6,
        sameRefProperty: "password",
      },
      isValid: false,
    });
  }
}

export { ResetPasswordModel };
