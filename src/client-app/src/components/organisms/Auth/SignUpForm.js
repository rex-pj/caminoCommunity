import React, { useState } from "react";
import styled from "styled-components";
import { SecondaryTextbox } from "../../../components/atoms/Textboxes";
import { SelectionPrimary } from "../../../components/atoms/Selections";
import { PanelBody, PanelFooter } from "../../../components/molecules/Panels";
import { LabelNormal } from "../../../components/atoms/Labels";
import { ButtonSecondary } from "../../../components/atoms/Buttons/Buttons";
import AuthNavigation from "../../../components/organisms/Navigation/AuthNavigation";
import AuthBanner from "../../../components/organisms/Banner/AuthBanner";
import DateSelector from "../../../components/organisms/DateSelector";
import { checkValidity } from "../../../utils/Validity";
import { ErrorBox } from "../../molecules/NotificationBars/NotificationBoxes";
import signupModel from "../../../models/signupModel";

const Textbox = styled(SecondaryTextbox)`
  border-radius: ${(p) => p.theme.size.normal};
  border: 1px solid ${(p) => p.theme.color.neutralBg};
  background-color: ${(p) => p.theme.color.lightBg};
  width: 100%;
  color: ${(p) => p.theme.color.darkText};
  padding: ${(p) => p.theme.size.tiny};

  ::placeholder {
    color: ${(p) => p.theme.color.darkText};
    font-size: ${(p) => p.theme.fontSize.small};
  }

  :focus {
    background-color: ${(p) => p.theme.color.neutralBg};
    color: ${(p) => p.theme.color.darkText};
  }

  &.invalid {
    border: 1px solid ${(p) => p.theme.color.dangerBg};
  }
`;

const Selection = styled(SelectionPrimary)`
  border-radius: ${(p) => p.theme.size.normal};
  border: 1px solid ${(p) => p.theme.color.primaryBg};
  background-color: ${(p) => p.theme.color.lightBg};
  width: 100%;
  color: ${(p) => p.theme.color.darkText};
  padding: 0 ${(p) => p.theme.size.tiny};
  font-size: ${(p) => p.theme.fontSize.small};

  :focus {
    background-color: ${(p) => p.theme.color.neutralBg};
    background-color: ${(p) => p.theme.color.moreDark};
  }
`;

const FormFooter = styled(PanelFooter)`
  text-align: center;
`;

const Label = styled(LabelNormal)`
  margin-left: ${(p) => p.theme.size.tiny};
  margin-bottom: 0;
  font-size: ${(p) => p.theme.fontSize.small};
`;

const FormRow = styled.div`
  margin-bottom: ${(p) => p.theme.size.tiny};
`;

const SubmitButton = styled(ButtonSecondary)`
  font-size: ${(p) => p.theme.fontSize.small};
  border: 1px solid ${(p) => p.theme.color.primaryBg};

  :hover {
    color: ${(p) => p.theme.color.neutralText};
  }

  :disabled {
    color: ${(p) => p.theme.color.primaryText};
    cursor: auto;
  }
`;

const BirthDateSelector = styled(DateSelector)`
  select {
    border-radius: ${(p) => p.theme.size.normal};
    border: 1px solid ${(p) => p.theme.color.primaryBg};
    background-color: ${(p) => p.theme.color.lightBg};
    color: ${(p) => p.theme.color.darkText};
    font-size: ${(p) => p.theme.fontSize.small};
  }

  &.invalid select {
    border: 1px solid ${(p) => p.theme.color.dangerBg};
    color: ${(p) => p.theme.color.dangerText};
  }
`;

const SignUpForm = (props) => {
  let [formData, setFormData] = useState(signupModel);
  const [error, setError] = useState();

  const handleInputBlur = (evt) => {
    alertInvalidForm(evt.target);
  };

  const handleInputChange = (evt) => {
    let data = formData || {};
    const { name, value } = evt.target;

    // Validate when input
    data[name].isValid = checkValidity(data, value, name);
    data[name].value = value;

    alertInvalidForm(evt.target);
    setFormData({ ...data });
  };

  const alertInvalidForm = (target) => {
    const { name } = target;
    if (!formData[name].isValid) {
      target.classList.add("invalid");
    } else {
      target.classList.remove("invalid");
    }
  };

  const onSignUp = (e) => {
    e.preventDefault();

    let isFormValid = true;
    for (let formIdentifier in formData) {
      isFormValid = formData[formIdentifier].isValid && isFormValid;

      if (!isFormValid) {
        showError(`Dữ liệu của ${formIdentifier} không hợp lệ`);
      }
    }

    if (!!isFormValid) {
      const signUpData = {};
      for (const formIdentifier in formData) {
        signUpData[formIdentifier] = formData[formIdentifier].value;
      }

      props.signUp(signUpData).catch(() => {
        showError("Có lỗi xảy ra trong quá trình đăng ký");
      });
    }
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

  const isFormCheckValid = checkIsFormValid();

  const showError = (message) => {
    setError(message);
  };

  return (
    <form onSubmit={(e) => onSignUp(e)} method="POST">
      <div className="row g-0">
        <div className="col col-12 col-sm-7">
          <AuthBanner
            icon="signature"
            title="Sign Up"
            instruction="Sign up here to join with other farmers"
          />
        </div>
        <div className="col col-12 col-sm-5">
          <AuthNavigation />
          <PanelBody>
            <FormRow>{error ? <ErrorBox>{error}</ErrorBox> : null}</FormRow>
            <FormRow>
              <Label>Lastname</Label>
              <Textbox
                autoComplete="off"
                placeholder="Your Lastname"
                name="lastname"
                onChange={(e) => handleInputChange(e)}
                onBlur={(e) => handleInputBlur(e)}
              />
            </FormRow>
            <FormRow>
              <Label>Firstname</Label>
              <Textbox
                autoComplete="off"
                placeholder="Your Firstname"
                name="firstname"
                onChange={(e) => handleInputChange(e)}
                onBlur={(e) => handleInputBlur(e)}
              />
            </FormRow>
            <FormRow>
              <Label>E-mail</Label>
              <Textbox
                autoComplete="off"
                placeholder="Your e-mail"
                type="email"
                name="email"
                onChange={(e) => handleInputChange(e)}
                onBlur={(e) => handleInputBlur(e)}
              />
            </FormRow>
            <FormRow>
              <Label>Password</Label>
              <Textbox
                autoComplete="new-password"
                placeholder="Your password"
                type="password"
                name="password"
                onChange={(e) => handleInputChange(e)}
                onBlur={(e) => handleInputBlur(e)}
              />
            </FormRow>
            <FormRow>
              <Label>Confirm password</Label>
              <Textbox
                autoComplete="off"
                placeholder="Confirm your password"
                type="password"
                name="confirmPassword"
                onChange={(e) => handleInputChange(e)}
                onBlur={(e) => handleInputBlur(e)}
              />
            </FormRow>
            <FormRow>
              <Label>Date of Birth</Label>
              <BirthDateSelector
                name="birthDate"
                onDateChanged={(e) => handleInputChange(e)}
                onBlur={handleInputBlur}
              />
            </FormRow>
            <FormRow>
              <Label>Sex</Label>
              <Selection
                placeholder="Male/Female"
                name="genderId"
                onChange={(e) => handleInputChange(e)}
                defaultValue={1}
              >
                <option value={1}>Male</option>
                <option value={2}>Female</option>
              </Selection>
            </FormRow>
            <FormFooter>
              <SubmitButton
                type="submit"
                disabled={!props.isFormEnabled || !isFormCheckValid}
              >
                Sign Up
              </SubmitButton>
            </FormFooter>
          </PanelBody>
        </div>
      </div>
    </form>
  );
};

export default SignUpForm;
