export const mapSelectOptions = (selections) => {
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
