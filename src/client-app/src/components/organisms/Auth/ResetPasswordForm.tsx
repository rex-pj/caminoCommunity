import * as React from "react";
import { useState, useEffect, useRef } from "react";
import styled from "styled-components";
import { SecondaryTextbox } from "../../../components/atoms/Textboxes";
import { PanelBody, PanelFooter } from "../../../components/molecules/Panels";
import { LabelNormal } from "../../../components/atoms/Labels";
import { ButtonSecondary } from "../../../components/atoms/Buttons/Buttons";
import ResetPasswordNavigation from "./ResetPasswordNavigation";
import { SecondaryHeading } from "../../atoms/Heading";
import { checkValidity } from "../../../utils/Validity";
import { ResetPasswordModel } from "../../../models/resetPasswordModel";
import { ErrorBox } from "../../molecules/NotificationBars/NotificationBoxes";
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
  cursor: pointer;
  border: 1px solid ${(p) => p.theme.color.primaryBg};

  :hover {
    color: ${(p) => p.theme.color.neutralText};
  }

  :disabled {
    color: ${(p) => p.theme.color.primaryText};
    cursor: auto;
  }
`;

const Instruction = styled.div`
  text-align: center;
  margin: ${(p) => p.theme.size.distance} ${(p) => p.theme.size.distance} 0
    ${(p) => p.theme.size.distance};
  padding: ${(p) => p.theme.size.distance};
  background: ${(p) => p.theme.rgbaColor.light};
  border-radius: ${(p) => p.theme.borderRadius.medium};

  ${SecondaryHeading} {
    font-size: ${(p) => p.theme.fontSize.giant};
    color: ${(p) => p.theme.color.neutralText};
    text-transform: uppercase;
  }

  p {
    color: ${(p) => p.theme.color.neutralText};
    margin-bottom: 0;
    font-size: ${(p) => p.theme.fontSize.small};
    font-weight: 600;
  }
`;

interface ResetPasswordFormProps {
  args?: {
    key?: string;
    email?: string;
  };
  resetPassword: (e: any) => Promise<any>;
}

const ResetPasswordForm = (props: ResetPasswordFormProps) => {
  const { args } = props;
  const { t } = useTranslation();
  const [formData, setFormData] = useState(new ResetPasswordModel());
  const [error, setError] = useState<string>();
  const initialRef = useRef(false);

  useEffect(() => {
    if (args && !initialRef.current) {
      let data = { ...formData } || new ResetPasswordModel();
      data.email.value = args.email;
      data.email.isValid = checkValidity(data, args.email, "email");

      data.key.value = args.key;
      data.key.isValid = checkValidity(data, args.key, "key");
      setFormData({ ...data });
      initialRef.current = true;
    }
  }, [args, formData]);

  const handleInputBlur = (
    evt: React.FocusEvent<HTMLInputElement, Element>
  ) => {
    alertInvalidForm(evt.target);
  };

  const handleInputChange = (evt: React.ChangeEvent<HTMLInputElement>) => {
    let data = formData || new ResetPasswordModel();
    const { name, value } = evt.target;

    // Validate when input
    data[name].isValid = checkValidity(data, value, name);
    data[name].value = value;

    alertInvalidForm(evt.target);
    setFormData({ ...data });
  };

  const alertInvalidForm = (target: EventTarget & HTMLInputElement) => {
    const { name } = target;
    if (!formData[name].isValid) {
      target.classList.add("invalid");
    } else {
      target.classList.remove("invalid");
    }
  };

  const onResetPassword = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    let isFormValid = true;
    for (let formIdentifier in formData) {
      isFormValid = formData[formIdentifier].isValid && isFormValid;

      if (!isFormValid) {
        showError(`Dữ liệu của ${formIdentifier} không hợp lệ`);
      }
    }

    if (isFormValid) {
      const resetPasswordRequest: any = {};
      for (const formIdentifier in formData) {
        resetPasswordRequest[formIdentifier] = formData[formIdentifier].value;
      }

      props.resetPassword(resetPasswordRequest).catch(() => {
        showError(
          "Có lỗi xảy ra khi thay đổi password của bạn, vui lòng thử lại!"
        );
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

  const showError = (message: string) => {
    setError(message);
  };

  const isFormCheckValid = checkIsFormValid();

  return (
    <form onSubmit={(e) => onResetPassword(e)} method="POST">
      <ResetPasswordNavigation />
      <Instruction>
        <p>
          Password must contain lower case, upper case letters, numeric, special
          characters
        </p>
      </Instruction>
      <PanelBody>
        <FormRow>{error ? <ErrorBox>{error}</ErrorBox> : null}</FormRow>
        <FormRow>
          <Label>{t("new_password_label")}</Label>
          <Textbox
            autoComplete="off"
            placeholder={t("please_input_new_password")}
            type="password"
            name="password"
            onChange={(e) => handleInputChange(e)}
            onBlur={(e) => handleInputBlur(e)}
          />
        </FormRow>
        <FormRow>
          <Label>{t("confirm_new_password_label")}</Label>
          <Textbox
            autoComplete="off"
            placeholder={t("please_confirm_new_password")}
            type="password"
            name="confirmPassword"
            onChange={(e) => handleInputChange(e)}
            onBlur={(e) => handleInputBlur(e)}
          />
        </FormRow>
        <FormFooter>
          <SubmitButton type="submit" disabled={!isFormCheckValid}>
            {t("change_password")}
          </SubmitButton>
        </FormFooter>
      </PanelBody>
    </form>
  );
};

export default ResetPasswordForm;
