class ImageInfoModel {
  constructor() {
    this.src = new ValidationFormControl("", {
      isValid: false,
      validation: {
        isImageLink: true,
        isRequired: true,
      },
    });
    this.width = new ValidationFormControl("auto", {
      isValid: true,
      validation: {
        isRequired: true,
      },
    });
    this.height = new ValidationFormControl("auto", {
      isValid: true,
      validation: {
        isRequired: true,
      },
    });
    this.alt = new ValidationFormControl("", {
      isValid: true,
      validation: {
        isRequired: true,
      },
    });
  }
  src: ValidationFormControl<string>;
  width: ValidationFormControl<number | string>;
  height: ValidationFormControl<number | string>;
  alt: ValidationFormControl<string>;
  [key: string]: any;
}

export { ImageInfoModel };
