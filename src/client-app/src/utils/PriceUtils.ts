export function formatPrice(number: number) {
  return number.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1.");
}

export const adjustPrice = (attributeValue: any, price?: number) => {
  let { priceAdjustment, pricePercentageAdjustment } = attributeValue;

  if (!price || Number.isNaN(price)) {
    return 0;
  }

  let priceParsed = Number.parseFloat(price.toString());
  const countPrice = () => {
    if (priceAdjustment) {
      priceParsed += priceAdjustment;
    } else if (pricePercentageAdjustment) {
      const percentageOfPrice = (priceParsed * pricePercentageAdjustment) / 100;
      priceParsed += percentageOfPrice;
    }

    return formatPrice(priceParsed);
  };
  return countPrice();
};
