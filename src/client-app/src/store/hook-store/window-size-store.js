import { useState, useEffect, useCallback } from "react";

function getBreakPoint(windowWidth) {
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
  const handleResetWindowSize = useCallback(() => {
    resetWindowSize();
  }, []);

  const resetWindowSize = () => {
    setWindowSize({
      isResized: false,
      isSizeTypeChanged: false,
      size: window.innerWidth,
      sizeType: isWindowClient ? getBreakPoint(window.innerWidth) : undefined,
      resetWindowSize: handleResetWindowSize,
    });
  };

  const [windowSize, setWindowSize] = useState({
    isResized: false,
    isSizeTypeChanged: false,
    size: window.innerWidth,
    sizeType: isWindowClient ? getBreakPoint(window.innerWidth) : undefined,
    resetWindowSize: handleResetWindowSize,
  });

  useEffect(() => {
    function setSize() {
      const sizeType = getBreakPoint(window.innerWidth);
      setWindowSize({
        isResized: window.innerWidth !== windowSize.size,
        isSizeTypeChanged: sizeType !== windowSize.sizeType,
        size: window.innerWidth,
        sizeType: getBreakPoint(window.innerWidth),
        resetWindowSize: handleResetWindowSize,
      });
    }

    if (isWindowClient) {
      window.addEventListener("resize", setSize);

      return () => window.removeEventListener("resize", setSize);
    }
  }, [isWindowClient, setWindowSize, windowSize, handleResetWindowSize]);

  return windowSize;
};
