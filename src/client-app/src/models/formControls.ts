class FormControl<T> {
  public constructor();
  public constructor(value?: T, options?: { isValid?: boolean });

  public constructor(...arr: any[]) {
    if (arr.length === 1) {
      this.value = arr[0];
    } else if (arr.length === 2) {
      this.value = arr[0];
      this.isValid = arr[1].isValid;
    }
  }

  value?: T;
  isValid?: boolean;
}

interface ValidationOptions {
  readonly isRequired?: boolean;
  readonly isImageLink?: boolean;
  readonly isEmail?: boolean;
  readonly minLength?: number;
  readonly sameRefProperty?: string;
  readonly pattern?: string[];
}

class ValidationFormControl<T> extends FormControl<T> {
  public constructor();
  public constructor(
    value?: T,
    options?: { isValid?: boolean; validation?: ValidationOptions }
  );

  public constructor(...arr: any[]) {
    if (arr.length === 1) {
      super(arr as T);
    } else if (arr.length === 2) {
      super();
      this.validation = arr[3].validation;
    } else {
      super();
    }
  }
  validation?: ValidationOptions;
}
