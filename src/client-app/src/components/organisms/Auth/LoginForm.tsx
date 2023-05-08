import * as React from "react";
import { useState } from "react";
import styled from "styled-components";
import { Link } from "react-router-dom";
import { SecondaryTextbox } from "../../atoms/Textboxes";
import { PanelBody, PanelFooter } from "../../molecules/Panels";
import { LabelNormal } from "../../atoms/Labels";
import { ButtonSecondary } from "../../atoms/Buttons/Buttons";
import AuthNavigation from "./AuthNavigation";
import AuthBanner from "./AuthBanner";
import { checkValidity } from "../../../utils/Validity";
import { LoginModel } from "../../../models/loginModel";
import { ErrorBox } from "../../molecules/NotificationBars/NotificationBoxes";
import bgUrl from "../../../assets/images/logo.png";
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

interface LoginFormProps {
  onlogin: (e: any, isRember: boolean) => Promise<any>;
  isFormEnabled: boolean;
}

const LoginForm = (props: LoginFormProps) => {
  const { t } = useTranslation();
  const [isRemember, setRemember] = useState(false);
  const [formData, setFormData] = useState(new LoginModel());
  const [error, setError] = useState<string>();

  const handleInputBlur = (
    evt: React.FocusEvent<HTMLInputElement, Element>
  ) => {
    const { name } = evt.target;
    if (!formData[name].isValid) {
      evt.target.classList.add("invalid");
    } else {
      evt.target.classList.remove("invalid");
    }
  };

  const handleInputChange = (evt: React.ChangeEvent<HTMLInputElement>) => {
    let data = formData || new LoginModel();
    const { name, value } = evt.target;

    // Validate when input
    data[name].isValid = checkValidity(data, value, name);
    data[name].value = value;

    setFormData({ ...data });
  };

  const handleCheckboxChange = (evt: React.ChangeEvent<HTMLInputElement>) => {
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

  const onlogin = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    let isFormValid = true;
    for (let formIdentifier in formData) {
      isFormValid = formData[formIdentifier].isValid && isFormValid;

      if (!isFormValid) {
        showError(`Dữ liệu của ${formIdentifier} không hợp lệ`);
      }
    }

    if (isFormValid) {
      const loginRequest: any = {};
      for (const formIdentifier in formData) {
        loginRequest[formIdentifier] = formData[formIdentifier].value;
      }

      props
        .onlogin(loginRequest, isRemember)
        .catch(() => showError("Có lỗi xảy ra trong quá trình đăng nhập"));
    }
  };

  const showError = (message: string) => {
    setError(message);
  };

  const isFormValid = checkIsFormValid();
  return (
    <form onSubmit={(e) => onlogin(e)} method="POST">
      <div className="row g-0">
        <div className="col col-12 col-sm-7">
          <AuthBanner
            imageUrl={`${bgUrl}`}
            title={t("login")}
            instruction={t("join_us_instruction")}
          />
        </div>
        <div className="col col-12 col-sm-5">
          <AuthNavigation />
          <PanelBody>
            <FormRow>{error ? <ErrorBox>{error}</ErrorBox> : null}</FormRow>
            <FormRow>
              <Label>{t("email_label")}</Label>
              <Textbox
                placeholder={t("please_input_your_email")}
                type="email"
                name="username"
                onChange={(e) => handleInputChange(e)}
                onBlur={(e) => handleInputBlur(e)}
              />
            </FormRow>
            <FormRow>
              <Label>{t("password_label")}</Label>
              <Textbox
                placeholder={t("please_input_your_password")}
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
              <Label htmlFor="isRemember">{t("remember_label")}</Label>
            </FormRow>
            <FormFooter>
              <SubmitButton
                disabled={!props.isFormEnabled || !isFormValid}
                type="submit"
              >
                {t("login")}
              </SubmitButton>
            </FormFooter>
            <ForgotPasswordRow>
              <Link to="/auth/forgot-password">
                {t("forgot_password_title")}
              </Link>
            </ForgotPasswordRow>
          </PanelBody>
        </div>
      </div>
    </form>
  );
};

export default LoginForm;
