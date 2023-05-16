import { ValidationFormControl } from "./formControls";

class SignupModel {
  lastname: ValidationFormControl<string>;
  firstname: ValidationFormControl<string>;
  email: ValidationFormControl<string>;
  password: ValidationFormControl<string>;
  confirmPassword: ValidationFormControl<string>;
  [index: string]: any;
  constructor() {
    this.lastname = new ValidationFormControl("", {
      validation: {
        isRequired: true,
      },
      isValid: false,
    });

    this.firstname = new ValidationFormControl("", {
      validation: {
        isRequired: true,
      },
      isValid: false,
    });

    this.email = new ValidationFormControl("", {
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

export { SignupModel };
