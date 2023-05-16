class PictureFormControl extends FormControl<any> {
  public constructor(options?: { isValid?: boolean });

  public constructor(value?: any, options?: { isValid?: boolean });
  public constructor(
    value?: any,
    file?: File,
    pictureId?: number,
    options?: { isValid?: boolean }
  );

  public constructor(
    value?: any,
    file?: File,
    puctureId?: number,
    options?: { isValid?: boolean }
  );

  public constructor(...arr: any[]) {
    super();
    if (arr.length === 1) {
      this.isValid = arr[0].isValid;
    } else if (arr.length === 2) {
      this.value = arr[0];
      this.isValid = arr[1].isValid;
    } else if (arr.length === 4) {
      this.value = arr[0];
      this.file = arr[1];
      this.pictureId = arr[2];
      this.isValid = arr[3].isValid;
    }
  }

  file?: File;
  pictureId?: string;
}

class ArticleCreationModel {
  name: ValidationFormControl<string>;
  content: ValidationFormControl<string>;
  articleCategoryId: ValidationFormControl<number>;
  articleCategoryName: FormControl<string>;
  picture: PictureFormControl;
  id: FormControl<number>;
  [key: string]: any;
  constructor() {
    this.name = new ValidationFormControl("", {
      isValid: false,
      validation: {
        isRequired: true,
      },
    });
    this.content = new ValidationFormControl("", {
      isValid: false,
      validation: {
        isRequired: true,
      },
    });
    this.articleCategoryId = new ValidationFormControl(0, {
      isValid: false,
      validation: {
        isRequired: true,
      },
    });
    this.articleCategoryName = new FormControl("", {
      isValid: true,
    });
    this.picture = new PictureFormControl({
      isValid: true,
    });
    this.id = new FormControl(0, {
      isValid: true,
    });
  }
}

export { ArticleCreationModel };
