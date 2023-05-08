import * as React from "react";
import { useState, useEffect, ChangeEvent } from "react";
import { Tabs } from "../Tabs/Tabs";
import EditorImageUploader from "./EditorImageUploader";
import EditorImageLink from "./EditorImageLink";
import styled from "styled-components";
import EditorImageInfo from "./EditorImageInfo";
import { ImageInfoModel } from "../../../models/imageInfoModel";
import { ButtonPrimary, ButtonLight } from "../../atoms/Buttons/Buttons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { AtomicBlockUtils, EditorState } from "draft-js";
import { fileToBase64 } from "../../../utils/Helper";
import { ImageUploadOnChangeEvent } from "../UploadControl/ImageUpload";

const Root = styled.div`
  min-width: 400px;
  margin: ${(p) => p.theme.size.tiny} ${(p) => p.theme.size.tiny} 0
    ${(p) => p.theme.size.tiny};
  min-height: 200px;
  background-color: ${(p) => p.theme.color.darkBg};
  border-radius: ${(p) => p.theme.borderRadius.normal};
`;

const Footer = styled.div`
  min-height: 20px;
  text-align: right;
  border-top: 1px solid ${(p) => p.theme.rgbaColor.darker};
  padding: ${(p) => p.theme.size.exTiny} ${(p) => p.theme.size.tiny};

  button {
    margin-left: ${(p) => p.theme.size.exTiny};
    font-weight: normal;
    padding: ${(p) => p.theme.size.exTiny} ${(p) => p.theme.size.tiny};

    svg {
      margin-right: ${(p) => p.theme.size.exTiny};
    }
  }
`;

const ImageEditorTabs = styled(Tabs)`
  ul.tabs-bar {
    border-bottom: 1px solid ${(p) => p.theme.rgbaColor.cyanLight};
  }
  ul.tabs-bar li button {
    background-color: transparent;
    color: ${(p) => p.theme.color.neutralText};
  }

  ul.tabs-bar li.actived button {
    background-color: ${(p) => p.theme.rgbaColor.cyanLight};
    color: ${(p) => p.theme.color.neutralText};
  }
`;

interface EditorImageModalProps {
  editorState: EditorState;
  convertImageCallback?: (e: any) => Promise<any>;
  onImageValidate?: (e: any) => Promise<any>;
  onClose: () => void;
  onAccept: (e: EditorState) => void;
}

const EditorImageModal: React.FC<EditorImageModalProps> = (props) => {
  const { editorState, convertImageCallback, onImageValidate } = props;
  const [imageData, setImageData] = useState<ImageInfoModel>(
    new ImageInfoModel()
  );
  const [imageSrc, setImageSrc] = useState("");

  const clearImageData = () => {
    setImageData({
      ...new ImageInfoModel(),
    });
  };

  const onClose = () => {
    clearImageData();
    props.onClose();
  };

  useEffect(() => {
    return () => {};
  }, []);

  const onAddImage = (e: EditorState) => {
    props.onAccept(e);
  };

  const handleImageChange = async (e: ImageUploadOnChangeEvent) => {
    const file = e.file;
    await convertImageCallback(file).then(async (fileData: any) => {
      const { file } = fileData;
      await handleImageUrlChange(e.target.name, file);
    });
  };

  const handleImageUrlChange = async (formName: string, src: string) => {
    let data = imageData || new ImageInfoModel();
    if (!src) {
      data[formName].isValid = false;
      data[formName].value = "";
      return;
    }

    let formData = new FormData();
    formData.append("file", src);
    await onImageValidate(formData)
      .then(async (response: any) => {
        data[formName].isValid = true;
        data[formName].value = src;
        // const previewSrc = await fileToBase64(file);
        setImageSrc(src);
        setImageData({
          ...data,
        });
      })
      .catch((error) => {
        data[formName].isValid = false;
        data[formName].value = null;
        setImageSrc("");
        setImageData({
          ...data,
        });
      });
  };

  const onScaleChanged = (e: ChangeEvent<HTMLInputElement>) => {
    const value = e.target.value;
    const formData = imageData;
    if ("auto".indexOf(value) >= 0) {
      formData[e.target.name].value = value;
    } else if (!value || Number.isNaN(value)) {
      formData[e.target.name].value = "auto";
    } else {
      const newSize = parseFloat(value);
      formData[e.target.name].value = newSize;
    }

    setImageData({
      ...formData,
    });
  };

  const handleInputChange = (evt: ChangeEvent<HTMLInputElement>) => {
    const formData = imageData || new ImageInfoModel();
    const { name, value } = evt.target;

    formData[name].value = value;
    setImageData({
      ...formData,
    });
  };

  const onImageAdded = () => {
    const { width, height, alt } = imageData;

    const contentState = editorState.getCurrentContent();
    const contentStateWithEntity = contentState.createEntity(
      "IMAGE",
      "IMMUTABLE",
      {
        src: imageSrc,
        height: height.value,
        width: width.value,
        alt: alt.value,
      }
    );
    const entityKey = contentStateWithEntity.getLastCreatedEntityKey();
    const newEditorState = EditorState.set(editorState, {
      currentContent: contentStateWithEntity,
    });

    const imageEditorState = AtomicBlockUtils.insertAtomicBlock(
      newEditorState,
      entityKey,
      " "
    );

    onAddImage(imageEditorState);
    onClose();
  };

  const onTabClicked = (e: React.MouseEvent<HTMLButtonElement>) => {
    clearImageData();
  };

  const { src } = imageData;
  const { isValid } = src;
  return (
    <Root>
      <ImageEditorTabs
        onTabClicked={onTabClicked}
        tabs={[
          {
            title: "Upload",
            tabComponent: (
              <EditorImageUploader
                handleImageChange={handleImageChange}
                imageData={imageData}
                imageSrc={imageSrc}
              />
            ),
          },
          {
            title: "Link",
            tabComponent: (
              <EditorImageLink
                onAddImage={onImageAdded}
                handleImageChange={handleImageUrlChange}
                onScaleChanged={onScaleChanged}
                imageData={imageData}
                imageSrc={imageSrc}
              />
            ),
          },
        ]}
      />
      <EditorImageInfo
        handleInfoChange={handleInputChange}
        onScaleChanged={onScaleChanged}
        imageData={imageData}
      />
      <Footer>
        <ButtonLight size="sm" onClick={onClose}>
          Đóng
        </ButtonLight>
        <ButtonPrimary size="sm" onClick={onImageAdded} disabled={!isValid}>
          <FontAwesomeIcon icon="check" />
          Lưu
        </ButtonPrimary>
      </Footer>
    </Root>
  );
};

export default EditorImageModal;
