import React from "react";
import NoImage from "../../molecules/NoImages/no-image";
import { Image } from "../../atoms/Images";

const ImageThumb = (props) => {
  const { src } = props;
  const className = props.className ? props.className : "";
  if (!src) {
    return <NoImage className={`no-image ${className}`}></NoImage>;
  }

  return <Image className="thumbnail-img" {...props}></Image>;
};

export default ImageThumb;
