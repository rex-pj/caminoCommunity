import React, { Fragment, useEffect } from "react";
import ImageUpload from "../UploadControl/ImageUpload";
import styled from "styled-components";
import { PanelBody } from "../../atoms/Panels";
import EditorImageScalePreview from "./EditorImageScalePreview";

const Body = styled(PanelBody)`
  padding: ${(p) => p.theme.size.tiny};
`;

const PhotoUpload = styled(ImageUpload)`
  text-align: center;
  margin: 0 auto ${(p) => p.theme.size.tiny} auto;
  display: block;
  width: 200px;

  > span {
    color: ${(p) => p.theme.color.neutral};
    height: ${(p) => p.theme.size.medium};
    padding: 0 ${(p) => p.theme.size.tiny};
    background-color: ${(p) => p.theme.color.lighter};
    border-radius: ${(p) => p.theme.borderRadius.large};
    border: 1px solid ${(p) => p.theme.color.neutral};
    cursor: pointer;
    font-weight: 600;

    :hover {
      background-color: ${(p) => p.theme.color.light};
    }

    svg {
      display: inline-block;
      margin: 10px auto 0 auto;
    }
  }
`;

export default (props) => {
  const { imageData, handleImageChange } = props;

  useEffect(() => {
    return () => {};
  }, []);

  return (
    <Fragment>
      <Body>
        <PhotoUpload onChange={(e) => handleImageChange(e)} name="src">
          Select image to upload
        </PhotoUpload>

        <EditorImageScalePreview imageData={imageData} />
      </Body>
    </Fragment>
  );
};
