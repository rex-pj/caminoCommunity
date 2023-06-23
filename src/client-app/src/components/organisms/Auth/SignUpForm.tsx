import * as React from "react";
import { useState } from "react";
import styled from "styled-components";
import { SecondaryTextbox } from "../../../components/atoms/Textboxes";
import { PanelBody, PanelFooter } from "../../../components/molecules/Panels";
import { LabelNormal } from "../../../components/atoms/Labels";
import { ButtonSecondary } from "../../../components/atoms/Buttons/Buttons";
import AuthNavigation from "./AuthNavigation";
import AuthBanner from "./AuthBanner";
import { validateEmail } from "../../../utils/Validity";
import { ErrorBox } from "../../molecules/NotificationBars/NotificationBoxes";
import { useTranslation } from "react-i18next";
import { useForm } from "react-hook-form";
import { ValidationWarningMessage } from "../../ErrorMessage";

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

interface SignUpFormProps {
  signUp: (e: any) => Promise<any>;
  isFormEnabled: boolean;
}

const SignUpForm = (props: SignUpFormProps) => {
  const { t } = useTranslation();
  const {
    register,
    handleSubmit,
    watch,
    formState: { errors },
  } = useForm();
  const [error, setError] = useState<string>();

  const onSignUp = async (data: any) => {
    const fromData = { ...data };
    return props.signUp(fromData).catch(() => {
      setError("Có lỗi xảy ra trong quá trình đăng ký");
    });
  };

  return (
    <form onSubmit={handleSubmit(onSignUp)} method="POST">
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
                {...register("lastname", {
                  required: {
                    value: true,
                    message: "This field is required",
                  },
                })}
                autoComplete="off"
                placeholder={t("please_input_your_lastname").toString()}
              />
              {errors.lastname && (
                <ValidationWarningMessage>
                  {errors.lastname.message?.toString()}
                </ValidationWarningMessage>
              )}
            </FormRow>
            <FormRow>
              <Label>{t("firstname_label")}</Label>
              <Textbox
                {...register("firstname", {
                  required: {
                    value: true,
                    message: "This field is required",
                  },
                })}
                autoComplete="off"
                placeholder={t("please_input_your_firstname").toString()}
              />
              {errors.firstname && (
                <ValidationWarningMessage>
                  {errors.firstname.message?.toString()}
                </ValidationWarningMessage>
              )}
            </FormRow>
            <FormRow>
              <Label>{t("email_label")}</Label>
              <Textbox
                type="email"
                {...register("email", {
                  required: {
                    value: true,
                    message: "This field is required",
                  },
                  validate: (val) => {
                    const isEmailValid = validateEmail(val);
                    if (!isEmailValid) {
                      return "Invalid email address";
                    }
                  },
                })}
                autoComplete="off"
                placeholder={t("please_input_your_email").toString()}
              />
              {errors.email && (
                <ValidationWarningMessage>
                  {errors.email.message?.toString()}
                </ValidationWarningMessage>
              )}
            </FormRow>
            <FormRow>
              <Label>{t("password_label")}</Label>
              <Textbox
                autoComplete="off"
                placeholder={t("please_input_your_password").toString()}
                type="password"
                {...register("password", {
                  required: {
                    value: true,
                    message: "This field is required",
                  },
                  minLength: {
                    value: 6,
                    message: "Must be more than 6 characters",
                  },
                })}
              />
              {errors.password && (
                <ValidationWarningMessage>
                  {errors.password.message?.toString()}
                </ValidationWarningMessage>
              )}
            </FormRow>
            <FormRow>
              <Label>{t("confirm_password_label")}</Label>
              <Textbox
                autoComplete="off"
                placeholder={t("please_confirm_your_password").toString()}
                type="password"
                {...register("confirmPassword", {
                  required: {
                    value: true,
                    message: "This field is required",
                  },
                  minLength: {
                    value: 6,
                    message: "Must be more than 6 characters",
                  },
                  validate: (val) => {
                    if (watch("password") !== val) {
                      return "Your password does not match";
                    }
                  },
                })}
              />
              {errors.confirmPassword && (
                <ValidationWarningMessage>
                  {errors.confirmPassword.message?.toString()}
                </ValidationWarningMessage>
              )}
            </FormRow>
            <FormFooter>
              <SubmitButton type="submit" disabled={!props.isFormEnabled}>
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
