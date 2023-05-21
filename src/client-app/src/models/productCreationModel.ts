import { IProductAttribute } from "./productAttributesModel";
import { ValidationFormControl, FormControl } from "./formControls";

class ProductCreationModel {
  name: ValidationFormControl<string>;
  price: ValidationFormControl<number>;
  description: ValidationFormControl<string>;
  categories: ValidationFormControl<any[]>;
  farms: ValidationFormControl<any[]>;
  pictures: FormControl<any[]>;
  id: FormControl<number>;
  productAttributes: FormControl<IProductAttribute[]>;
  [index: string]: any;
  constructor() {
    this.name = new ValidationFormControl("", {
      validation: {
        isRequired: true,
      },
      isValid: false,
    });

    this.price = new ValidationFormControl(0, {
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

    this.categories = new ValidationFormControl([], {
      validation: {
        isRequired: true,
      },
      isValid: false,
    });

    this.farms = new ValidationFormControl([], {
      validation: {
        isRequired: true,
      },
      isValid: false,
    });

    this.pictures = new FormControl([], {
      isValid: true,
    });

    this.id = new FormControl(0, {
      isValid: true,
    });

    this.productAttributes = new FormControl<IProductAttribute[]>([], {
      isValid: true,
    });
  }
}

export { ProductCreationModel };
