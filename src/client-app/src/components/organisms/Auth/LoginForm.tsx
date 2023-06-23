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
import { ErrorBox } from "../../molecules/NotificationBars/NotificationBoxes";
import bgUrl from "../../../assets/images/logo.png";
import { useTranslation } from "react-i18next";
import { useForm } from "react-hook-form";
import { ValidationWarningMessage } from "../../ErrorMessage";
import { validateEmail } from "../../../utils/Validity";

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
  const [error, setError] = useState<string>();
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm();

  const onlogin = (data: any) => {
    const { isRemember } = data;
    const loginRequest: any = { ...data };

    props
      .onlogin(loginRequest, isRemember)
      .catch(() => setError("Có lỗi xảy ra trong quá trình đăng nhập"));
  };

  return (
    <form onSubmit={handleSubmit(onlogin)} method="POST">
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
                {...register("username", {
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
                placeholder={t("please_input_your_email").toString()}
                type="email"
              />
              {errors.username && (
                <ValidationWarningMessage>
                  {errors.username.message?.toString()}
                </ValidationWarningMessage>
              )}
            </FormRow>
            <FormRow>
              <Label>{t("password_label")}</Label>
              <Textbox
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
                placeholder={t("please_input_your_password").toString()}
                type="password"
              />
              {errors.password && (
                <ValidationWarningMessage>
                  {errors.password.message?.toString()}
                </ValidationWarningMessage>
              )}
            </FormRow>
            <FormRow className="mt-3">
              <input
                {...register("isRemember")}
                type="checkbox"
                id="isRemember"
              ></input>
              <Label htmlFor="isRemember">{t("remember_label")}</Label>
            </FormRow>
            <FormFooter>
              <SubmitButton disabled={!props.isFormEnabled} type="submit">
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
