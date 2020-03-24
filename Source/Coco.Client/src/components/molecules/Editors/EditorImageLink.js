import React, { Fragment, useState } from "react";
import styled from "styled-components";
import { PanelBody } from "../../atoms/Panels";
import { ButtonPrimary, ButtonSecondary } from "../../atoms/Buttons/Buttons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { Textbox } from "../../atoms/Textboxes";
import { LabelNormal } from "../../atoms/Labels";
import { checkValidity } from "../../../utils/Validity";
import EditorImageScalePreview from "./EditorImageScalePreview";
import { AtomicBlockUtils, EditorState } from "draft-js";

const Body = styled(PanelBody)`
  padding: ${p => p.theme.size.tiny};
`;

const Footer = styled.div`
  min-height: 20px;
  text-align: right;
  border-top: 1px solid ${p => p.theme.rgbaColor.darker};
  padding: ${p => p.theme.size.exTiny} ${p => p.theme.size.tiny};

  button {
    margin-left: ${p => p.theme.size.exTiny};
    font-weight: normal;
    padding: ${p => p.theme.size.exTiny} ${p => p.theme.size.tiny};

    svg {
      margin-right: ${p => p.theme.size.exTiny};
    }
  }
`;

const FormRow = styled.div`
  margin-bottom: ${p => p.theme.size.tiny};

  ${LabelNormal} {
    display: block;
  }

  ${Textbox} {
    width: 100%;
  }
`;

export default props => {
  const formData = {
    src: {
      value: "",
      validation: {
        isRequired: true,
        isImageLink: true
      },
      isValid: false
    },
    width: {
      value: "auto",
      validation: {
        isRequired: false
      },
      isValid: true
    },
    height: {
      value: "auto",
      validation: {
        isRequired: false
      },
      isValid: true
    },
    alt: {
      value: "",
      validation: {
        isRequired: false
      },
      isValid: true
    }
  };

  const [imageData, setImageData] = useState(formData);

  const onClose = () => {
    props.onClose();
  };

  const handleKeyUp = e => {
    if (e.key === "Enter") {
      onAddImage();
    }
  };

  const handleInputChange = evt => {
    let data = imageData || {};
    const { name, value } = evt.target;

    data[name].isValid = checkValidity(data, value, name);
    data[name].value = value;

    setImageData({
      ...data
    });
  };

  const onAddImage = () => {
    const { src, width, height, alt } = imageData;
    const { editorState, onAddImage } = props;

    const contentState = editorState.getCurrentContent();
    const contentStateWithEntity = contentState.createEntity(
      "IMAGE",
      "IMMUTABLE",
      {
        src: src.value,
        height: height.value,
        width: width.value,
        alt: alt.value
      }
    );
    const entityKey = contentStateWithEntity.getLastCreatedEntityKey();
    const newEditorState = EditorState.set(editorState, {
      currentContent: contentStateWithEntity
    });

    const imageEditorState = AtomicBlockUtils.insertAtomicBlock(
      newEditorState,
      entityKey,
      " "
    );

    onAddImage(imageEditorState);
    onClose();
  };

  const onWithScaleChanged = e => {
    const value = e.target.value;
    const formData = imageData;
    if ("auto".indexOf(value) >= 0) {
      formData[e.target.name].value = value;
    } else if (!value || isNaN(value)) {
      formData[e.target.name].value = "auto";
    } else {
      const newSize = parseFloat(value);
      formData[e.target.name].value = newSize;
    }

    setImageData({
      ...formData
    });
  };

  const { src, width, height, alt } = imageData;
  const { isValid } = src;
  return (
    <Fragment>
      <Body>
        <FormRow>
          <LabelNormal>Đường dẫn tới hình ảnh</LabelNormal>
          <Textbox
            name="src"
            onKeyUp={handleKeyUp}
            value={src.value}
            autoComplete="off"
            onChange={e => handleInputChange(e)}
          />
        </FormRow>
        <EditorImageScalePreview
          src={src.value}
          width={width.value}
          height={height.value}
          alt={alt.value}
          handleInputChange={handleInputChange}
          onWithScaleChanged={onWithScaleChanged}
        />
      </Body>
      <Footer>
        <ButtonSecondary size="sm" onClick={onClose}>
          Đóng
        </ButtonSecondary>
        <ButtonPrimary size="sm" onClick={onAddImage} disabled={!isValid}>
          <FontAwesomeIcon icon="check" />
          Lưu
        </ButtonPrimary>
      </Footer>
    </Fragment>
  );
};
