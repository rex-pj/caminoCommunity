import React, { Fragment, useState, useEffect } from "react";
import styled from "styled-components";
import { PanelBody } from "../../atoms/Panels";
import { Textbox } from "../../atoms/Textboxes";
import { LabelNormal } from "../../atoms/Labels";
import EditorImageScalePreview from "./EditorImageScalePreview";

const Body = styled(PanelBody)`
  padding: ${(p) => p.theme.size.tiny};
`;

const FormRow = styled.div`
  margin-bottom: ${(p) => p.theme.size.tiny};

  ${LabelNormal} {
    display: block;
  }

  ${Textbox} {
    width: 100%;
  }
`;

export default (props) => {
  const { handleImageChange, imageData, onAddImage } = props;
  const [photoData, setPhotoData] = useState(imageData);

  const handleKeyUp = (e) => {
    if (e.key === "Enter") {
      onAddImage();
    }
  };

  const onImageUrlChange = (e) => {
    var { name } = e.target;
    const { src } = photoData;
    handleImageChange(name, src.value);
  };

  const onInputChange = (e) => {
    const formData = photoData || {};
    const { name, value } = e.target;
    formData[name].value = value;
    setPhotoData({
      ...formData,
    });
  };

  useEffect(() => {
    return () => {};
  }, []);

  const { src } = photoData;
  return (
    <Fragment>
      <Body>
        <FormRow>
          <LabelNormal>Url of your image</LabelNormal>
          <Textbox
            name="src"
            onKeyUp={handleKeyUp}
            value={src.value}
            autoComplete="off"
            onChange={(e) => onInputChange(e)}
            onBlur={(e) => onImageUrlChange(e)}
          />
        </FormRow>
        <EditorImageScalePreview imageData={photoData} />
      </Body>
    </Fragment>
  );
};
