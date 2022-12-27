import React, { Fragment } from "react";
import styled from "styled-components";
import NoImage from "../../molecules/NoImages/no-image";
import { Image } from "../../atoms/Images";

const ImageWrap = styled.div`
  text-align: center;
`;

const EmptyImage = styled(NoImage)`
  border-radius: ${(p) => p.theme.borderRadius.medium};
  width: 200px;
  height: 200px;
  display: inline-block;
  font-size: ${(p) => p.theme.size.large};
`;

const EditorImageScalePreview = (props) => {
  const { imageData, previewSrc } = props;

  const { src, width, height, alt } = imageData;
  return (
    <Fragment>
      <ImageWrap>
        {src.value ? (
          <Image
            src={previewSrc}
            alt={alt.value}
            width={width.value}
            height={height.value}
          />
        ) : (
          <EmptyImage />
        )}
      </ImageWrap>
    </Fragment>
  );
};

export default EditorImageScalePreview;
