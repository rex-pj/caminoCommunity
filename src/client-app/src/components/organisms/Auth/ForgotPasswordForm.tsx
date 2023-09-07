import * as React from "react";
import { useState } from "react";
import styled from "styled-components";
import { SecondaryTextbox } from "../../atoms/Textboxes";
import { PanelBody, PanelFooter } from "../../molecules/Panels";
import { LabelNormal } from "../../atoms/Labels";
import { ButtonSecondary } from "../../atoms/Buttons/Buttons";
import ForgotPasswordNavigation from "./ForgotPasswordNavigation";
import AuthBanner from "./AuthBanner";
import { validateEmail } from "../../../utils/Validity";
import { ErrorBox, SuccessBox } from "../../molecules/NotificationBars/NotificationBoxes";
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
  border: 1px solid ${(p) => p.theme.color.primaryBg};

  :hover {
    color: ${(p) => p.theme.color.neutralText};
  }

  :disabled {
    color: ${(p) => p.theme.color.primaryText};
    cursor: auto;
  }
`;

interface Props {
  onForgotPassword: (e: any) => Promise<any>;
}

const ForgotPasswordForm = (props: Props) => {
  const { t } = useTranslation();
  const [isSubmitted, setSubmitted] = useState(false);
  const [error, setError] = useState<string>();
  const [successMessage, setSuccessMessage] = useState<string>();
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm();

  const onUpdate = async (data: any) => {
    setError("");
    setSuccessMessage("");

    if (!isSubmitted) {
      setSubmitted(true);
    }

    const requestData: any = { ...data };
    await props
      .onForgotPassword(requestData)
      .then(() => {
        showError("");
        setSuccessMessage("Thông báo quên mật khẩu thành công, vui lòng kiểm tra email và làm theo hướng dẫn!");
      })
      .catch(() => {
        setSuccessMessage("");
        showError("Có lỗi xảy ra trong quá trình thông báo quên mật khẩu");
      });
  };

  const showError = (message: string) => {
    setError(message);
  };

  return (
    <form onSubmit={handleSubmit(onUpdate)} method="POST">
      <div className="row g-0">
        <div className="col col-12 col-sm-7">
          <AuthBanner icon="unlock-alt" title={t("recover_your_password")} />
        </div>
        <div className="col col-12 col-sm-5">
          <ForgotPasswordNavigation />
          <PanelBody>
            <FormRow>
              {error ? <ErrorBox>{error}</ErrorBox> : null}
              {successMessage ? <SuccessBox>{successMessage}</SuccessBox> : null}
            </FormRow>
            <FormRow>
              <Label>Email</Label>
              <Textbox
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
                placeholder={t("please_input_your_email").toString()}
                type="email"
                autoComplete="off"
              />
              {errors.email && <ValidationWarningMessage>{errors.email.message?.toString()}</ValidationWarningMessage>}
            </FormRow>
            <FormFooter>
              <SubmitButton type="submit">{t("change_password")}</SubmitButton>
            </FormFooter>
          </PanelBody>
        </div>
      </div>
    </form>
  );
};

export default ForgotPasswordForm;
