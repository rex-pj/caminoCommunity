import * as React from "react";
import styled from "styled-components";
import { Image, ImageProps } from "../../atoms/Images";
import { ImgHTMLAttributes } from "react";

export interface RootProps extends ImgHTMLAttributes<HTMLImageElement> {
  readonly maxHeight?: number | string;
  readonly maxWidth?: number | string;
  readonly height?: number | string;
  readonly width?: number | string;
}

const Root = styled.span<RootProps>`
  border-radius: ${(p) => p.theme.borderRadius.normal};
  text-align: center;
  display: block;
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

const Thumbnail = (props: ImageProps) => (
  <Root
    width={props.width}
    height={props.height}
    maxHeight={props.maxHeight}
    maxWidth={props.maxWidth}
    className={props.className}
    onClick={props.onClick}
  >
    <Image
      src={props.src}
      alt={props.alt}
      width={props.width}
      height={props.height}
      maxHeight={props.maxHeight}
      maxWidth={props.maxWidth}
    />
  </Root>
);

export default Thumbnail;
