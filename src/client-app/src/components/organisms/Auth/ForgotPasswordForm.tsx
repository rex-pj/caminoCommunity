import * as React from "react";
import { useState } from "react";
import styled from "styled-components";
import { SecondaryTextbox } from "../../atoms/Textboxes";
import { PanelBody, PanelFooter } from "../../molecules/Panels";
import { LabelNormal } from "../../atoms/Labels";
import { ButtonSecondary } from "../../atoms/Buttons/Buttons";
import ForgotPasswordNavigation from "./ForgotPasswordNavigation";
import AuthBanner from "./AuthBanner";
import { ForgotPasswordModel } from "../../../models/forgotPasswordModel";
import { checkValidity } from "../../../utils/Validity";
import {
  ErrorBox,
  SuccessBox,
} from "../../molecules/NotificationBars/NotificationBoxes";
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
  font-weight: 600;
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

interface ForgotPasswordFormProps {
  onForgotPassword: (e: any) => Promise<any>;
}

const ForgotPasswordForm = (props: ForgotPasswordFormProps) => {
  const { t } = useTranslation();
  const [formData, setFormData] = useState(new ForgotPasswordModel());
  const [isFormEnabled, setFormEnabled] = useState(false);
  const [isSubmitted, setSubmitted] = useState(false);
  const [error, setError] = useState<string>();
  const [successMessage, setSuccessMessage] = useState<string>();

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
    if (isSubmitted) {
      setSubmitted(false);
    }

    const data = formData || new ForgotPasswordModel();
    const { name, value } = evt.target;

    const prevValid = data[name].isValid;
    // Validate when input
    data[name].isValid = checkValidity(data, value, name);
    data[name].value = value;

    if (prevValid !== data[name].isValid) {
      setFormData({ ...data });
    }

    if (formData[name].isValid && !isFormEnabled) {
      setFormEnabled(true);
    }
  };

  const onUpdate = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    setError("");
    setSuccessMessage("");
    if (isFormEnabled) {
      setFormEnabled(false);
    }

    if (!isSubmitted) {
      setSubmitted(true);
    }

    let isFormValid = true;
    for (let formIdentifier in formData) {
      isFormValid = formData[formIdentifier].isValid && isFormValid;

      if (!isFormValid) {
        showError(`Dữ liệu của ${formIdentifier} không hợp lệ`);
      }
    }

    if (isFormValid) {
      const requestData: any = {};
      for (const formIdentifier in formData) {
        requestData[formIdentifier] = formData[formIdentifier].value;
      }

      await props
        .onForgotPassword(requestData)
        .then(() => {
          showError("");
          setSuccessMessage(
            "Thông báo quên mật khẩu thành công, vui lòng kiểm tra email và làm theo hướng dẫn!"
          );
          setFormEnabled(true);
        })
        .catch(() => {
          setSuccessMessage("");
          showError("Có lỗi xảy ra trong quá trình thông báo quên mật khẩu");
          setFormEnabled(true);
        });
    }
  };

  const showError = (message: string) => {
    setError(message);
  };

  return (
    <form onSubmit={(e) => onUpdate(e)} method="POST">
      <div className="row g-0">
        <div className="col col-12 col-sm-7">
          <AuthBanner icon="unlock-alt" title={t("recover_your_password")} />
        </div>
        <div className="col col-12 col-sm-5">
          <ForgotPasswordNavigation />
          <PanelBody>
            <FormRow>
              {error ? <ErrorBox>{error}</ErrorBox> : null}
              {successMessage ? (
                <SuccessBox>{successMessage}</SuccessBox>
              ) : null}
            </FormRow>
            <FormRow>
              <Label>E-mail</Label>
              <Textbox
                placeholder={t("please_input_your_email").toString()}
                type="email"
                name="email"
                autoComplete="off"
                onChange={(e) => handleInputChange(e)}
                onBlur={(e) => handleInputBlur(e)}
              />
            </FormRow>
            <FormFooter>
              <SubmitButton disabled={!isFormEnabled} type="submit">
                {t("change_password")}
              </SubmitButton>
            </FormFooter>
          </PanelBody>
        </div>
      </div>
    </form>
  );
};

export default ForgotPasswordForm;
