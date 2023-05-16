import { ValidationFormControl } from "./formControls";
class LoginModel {
  username: ValidationFormControl<string>;
  password: ValidationFormControl<string>;
  [key: string]: any;
  constructor() {
    this.username = new ValidationFormControl("", {
      isValid: false,
      validation: {
        isRequired: true,
      },
    });
    this.password = new ValidationFormControl("", {
      isValid: false,
      validation: {
        isRequired: true,
      },
    });
  }
}

export { LoginModel };
