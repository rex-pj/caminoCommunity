class ProductCreationModel {
  name: ValidationFormControl<string>;
  price: ValidationFormControl<number>;
  description: ValidationFormControl<string>;
  categories: ValidationFormControl<any[]>;
  farms: ValidationFormControl<any[]>;
  pictures: FormControl<any[]>;
  id: FormControl<number>;
  productAttributes: FormControl<any[]>;
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

    this.productAttributes = new FormControl([], {
      isValid: true,
    });
  }
}

export { ProductCreationModel };
