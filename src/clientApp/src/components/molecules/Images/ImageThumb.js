import React from "react";
import NoImage from "../../atoms/NoImages/no-image";
import { Image } from "../../atoms/Images";

export default (props) => {
  const { src } = props;
  const className = props.className ? props.className : "";
  if (!src) {
    return <NoImage className={`no-image ${className}`}></NoImage>;
  }

  return <Image {...props}></Image>;
};
