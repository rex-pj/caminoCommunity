import { ImgHTMLAttributes } from "react";
import styled from "styled-components";

export interface ImageProps extends ImgHTMLAttributes<HTMLImageElement> {
  readonly round?: number;
  readonly maxHeight?: number | string;
  readonly maxWidth?: number | string;
  readonly height?: number | string;
  readonly width?: number | string;
}

const Image = styled.img<ImageProps>`
  max-width: 100%;
  border-radius: ${(p) => (p.round ? `${p.round}px` : "")};
  height: ${(p) => calculateUnit(p.height, "px", "")};
  width: ${(p) => calculateUnit(p.width, "px", "")};
  max-height: ${(p) => calculateUnit(p.maxHeight, "px", "")};
  max-width: ${(p) => calculateUnit(p.maxWidth, "px", "")};
`;

const calculateUnit = (
  value?: number | string,
  unit?: string,
  noneText?: string
) => {
  if (!value) {
    return noneText;
  }
  if (typeof value === "number") {
    return `${value}${unit}`;
  }

  return noneText;
};

const ImageCircle = styled(Image)`
  border-radius: 100%;
`;

const ImageRound = styled(Image)`
  border-radius: ${(p) => p.theme.borderRadius.normal};
`;

export { Image, ImageCircle, ImageRound };
