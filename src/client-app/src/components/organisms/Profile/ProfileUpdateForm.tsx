import * as React from "react";
import { Fragment, useState } from "react";
import styled from "styled-components";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { PanelBody } from "../../molecules/Panels";
import { ButtonSecondary } from "../../../components/atoms/Buttons/Buttons";
import LabelAndTextbox from "../../molecules/InfoWithLabels/LabelAndTextbox";
import { PanelFooter } from "../../../components/molecules/Panels";
import { QuaternaryDarkHeading } from "../../atoms/Heading";
import { useForm } from "react-hook-form";
import { ValidationWarningMessage } from "../../ErrorMessage";
import {
  ErrorBox,
  SuccessBox,
} from "../../molecules/NotificationBars/NotificationBoxes";

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

interface Props {
  userInfo?: any;
  onUpdate: (e: any) => Promise<any>;
  isFormEnabled?: boolean;
}

const ProfileUpdateForm = (props: Props) => {
  const { userInfo } = props;
  const [error, setError] = useState<string>();
  const [successMessage, setSuccessMessage] = useState<string>();
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm({
    defaultValues: {
      lastname: userInfo?.lastname,
      firstname: userInfo?.firstname,
      displayName: userInfo?.displayName,
    },
  });

  const onUpdate = (data: any) => {
    const profileData: any = { ...data };
    return props
      .onUpdate(profileData)
      .then(() => {
        setSuccessMessage("Cập nhật thông tin cá nhân thành công");
      })
      .catch(() => {
        setError("Có lỗi xảy ra trong quá trình cập nhật thông tin cá nhân");
      });
  };

  return (
    <Fragment>
      <Heading>Update personal information</Heading>
      <MainPanel>
        <FormGroup>{error ? <ErrorBox>{error}</ErrorBox> : null}</FormGroup>
        <FormGroup>
          {successMessage ? <SuccessBox>{successMessage}</SuccessBox> : null}
        </FormGroup>
        <form onSubmit={handleSubmit(onUpdate)} method="POST">
          {userInfo ? (
            <Fragment>
              <FormGroup>
                <LabelAndTextbox
                  label="Lastname"
                  defaultValue={userInfo?.lastname}
                  {...register("lastname", {
                    required: {
                      value: true,
                      message: "This field is required",
                    },
                  })}
                />
                {errors.lastname && (
                  <ValidationWarningMessage>
                    {errors.lastname.message?.toString()}
                  </ValidationWarningMessage>
                )}
              </FormGroup>
              <FormGroup>
                <LabelAndTextbox
                  label="Firstname"
                  defaultValue={userInfo?.firstname}
                  {...register("firstname", {
                    required: {
                      value: true,
                      message: "This field is required",
                    },
                  })}
                />
                {errors.firstname && (
                  <ValidationWarningMessage>
                    {errors.firstname.message?.toString()}
                  </ValidationWarningMessage>
                )}
              </FormGroup>
              <FormGroup>
                <LabelAndTextbox
                  label="Display Name"
                  defaultValue={userInfo?.displayName}
                  {...register("displayName", {
                    required: {
                      value: true,
                      message: "This field is required",
                    },
                  })}
                />
                {errors.displayName && (
                  <ValidationWarningMessage>
                    {errors.displayName.message?.toString()}
                  </ValidationWarningMessage>
                )}
              </FormGroup>
            </Fragment>
          ) : null}
          <FormFooter>
            <ButtonSecondary
              type="submit"
              size="xs"
              disabled={!props.isFormEnabled}
            >
              <FontAwesomeIcon icon="pencil-alt" className="me-1" />
              Update
            </ButtonSecondary>
          </FormFooter>
        </form>
      </MainPanel>
    </Fragment>
  );
};

export default ProfileUpdateForm;
