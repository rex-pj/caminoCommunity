import React, { Fragment, useState } from "react";
import styled from "styled-components";
import { PanelBody } from "../../atoms/Panels";
import { ButtonPrimary, ButtonSecondary } from "../../atoms/Buttons/Buttons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { Textbox } from "../../atoms/Textboxes";
import { LabelNormal } from "../../atoms/Labels";
import { checkValidity } from "../../../utils/Validity";

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
    link: {
      value: "",
      validation: {
        isRequired: true,
        isImageLink: true
      },
      isValid: false
    }
  };

  const [imageData, setImageData] = useState(formData);

  const onClose = () => {
    props.onClose();
  };

  const handleKeyUp = e => {
    if (e.key === "Enter") {
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
    props.onAddImage();
  };

  const { link } = imageData;
  const { isValid } = link;
  return (
    <Fragment>
      <Body>
        <FormRow>
          <LabelNormal>Đường dẫn tới hình ảnh</LabelNormal>
          <Textbox
            name="link"
            onKeyUp={handleKeyUp}
            value={link.value}
            autoComplete="off"
            onChange={e => handleInputChange(e)}
          />
        </FormRow>
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
