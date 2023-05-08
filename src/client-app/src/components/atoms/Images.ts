import { ImgHTMLAttributes } from "react";
import styled from "styled-components";

export interface ImageProps extends ImgHTMLAttributes<HTMLImageElement> {
  readonly round?: number;
}

const Image = styled.img<ImageProps>`
  max-width: 100%;
  border-radius: ${(p) => (p.round ? `${p.round}px` : "0")};
`;

const ImageCircle = styled(Image)`
  border-radius: 100%;
`;

const ImageRound = styled(Image)`
  border-radius: ${(p) => p.theme.borderRadius.normal};
`;

export { Image, ImageCircle, ImageRound };
