import * as React from "react";
import styled from "styled-components";
import { Image, ImageProps } from "../../atoms/Images";
import { ImgHTMLAttributes } from "react";

const Root = styled.span`
  border-radius: ${(p) => p.theme.borderRadius.normal};
  text-align: center;
  display: block;
`;

const Thumbnail: React.FC<ImageProps> = (props) => (
  <Root className={props.className} onClick={props.onClick}>
    <Image
      src={props.src}
      alt={props.alt}
      width={props.width}
      height={props.height}
    />
  </Root>
);

export default Thumbnail;
