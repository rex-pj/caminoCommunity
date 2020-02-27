import React, { Fragment } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import styled from "styled-components";
import { ButtonPrimary, ButtonSecondary } from "../../atoms/Buttons/Buttons";
import TabPanel from "../Tabs/TabPanel";

const Body = styled.div`
  height: auto;
  padding: ${p => p.theme.size.tiny} ${p => p.theme.size.distance};
`;

const Footer = styled.div`
  min-height: 20px;
  text-align: right;
  border-top: 1px solid ${p => p.theme.color.light};
  padding: ${p => p.theme.size.exTiny} ${p => p.theme.size.distance};

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

  const onAddImage = () => {};

  return (
    <Fragment>
      <Body>
        <TabPanel />
      </Body>
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
