export interface SelectOption {
  id: any;
  text: string;
}

export const mapSelectOptions = (selections?: SelectOption[]) => {
  if (!selections) {
    return [];
  }
  return selections.map((cat) => {
    return {
      value: cat.id,
      label: cat.text,
    };
  });
};
