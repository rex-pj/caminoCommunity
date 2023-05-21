import { ValidationFormControl, FormControl } from "./formControls";
class FarmCreationModel {
  id: FormControl<number>;
  name: ValidationFormControl<string>;
  description: ValidationFormControl<string>;
  address: ValidationFormControl<string>;
  farmTypeName: FormControl<string>;
  farmTypeId: ValidationFormControl<number>;
  pictures: FormControl<any[]>;
  [key: string]: any;

  constructor() {
    this.id = new FormControl(0, {
      isValid: true,
    });
    this.name = new ValidationFormControl("", {
      validation: {
        isRequired: true,
      },
      isValid: false,
    });
    this.description = new ValidationFormControl("", {
      validation: {
        isRequired: true,
      },
      isValid: false,
    });
    this.address = new ValidationFormControl("", {
      validation: {
        isRequired: false,
      },
      isValid: true,
    });
    this.farmTypeName = new FormControl("", {
      isValid: true,
    });
    this.farmTypeId = new ValidationFormControl(0, {
      validation: {
        isRequired: true,
      },
      isValid: false,
    });
    this.pictures = new FormControl([], {
      isValid: true,
    });
  }
}

export { FarmCreationModel };
