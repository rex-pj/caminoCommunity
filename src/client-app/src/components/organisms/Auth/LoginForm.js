import React, { useState } from "react";
import styled from "styled-components";
import { Link } from "react-router-dom";
import { SecondaryTextbox } from "../../atoms/Textboxes";
import { PanelBody, PanelFooter } from "../../molecules/Panels";
import { LabelNormal } from "../../atoms/Labels";
import { ButtonSecondary } from "../../atoms/Buttons/Buttons";
import AuthNavigation from "../Navigation/AuthNavigation";
import AuthBanner from "../Banner/AuthBanner";
import { checkValidity } from "../../../utils/Validity";
import loginModel from "../../../models/loginModel";
import { ErrorBox } from "../../molecules/NotificationBars/NotificationBoxes";
import bgUrl from "../../../assets/images/logo.png";

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

const ForgotPasswordRow = styled(FormRow)`
  margin-top: ${(p) => p.theme.size.exTiny};
  font-size: ${(p) => p.theme.fontSize.small};
  a {
    color: ${(p) => p.theme.color.neutralText};
  }
  text-align: center;
`;

const LoginForm = (props) => {
  const [isRemember, setRemember] = useState(false);
  const [formData, setFormData] = useState(loginModel);
  const [error, setError] = useState();

  const handleInputBlur = (evt) => {
    const { name } = evt.target;
    if (!formData[name].isValid) {
      evt.target.classList.add("invalid");
    } else {
      evt.target.classList.remove("invalid");
    }
  };

  const handleInputChange = (evt) => {
    let data = formData || {};
    const { name, value } = evt.target;

    // Validate when input
    data[name].isValid = checkValidity(data, value, name);
    data[name].value = value;

    setFormData({ ...data });
  };

  const handleCheckboxChange = (evt) => {
    const { checked } = evt.target;
    setRemember(checked);
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

  const onlogin = (e) => {
    e.preventDefault();

    let isFormValid = true;
    for (let formIdentifier in formData) {
      isFormValid = formData[formIdentifier].isValid && isFormValid;

      if (!isFormValid) {
        showError(`Dữ liệu của ${formIdentifier} không hợp lệ`);
      }
    }

    if (!!isFormValid) {
      const loginData = {};
      for (const formIdentifier in formData) {
        loginData[formIdentifier] = formData[formIdentifier].value;
      }

      props
        .onlogin(loginData, isRemember)
        .catch(() => showError("Có lỗi xảy ra trong quá trình đăng nhập"));
    }
  };

  const showError = (message) => {
    setError(message);
  };

  const isFormValid = checkIsFormValid();
  return (
    <form onSubmit={(e) => onlogin(e)} method="POST">
      <div className="row g-0">
        <div className="col col-12 col-sm-7">
          <AuthBanner
            imageUrl={`${bgUrl}`}
            title="Login"
            instruction="Join with us to connect with many other farmers"
          />
        </div>
        <div className="col col-12 col-sm-5">
          <AuthNavigation />
          <PanelBody>
            <FormRow>{error ? <ErrorBox>{error}</ErrorBox> : null}</FormRow>
            <FormRow>
              <Label>E-mail</Label>
              <Textbox
                placeholder="Please input your e-mail"
                type="email"
                name="username"
                onChange={(e) => handleInputChange(e)}
                onBlur={(e) => handleInputBlur(e)}
              />
            </FormRow>
            <FormRow>
              <Label>Password</Label>
              <Textbox
                placeholder="Password"
                type="password"
                name="password"
                onChange={(e) => handleInputChange(e)}
                onBlur={(e) => handleInputBlur(e)}
              />
            </FormRow>
            <FormRow className="mt-3">
              <input
                type="checkbox"
                name="isRemember"
                id="isRemember"
                onChange={(e) => handleCheckboxChange(e)}
              ></input>
              <Label htmlFor="isRemember">Remember?</Label>
            </FormRow>
            <FormFooter>
              <SubmitButton
                disabled={!props.isFormEnabled || !isFormValid}
                type="submit"
              >
                Login
              </SubmitButton>
            </FormFooter>
            <ForgotPasswordRow>
              <Link to="/auth/forgot-password">Forgot your password?</Link>
            </ForgotPasswordRow>
          </PanelBody>
        </div>
      </div>
    </form>
  );
};

export default LoginForm;
