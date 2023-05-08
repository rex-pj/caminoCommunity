import * as React from "react";
import NoImage from "../../molecules/NoImages/no-image";
import { ImageProps, Image } from "../../atoms/Images";

const ImageThumb: React.FC<ImageProps> = (props) => {
  const { src, className } = props;
  if (!src) {
    return <NoImage className={`no-image ${className??""}`}></NoImage>;
  }

  return <Image className="thumbnail-img" {...props}></Image>;
};

export default ImageThumb;
