import React, { Fragment } from "react";
import styled from "styled-components";
import { PanelBody } from "../../atoms/Panels";
import { ButtonPrimary, ButtonSecondary } from "../../atoms/Buttons/Buttons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

const Body = styled(PanelBody)`
  padding: ${p => p.theme.size.tiny};
`;

const Footer = styled.div`
  min-height: 20px;
  text-align: right;
  border-top: 1px solid ${p => p.theme.color.light};
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

export default props => {
  const onClose = () => {
    props.onClose();
  };

  const onAddImage = () => {
    props.onAddImage();
  };

  return (
    <Fragment>
      <Body>Image Linked</Body>
      <Footer>
        <ButtonSecondary size="sm" onClick={onClose}>
          Đóng
        </ButtonSecondary>
        <ButtonPrimary size="sm" onClick={onAddImage}>
          <FontAwesomeIcon icon="check" />
          Lưu
        </ButtonPrimary>
      </Footer>
    </Fragment>
  );
};
