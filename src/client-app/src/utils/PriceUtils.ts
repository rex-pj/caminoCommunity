export function formatPrice(number: number) {
  return number.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1.");
}

export const adjustPrice = (attributeValue: any, price?: number) => {
  let { priceAdjustment, pricePercentageAdjustment } = attributeValue;

  if (!price) {
    return 0;
  }

  let priceParsed = price.valueOf();
  const countPrice = () => {
    if (priceAdjustment) {
      price += priceAdjustment;
    } else if (pricePercentageAdjustment) {
      const percentageOfPrice = (priceParsed * pricePercentageAdjustment) / 100;
      priceParsed += percentageOfPrice;
    }

    return formatPrice(priceParsed);
  };
  return countPrice();
};
