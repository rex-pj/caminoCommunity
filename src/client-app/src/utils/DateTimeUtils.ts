export function convertDateTimeToPeriod(dateTime) {
  const prevDate = new Date(dateTime);
  const today = new Date();
  const millisec = Math.abs(today - prevDate);

  const seconds = (millisec / 1000).toFixed(1);
  const minutes = parseInt((millisec / (1000 * 60)).toFixed(1));
  const hours = parseInt((millisec / (1000 * 60 * 60)).toFixed(1));
  const days = parseInt((millisec / (1000 * 60 * 60 * 24)).toFixed(1));

  if (seconds < 60) {
    return `${seconds} sec`;
  }

  if (minutes < 60) {
    return `${minutes} min`;
  }

  if (hours < 24) {
    return `${hours} hrs`;
  }

  return `${days} days`;
}
