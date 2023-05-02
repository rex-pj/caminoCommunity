import React, { useState } from "react";
import styled from "styled-components";
import { SecondaryTextbox } from "../../../components/atoms/Textboxes";
import { SelectionPrimary } from "../../../components/atoms/Selections";
import { PanelBody, PanelFooter } from "../../../components/molecules/Panels";
import { LabelNormal } from "../../../components/atoms/Labels";
import { ButtonSecondary } from "../../../components/atoms/Buttons/Buttons";
import AuthNavigation from "./AuthNavigation";
import AuthBanner from "./AuthBanner";
import DateSelector from "../../../components/organisms/DateSelector";
import { checkValidity } from "../../../utils/Validity";
import { ErrorBox } from "../../molecules/NotificationBars/NotificationBoxes";
import signupModel from "../../../models/signupModel";
import { useTranslation } from "react-i18next";

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
  const { t } = useTranslation();
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

    if (isFormValid) {
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
            title={t("sign_up")}
            instruction={t("signup_instruction")}
          />
        </div>
        <div className="col col-12 col-sm-5">
          <AuthNavigation />
          <PanelBody>
            <FormRow>{error ? <ErrorBox>{error}</ErrorBox> : null}</FormRow>
            <FormRow>
              <Label>{t("lastname_label")}</Label>
              <Textbox
                autoComplete="off"
                placeholder={t("please_input_your_lastname")}
                name="lastname"
                onChange={(e) => handleInputChange(e)}
                onBlur={(e) => handleInputBlur(e)}
              />
            </FormRow>
            <FormRow>
              <Label>{t("firstname_label")}</Label>
              <Textbox
                autoComplete="off"
                placeholder={t("please_input_your_firstname")}
                name="firstname"
                onChange={(e) => handleInputChange(e)}
                onBlur={(e) => handleInputBlur(e)}
              />
            </FormRow>
            <FormRow>
              <Label>{t("email_label")}</Label>
              <Textbox
                autoComplete="off"
                placeholder={t("please_input_your_email")}
                type="email"
                name="email"
                onChange={(e) => handleInputChange(e)}
                onBlur={(e) => handleInputBlur(e)}
              />
            </FormRow>
            <FormRow>
              <Label>{t("password_label")}</Label>
              <Textbox
                autoComplete="new-password"
                placeholder={t("please_input_your_password")}
                type="password"
                name="password"
                onChange={(e) => handleInputChange(e)}
                onBlur={(e) => handleInputBlur(e)}
              />
            </FormRow>
            <FormRow>
              <Label>{t("confirm_password_label")}</Label>
              <Textbox
                autoComplete="off"
                placeholder={t("please_confirm_your_password")}
                type="password"
                name="confirmPassword"
                onChange={(e) => handleInputChange(e)}
                onBlur={(e) => handleInputBlur(e)}
              />
            </FormRow>
            <FormRow>
              <Label>{t("date_of_birth_label")}</Label>
              <BirthDateSelector
                name="birthDate"
                onDateChanged={(e) => handleInputChange(e)}
                onBlur={handleInputBlur}
              />
            </FormRow>
            <FormRow>
              <Label>{t("sex_label")}</Label>
              <Selection
                placeholder={t("male_or_female")}
                name="genderId"
                onChange={(e) => handleInputChange(e)}
                defaultValue={1}
              >
                <option value={1}>{t("male")}</option>
                <option value={2}>{t("female")}</option>
              </Selection>
            </FormRow>
            <FormFooter>
              <SubmitButton
                type="submit"
                disabled={!props.isFormEnabled || !isFormCheckValid}
              >
                {t("sign_up")}
              </SubmitButton>
            </FormFooter>
          </PanelBody>
        </div>
      </div>
    </form>
  );
};

export default SignUpForm;
