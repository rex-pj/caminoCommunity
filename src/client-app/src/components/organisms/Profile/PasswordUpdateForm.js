import React, { Fragment, useState } from "react";
import styled from "styled-components";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { PanelBody } from "../../molecules/Panels";
import { ButtonPrimary } from "../../atoms/Buttons/Buttons";
import LabelAndTextbox from "../../molecules/InfoWithLabels/LabelAndTextbox";
import passwordUpdateModel from "../../../models/passwordUpdateModel";
import { checkValidity } from "../../../utils/Validity";
import { PanelFooter } from "../../molecules/Panels";
import { QuaternaryDarkHeading } from "../../atoms/Heading";
import { ErrorBox } from "../../molecules/NotificationBars/NotificationBoxes";

const MainPanel = styled(PanelBody)`
  border-radius: ${(p) => p.theme.borderRadius.normal};
  box-shadow: ${(p) => p.theme.shadow.BoxShadow};
  margin-bottom: ${(p) => p.theme.size.normal};
  background-color: ${(p) => p.theme.color.whiteBg};
`;

const FormGroup = styled.div`
  margin-bottom: ${(p) => p.theme.size.exTiny};
  border-bottom: 1px solid ${(p) => p.theme.color.neutralBg};
`;

const Heading = styled(QuaternaryDarkHeading)`
  margin-bottom: ${(p) => p.theme.size.distance};
  margin-left: ${(p) => p.theme.size.exTiny};
`;

const FormFooter = styled(PanelFooter)`
  padding-left: 0;
  padding-right: 0;
`;

const PasswordUpdateForm = (props) => {
  const [formData, setFormData] = useState(passwordUpdateModel);
  const [error, setError] = useState();

  const onTextboxChange = (e) => {
    let data = formData || {};
    const { name, value } = e.target;

    // Validate when input
    data[name].isValid = checkValidity(data, value, name);
    data[name].value = value;

    setFormData({ ...data });
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
        showError(`Dữ liệu của ${formIdentifier} không hợp lệ`);
      }
    }

    if (!!isFormValid) {
      const profileData = {};
      for (const formIdentifier in formData) {
        profileData[formIdentifier] = formData[formIdentifier].value;
      }

      props
        .onUpdate(profileData)
        .catch(() =>
          showError("Có lỗi xảy ra trong quá trình thay đổi mật khẩu!")
        );
    }
  };

  const showError = (message) => {
    setError(message);
  };

  const { currentPassword, newPassword, confirmPassword } = formData;
  const isFormValid = checkIsFormValid();

  return (
    <Fragment>
      <Heading>Change Password</Heading>
      <MainPanel>
        <form onSubmit={(e) => onUpdate(e)} method="POST">
          <Fragment>
            <FormGroup>{error ? <ErrorBox>{error}</ErrorBox> : null}</FormGroup>
            <FormGroup>
              <LabelAndTextbox
                label="Current password"
                name="currentPassword"
                type="password"
                placeholder="Enter current password"
                value={currentPassword.value}
                onChange={onTextboxChange}
              />
            </FormGroup>
            <FormGroup>
              <LabelAndTextbox
                label="New password"
                name="newPassword"
                placeholder="Enter new password"
                value={newPassword.value}
                type="password"
                onChange={onTextboxChange}
              />
            </FormGroup>
            <FormGroup>
              <LabelAndTextbox
                label="Confirm new password"
                name="confirmPassword"
                placeholder="Confirm new password"
                value={confirmPassword.value}
                type="password"
                onChange={onTextboxChange}
              />
            </FormGroup>
          </Fragment>
          <FormFooter>
            <ButtonPrimary
              type="submit"
              size="xs"
              disabled={!props.isFormEnabled || !isFormValid}
            >
              <FontAwesomeIcon icon="pencil-alt" className="me-1" />
              Change Password
            </ButtonPrimary>
          </FormFooter>
        </form>
      </MainPanel>
    </Fragment>
  );
};

export default PasswordUpdateForm;
