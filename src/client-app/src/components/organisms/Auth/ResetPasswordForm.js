import React, { useState, useCallback } from "react";
import styled from "styled-components";
import { SecondaryTextbox } from "../../../components/atoms/Textboxes";
import { PanelBody, PanelFooter } from "../../../components/molecules/Panels";
import { LabelNormal } from "../../../components/atoms/Labels";
import { ButtonPrimary } from "../../../components/atoms/Buttons/Buttons";
import ResetPasswordNavigation from "../../../components/organisms/Navigation/ResetPasswordNavigation";
import { SecondaryHeading } from "../../atoms/Heading";
import { checkValidity } from "../../../utils/Validity";
import resetPasswordModel from "../../../models/resetPasswordModel";

const Textbox = styled(SecondaryTextbox)`
  border-radius: ${(p) => p.theme.size.normal};
  border: 1px solid ${(p) => p.theme.color.secondaryBg};
  background-color: ${(p) => p.theme.rgbaColor.darkLight};
  width: 100%;
  color: ${(p) => p.theme.color.darkText};
  padding: ${(p) => p.theme.size.tiny};

  ::placeholder {
    color: ${(p) => p.theme.color.neutralText};
    font-size: ${(p) => p.theme.fontSize.small};
  }

  :focus {
    background-color: ${(p) => p.theme.color.moreDark};
  }

  &.invalid {
    border: 1px solid ${(p) => p.theme.color.secondaryDangerBg};
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

const SubmitButton = styled(ButtonPrimary)`
  font-size: ${(p) => p.theme.fontSize.small};
  cursor: pointer;
  border: 1px solid ${(p) => p.theme.color.secondaryBg};

  :hover {
    color: ${(p) => p.theme.color.neutralText};
  }

  :disabled {
    background-color: ${(p) => p.theme.color.secondaryBg};
    color: ${(p) => p.theme.color.neutralText};
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

export default (props) => {
  let formData = resetPasswordModel;
  const { args } = props;
  const [, updateState] = useState();
  const forceUpdate = useCallback(() => updateState({}), []);
  if (args) {
    formData.email.value = args.email;
    formData.email.isValid = checkValidity(formData, args.email, "email");

    formData.key.value = args.key;
    formData.key.isValid = checkValidity(formData, args.key, "key");
  }

  const handleInputBlur = (evt) => {
    alertInvalidForm(evt.target);
  };

  const handleInputChange = (evt) => {
    formData = formData || {};
    const { name, value } = evt.target;

    // Validate when input
    formData[name].isValid = checkValidity(formData, value, name);
    formData[name].value = value;

    alertInvalidForm(evt.target);
    forceUpdate();
  };

  const alertInvalidForm = (target) => {
    const { name } = target;
    if (!formData[name].isValid) {
      target.classList.add("invalid");
    } else {
      target.classList.remove("invalid");
    }
  };

  const onResetPassword = (e) => {
    e.preventDefault();

    let isFormValid = true;
    for (let formIdentifier in formData) {
      isFormValid = formData[formIdentifier].isValid && isFormValid;

      if (!isFormValid) {
        props.showValidationError(
          "Something went wrong with your input",
          "Something went wrong with your information, please check and input again"
        );
      }
    }

    if (!!isFormValid) {
      const resetPasswordData = {};
      for (const formIdentifier in formData) {
        resetPasswordData[formIdentifier] = formData[formIdentifier].value;
      }

      props.resetPassword(resetPasswordData);
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
        <FormRow>
          <Label>Current password</Label>
          <Textbox
            autoComplete="off"
            placeholder="Your current password"
            name="currentPassword"
            type="password"
            onChange={(e) => handleInputChange(e)}
            onBlur={(e) => handleInputBlur(e)}
          />
        </FormRow>
        <FormRow>
          <Label>New password</Label>
          <Textbox
            autoComplete="off"
            placeholder="Enter new password"
            type="password"
            name="password"
            onChange={(e) => handleInputChange(e)}
            onBlur={(e) => handleInputBlur(e)}
          />
        </FormRow>
        <FormRow>
          <Label>Confirm password</Label>
          <Textbox
            autoComplete="off"
            placeholder="Please confirm your new password"
            type="password"
            name="confirmPassword"
            onChange={(e) => handleInputChange(e)}
            onBlur={(e) => handleInputBlur(e)}
          />
        </FormRow>
        <FormFooter>
          <SubmitButton type="submit" disabled={!isFormCheckValid}>
            Change password
          </SubmitButton>
        </FormFooter>
      </PanelBody>
    </form>
  );
};
