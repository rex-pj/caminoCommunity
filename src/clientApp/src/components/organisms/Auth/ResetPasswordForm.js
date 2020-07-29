import React, { useState, useCallback } from "react";
import styled from "styled-components";
import { TextboxSecondary } from "../../../components/atoms/Textboxes";
import { PanelBody, PanelFooter } from "../../../components/atoms/Panels";
import { LabelNormal } from "../../../components/atoms/Labels";
import { ButtonPrimary } from "../../../components/atoms/Buttons/Buttons";
import ResetPasswordNavigation from "../../../components/organisms/NavigationMenu/ResetPasswordNavigation";
import { SecondaryHeading } from "../../atoms/Heading";
import { checkValidity } from "../../../utils/Validity";
import resetPasswordModel from "../../../models/resetPasswordModel";

const Textbox = styled(TextboxSecondary)`
  border-radius: ${(p) => p.theme.size.normal};
  border: 1px solid ${(p) => p.theme.color.primaryLight};
  background-color: ${(p) => p.theme.rgbaColor.darkLight};
  width: 100%;
  color: ${(p) => p.theme.color.dark};
  padding: ${(p) => p.theme.size.tiny};

  ::placeholder {
    color: ${(p) => p.theme.color.light};
    font-size: ${(p) => p.theme.fontSize.small};
  }

  :focus {
    background-color: ${(p) => p.theme.color.moreDark};
  }

  &.invalid {
    border: 1px solid ${(p) => p.theme.color.dangerLight};
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
  border: 1px solid ${(p) => p.theme.color.primaryLight};

  :hover {
    color: ${(p) => p.theme.color.light};
  }

  :disabled {
    background-color: ${(p) => p.theme.color.primaryLight};
    color: ${(p) => p.theme.color.neutral};
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
    color: ${(p) => p.theme.color.lighter};
    text-transform: uppercase;
  }

  p {
    color: ${(p) => p.theme.color.light};
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
          "Thông tin bạn nhập có thể bị sai",
          "Có thể bạn nhập sai thông tin này, vui lòng kiểm tra và nhập lại"
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
        <p>Mật khẩu nên bao gồm chữ, số, ký tự đặc biệt và chữ viết hoa</p>
      </Instruction>
      <PanelBody>
        <FormRow>
          <Label>Mật khẩu hiện tại</Label>
          <Textbox
            autoComplete="off"
            placeholder="Nhập mật khẩu hiện tại của bạn vào đây"
            name="currentPassword"
            type="password"
            onChange={(e) => handleInputChange(e)}
            onBlur={(e) => handleInputBlur(e)}
          />
        </FormRow>
        <FormRow>
          <Label>Mật khẩu mới</Label>
          <Textbox
            autoComplete="off"
            placeholder="Nhập mật khẩu mới"
            type="password"
            name="password"
            onChange={(e) => handleInputChange(e)}
            onBlur={(e) => handleInputBlur(e)}
          />
        </FormRow>
        <FormRow>
          <Label>Nhập lại mật khẩu</Label>
          <Textbox
            autoComplete="off"
            placeholder="Nhập lại mật khẩu mới"
            type="password"
            name="confirmPassword"
            onChange={(e) => handleInputChange(e)}
            onBlur={(e) => handleInputBlur(e)}
          />
        </FormRow>
        <FormFooter>
          <SubmitButton type="submit" disabled={!isFormCheckValid}>
            Đổi mật khẩu
          </SubmitButton>
        </FormFooter>
      </PanelBody>
    </form>
  );
};
