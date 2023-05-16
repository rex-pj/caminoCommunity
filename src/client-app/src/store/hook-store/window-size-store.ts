import { useState, useEffect } from "react";

function getBreakPoint(windowWidth: number) {
  if (!windowWidth) {
    return undefined;
  }
  if (windowWidth < 567) {
    return "xs";
  }
  if (windowWidth < 768) {
    return "sm";
  }
  if (windowWidth < 992) {
    return "md";
  }
  if (windowWidth < 1200) {
    return "lg";
  }
  if (windowWidth < 1400) {
    return "xl";
  }
  return "xxl";
}

export const useWindowSize = () => {
  const isWindowClient = typeof window === "object";
  const defaultState = {
    isResized: false,
    isSizeTypeChanged: false,
    size: window.innerWidth,
    sizeType: isWindowClient ? getBreakPoint(window.innerWidth) : undefined,
  };

  const resetWindowSize = () => {
    setWindowSize({
      ...defaultState,
    });
  };

  const [windowSize, setWindowSize] = useState({
    ...defaultState,
  });

  useEffect(() => {
    function setSize() {
      const sizeType = getBreakPoint(window.innerWidth);
      setWindowSize({
        isResized: window.innerWidth !== windowSize.size,
        size: window.innerWidth,
        sizeType: getBreakPoint(window.innerWidth),
        isSizeTypeChanged: sizeType !== windowSize.sizeType,
      });
    }

    if (isWindowClient) {
      window.addEventListener("resize", setSize);

      return () => window.removeEventListener("resize", setSize);
    }
  }, [isWindowClient, setWindowSize, windowSize]);

  return [windowSize, resetWindowSize];
};
