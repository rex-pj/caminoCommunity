import * as React from "react";
import {
  Fragment,
  useState,
  useEffect,
  ChangeEvent,
  KeyboardEvent,
} from "react";
import styled from "styled-components";
import { PanelBody } from "../../molecules/Panels";
import { PrimaryTextbox } from "../../atoms/Textboxes";
import { LabelNormal } from "../../atoms/Labels";
import EditorImageScalePreview from "./EditorImageScalePreview";
import { ImageInfoModel } from "../../../models/imageInfoModel";

const Body = styled(PanelBody)`
  padding: ${(p) => p.theme.size.tiny};
`;

const FormRow = styled.div`
  margin-bottom: ${(p) => p.theme.size.tiny};

  ${LabelNormal} {
    display: block;
  }

  ${PrimaryTextbox} {
    width: 100%;
  }
`;

interface EditorImageLinkProps {
  imageData: ImageInfoModel;
  handleImageChange: (name: string, value?: string) => void;
  onScaleChanged: (e: ChangeEvent<HTMLInputElement>) => void;
  onAddImage: () => void;
  imageSrc: string;
}

const EditorImageLink: React.FC<EditorImageLinkProps> = (props) => {
  const { handleImageChange, imageData, onAddImage, imageSrc } = props;
  const [photoData, setPhotoData] = useState<ImageInfoModel>(imageData);

  const handleKeyUp = (e: KeyboardEvent<HTMLInputElement>) => {
    if (e.key === "Enter") {
      onAddImage();
    }
  };

  const onImageUrlChange = (e: ChangeEvent<HTMLInputElement>) => {
    const { name } = e.target;
    const { src } = photoData;
    handleImageChange(name, src.value);
  };

  const onInputChange = (e: ChangeEvent<HTMLInputElement>) => {
    const formData = photoData || new ImageInfoModel();
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
          <PrimaryTextbox
            name="src"
            onKeyUp={handleKeyUp}
            value={src.value}
            autoComplete="off"
            onChange={(e) => onInputChange(e)}
            onBlur={(e) => onImageUrlChange(e)}
          />
        </FormRow>
        <EditorImageScalePreview previewSrc={imageSrc} imageData={photoData} />
      </Body>
    </Fragment>
  );
};

export default EditorImageLink;
