export function formatPrice(num) {
  return num.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1.");
}

export const adjustPrice = (attributeValue, price) => {
  let { priceAdjustment, pricePercentageAdjustment } = attributeValue;

  const countPrice = () => {
    if (priceAdjustment) {
      price += priceAdjustment;
    } else if (pricePercentageAdjustment) {
      const percentageOfPrice = (price * pricePercentageAdjustment) / 100;
      price += percentageOfPrice;
    }

    return formatPrice(price);
  };
  return countPrice();
};
