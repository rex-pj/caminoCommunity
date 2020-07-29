import React, { Fragment, useState, useCallback } from "react";
import styled from "styled-components";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { PanelBody } from "../../atoms/Panels";
import { ButtonPrimary } from "../../atoms/Buttons/Buttons";
import LabelAndTextbox from "../../molecules/InfoWithLabels/LabelAndTextbox";
import passwordUpdateModel from "../../../models/passwordUpdateModel";
import { checkValidity } from "../../../utils/Validity";
import { PanelFooter } from "../../atoms/Panels";
import { QuaternaryDarkHeading } from "../../atoms/Heading";

const MainPanel = styled(PanelBody)`
  border-radius: ${(p) => p.theme.borderRadius.normal};
  box-shadow: ${(p) => p.theme.shadow.BoxShadow};
  margin-bottom: ${(p) => p.theme.size.normal};
  background-color: ${(p) => p.theme.color.white};
`;

const FormGroup = styled.div`
  margin-bottom: ${(p) => p.theme.size.exTiny};
  border-bottom: 1px solid ${(p) => p.theme.color.lighter};
`;

const Heading = styled(QuaternaryDarkHeading)`
  margin-bottom: ${(p) => p.theme.size.distance};
  margin-left: ${(p) => p.theme.size.exTiny};
`;

const SubmitButton = styled(ButtonPrimary)`
  font-size: ${(p) => p.theme.fontSize.small};
  cursor: pointer;

  :hover {
    color: ${(p) => p.theme.color.light};
  }

  :disabled {
    background-color: ${(p) => p.theme.color.primaryLight};
    color: ${(p) => p.theme.color.neutral};
    cursor: auto;
  }

  svg {
    margin-right: ${(p) => p.theme.size.exTiny};
  }
`;

const FormFooter = styled(PanelFooter)`
  padding-left: 0;
  padding-right: 0;
`;

export default (props) => {
  let formData = passwordUpdateModel;
  const [, updateState] = useState();
  const forceUpdate = useCallback(() => updateState({}), []);

  const onTextboxChange = (e) => {
    formData = formData || {};
    const { name, value } = e.target;

    // Validate when input
    formData[name].isValid = checkValidity(formData, value, name);
    formData[name].value = value;

    forceUpdate();
  };

  const checkIsFormValid = () => {
    let isFormValid = false;
    for (let formIdentifier in formData) {
      isFormValid = formData[formIdentifier].isValid;
      if (!isFormValid) {
        break;
      }
    }

    return isFormValid;
  };

  const onUpdate = (e) => {
    e.preventDefault();

    let isFormValid = true;
    for (let formIdentifier in formData) {
      isFormValid = formData[formIdentifier].isValid && isFormValid;

      if (!isFormValid) {
        props.showValidationError(
          "Thông tin bạn nhập có thể bị sai",
          "Có thể bạn nhập sai thông tin này, vui lòng kiểm tra và nhập lại"
        );
      }
    }

    if (!!isFormValid) {
      const profileData = {};
      for (const formIdentifier in formData) {
        profileData[formIdentifier] = formData[formIdentifier].value;
      }

      props.onUpdate(profileData);
    }
  };

  const { currentPassword, newPassword, confirmPassword } = formData;
  const isFormValid = checkIsFormValid();

  return (
    <Fragment>
      <Heading>Thay đổi mật khẩu</Heading>
      <MainPanel>
        <form onSubmit={(e) => onUpdate(e)} method="POST">
          <Fragment>
            <FormGroup>
              <LabelAndTextbox
                label="Mật khẩu hiện tại"
                name="currentPassword"
                type="password"
                placeholder="Nhập mật khẩu hiện tại"
                value={currentPassword.value}
                onChange={onTextboxChange}
              />
            </FormGroup>
            <FormGroup>
              <LabelAndTextbox
                label="Mật khẩu mới"
                name="newPassword"
                placeholder="Nhập mật khẩu mới"
                value={newPassword.value}
                type="password"
                onChange={onTextboxChange}
              />
            </FormGroup>
            <FormGroup>
              <LabelAndTextbox
                label="Tên hiển thị"
                name="confirmPassword"
                placeholder="Xác nhận lại mật khẩu mới"
                value={confirmPassword.value}
                type="password"
                onChange={onTextboxChange}
              />
            </FormGroup>
          </Fragment>
          <FormFooter>
            <SubmitButton
              type="submit"
              size="sm"
              disabled={!props.isFormEnabled || !isFormValid}
            >
              <FontAwesomeIcon icon="pencil-alt" />
              Cập Nhật
            </SubmitButton>
          </FormFooter>
        </form>
      </MainPanel>
    </Fragment>
  );
};
