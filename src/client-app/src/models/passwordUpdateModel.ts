import { ValidationFormControl } from "./formControls";

class PasswordUpdateModel {
  currentPassword: ValidationFormControl<string>;
  newPassword: ValidationFormControl<string>;
  confirmPassword: ValidationFormControl<string>;
  [index: string]: any;
  constructor() {
    this.currentPassword = new ValidationFormControl("", {
      isValid: false,
      validation: {
        isRequired: true,
        minLength: 6,
      },
    });
    this.newPassword = new ValidationFormControl("", {
      isValid: false,
      validation: {
        isRequired: true,
        minLength: 6,
      },
    });
    this.confirmPassword = new ValidationFormControl("", {
      isValid: false,
      validation: {
        isRequired: true,
        minLength: 6,
        sameRefProperty: "newPassword",
      },
    });
  }
}

export { PasswordUpdateModel };
