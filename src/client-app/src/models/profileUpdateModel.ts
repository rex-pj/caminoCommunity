import { ValidationFormControl } from "./formControls";

class ProfileUpdateModel {
  displayName: ValidationFormControl<string>;
  lastname: ValidationFormControl<string>;
  firstname: ValidationFormControl<string>;
  [index: string]: any;
  constructor() {
    this.displayName = new ValidationFormControl("", {
      validation: {
        isRequired: true,
      },
      isValid: false,
    });
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
  }
}

export { ProfileUpdateModel };
