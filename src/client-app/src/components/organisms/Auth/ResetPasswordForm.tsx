import * as React from "react";
import { useState } from "react";
import styled from "styled-components";
import { SecondaryTextbox } from "../../../components/atoms/Textboxes";
import { PanelBody, PanelFooter } from "../../../components/molecules/Panels";
import { LabelNormal } from "../../../components/atoms/Labels";
import { ButtonSecondary } from "../../../components/atoms/Buttons/Buttons";
import ResetPasswordNavigation from "./ResetPasswordNavigation";
import { SecondaryHeading } from "../../atoms/Heading";
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

interface Props {
  args?: {
    key?: string;
    email?: string;
  };
  resetPassword: (e: any) => Promise<any>;
}

const ResetPasswordForm = (props: Props) => {
  const { args } = props;
  const { t } = useTranslation();
  const [error, setError] = useState<string>();
  const {
    register,
    handleSubmit,
    watch,
    formState: { errors },
  } = useForm({
    defaultValues: {
      email: args?.email,
      key: args?.key,
      password: null,
      confirmPassword: null,
    },
  });

  const onResetPassword = (data: any) => {
    const resetPasswordRequest: any = { ...data };
    props.resetPassword(resetPasswordRequest).catch(() => {
      setError(
        "Có lỗi xảy ra khi thay đổi password của bạn, vui lòng thử lại!"
      );
    });
  };

  return (
    <form onSubmit={handleSubmit(onResetPassword)} method="POST">
      <ResetPasswordNavigation />
      <Instruction>
        <p>{t("reset_password_rules_instruction").toString()}</p>
      </Instruction>
      <PanelBody>
        <FormRow>{error ? <ErrorBox>{error}</ErrorBox> : null}</FormRow>
        <FormRow>
          <Label>{t("new_password_label")}</Label>
          <Textbox
            autoComplete="off"
            placeholder={t("please_input_new_password").toString()}
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
          <Label>{t("confirm_new_password_label")}</Label>
          <Textbox
            autoComplete="off"
            placeholder={t("please_confirm_new_password").toString()}
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
          <SubmitButton type="submit">{t("change_password")}</SubmitButton>
        </FormFooter>
      </PanelBody>
    </form>
  );
};

export default ResetPasswordForm;
