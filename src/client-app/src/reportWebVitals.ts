const reportWebVitals = (onLogEntry?: Function) => {
  if (onLogEntry && onLogEntry instanceof Function) {
    import("web-vitals").then(({ onCLS, onFID, onFCP, onLCP, onTTFB }) => {
      onCLS(console.log);
      onFID(console.log);
      onFCP(console.log);
      onLCP(console.log);
      onTTFB(console.log);
    });
  }
};

export default reportWebVitals;
