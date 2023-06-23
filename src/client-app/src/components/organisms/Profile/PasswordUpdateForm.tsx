import * as React from "react";
import { Fragment, useState } from "react";
import styled from "styled-components";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { PanelBody, PanelFooter } from "../../molecules/Panels";
import { ButtonPrimary } from "../../atoms/Buttons/Buttons";
import LabelAndTextbox from "../../molecules/InfoWithLabels/LabelAndTextbox";
import { QuaternaryDarkHeading } from "../../atoms/Heading";
import { ErrorBox } from "../../molecules/NotificationBars/NotificationBoxes";
import { useForm } from "react-hook-form";
import { ValidationWarningMessage } from "../../ErrorMessage";

const MainPanel = styled(PanelBody)`
  border-radius: ${(p) => p.theme.borderRadius.normal};
  box-shadow: ${(p) => p.theme.shadow.BoxShadow};
  margin-bottom: ${(p) => p.theme.size.normal};
  background-color: ${(p) => p.theme.color.whiteBg};
`;

const FormGroup = styled.div`
  margin-bottom: ${(p) => p.theme.size.exTiny};
  border-bottom: 1px solid ${(p) => p.theme.color.neutralBg};
`;

const Heading = styled(QuaternaryDarkHeading)`
  margin-bottom: ${(p) => p.theme.size.distance};
  margin-left: ${(p) => p.theme.size.exTiny};
`;

const FormFooter = styled(PanelFooter)`
  padding-left: 0;
  padding-right: 0;
`;

type Props = {
  isFormEnabled?: boolean;
  onUpdate: (e: any) => Promise<any>;
};

const PasswordUpdateForm = (props: Props) => {
  const [error, setError] = useState<string>();
  const {
    register,
    handleSubmit,
    watch,
    formState: { errors },
  } = useForm();

  const onUpdate = (data: any) => {
    const profileData: any = { ...data };
    props
      .onUpdate(profileData)
      .catch(() =>
        setError("Có lỗi xảy ra trong quá trình thay đổi mật khẩu!")
      );
  };

  return (
    <Fragment>
      <Heading>Change Password</Heading>
      <MainPanel>
        <form onSubmit={handleSubmit(onUpdate)} method="POST">
          <Fragment>
            <FormGroup>{error ? <ErrorBox>{error}</ErrorBox> : null}</FormGroup>
            <FormGroup>
              <LabelAndTextbox
                label="Current password"
                type="password"
                placeholder="Enter current password"
                {...register("currentPassword", {
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
              {errors.currentPassword && (
                <ValidationWarningMessage>
                  {errors.currentPassword.message?.toString()}
                </ValidationWarningMessage>
              )}
            </FormGroup>
            <FormGroup>
              <LabelAndTextbox
                label="New password"
                placeholder="Enter new password"
                type="password"
                {...register("newPassword", {
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
              {errors.newPassword && (
                <ValidationWarningMessage>
                  {errors.newPassword.message?.toString()}
                </ValidationWarningMessage>
              )}
            </FormGroup>
            <FormGroup>
              <LabelAndTextbox
                label="Confirm new password"
                placeholder="Confirm new password"
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
                    if (watch("newPassword") !== val) {
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
            </FormGroup>
          </Fragment>
          <FormFooter>
            <ButtonPrimary
              type="submit"
              size="xs"
              disabled={!props.isFormEnabled}
            >
              <FontAwesomeIcon icon="pencil-alt" className="me-1" />
              Change Password
            </ButtonPrimary>
          </FormFooter>
        </form>
      </MainPanel>
    </Fragment>
  );
};

export default PasswordUpdateForm;
