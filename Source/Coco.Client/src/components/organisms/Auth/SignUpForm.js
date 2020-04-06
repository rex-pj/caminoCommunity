import React, { useState, useCallback } from "react";
import styled from "styled-components";
import { TextboxSecondary } from "../../../components/atoms/Textboxes";
import { SelectionSecondary } from "../../../components/atoms/Selections";
import { PanelBody, PanelFooter } from "../../../components/atoms/Panels";
import { LabelNormal } from "../../../components/atoms/Labels";
import { ButtonPrimary } from "../../../components/atoms/Buttons/Buttons";
import AuthNavigation from "../../../components/organisms/NavigationMenu/AuthNavigation";
import AuthBanner from "../../../components/organisms/Banner/AuthBanner";
import DateSelector from "../../../components/molecules/DateSelector";
import { checkValidity } from "../../../utils/Validity";
import SignupModel from "../../../models/SignupModel";

const Textbox = styled(TextboxSecondary)`
  border-radius: ${(p) => p.theme.size.normal};
  border: 1px solid ${(p) => p.theme.color.primaryLight};
  background-color: ${(p) => p.theme.color.lighter};
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

const Selection = styled(SelectionSecondary)`
  border-radius: ${(p) => p.theme.size.normal};
  border: 1px solid ${(p) => p.theme.color.primaryLight};
  background-color: ${(p) => p.theme.color.lighter};
  width: 100%;
  color: ${(p) => p.theme.color.dark};
  padding: 0 ${(p) => p.theme.size.tiny};
  font-size: ${(p) => p.theme.fontSize.small};

  :focus {
    background-color: ${(p) => p.theme.color.moreDark};
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

const BirthDateSelector = styled(DateSelector)`
  select {
    border-radius: ${(p) => p.theme.size.normal};
    border: 1px solid ${(p) => p.theme.color.primaryLight};
    background-color: ${(p) => p.theme.color.lighter};
    color: ${(p) => p.theme.color.dark};
    font-size: ${(p) => p.theme.fontSize.small};
  }

  &.invalid select {
    border: 1px solid ${(p) => p.theme.color.dangerLight};
    color: ${(p) => p.theme.color.dangerLight};
  }
`;

export default (props) => {
  let formData = SignupModel;
  const [, updateState] = useState();
  const forceUpdate = useCallback(() => updateState({}), []);

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

  const onSignUp = (e) => {
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
      const signUpData = {};
      for (const formIdentifier in formData) {
        signUpData[formIdentifier] = formData[formIdentifier].value;
      }

      props.signUp(signUpData);
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
    <form onSubmit={(e) => onSignUp(e)} method="POST">
      <div className="row no-gutters">
        <div className="col col-12 col-sm-7">
          <AuthBanner
            icon="signature"
            title="Ghi Danh"
            instruction="Ghi danh tại đây, cùng tham gia với nhiều nhà nông khác"
          />
        </div>
        <div className="col col-12 col-sm-5">
          <AuthNavigation />
          <PanelBody>
            <FormRow>
              <Label>Họ</Label>
              <Textbox
                autoComplete="off"
                placeholder="Nhập họ của bạn vào đây"
                name="lastname"
                onChange={(e) => handleInputChange(e)}
                onBlur={(e) => handleInputBlur(e)}
              />
            </FormRow>
            <FormRow>
              <Label>Tên</Label>
              <Textbox
                autoComplete="off"
                placeholder="Nhập tên của bạn vào đây"
                name="firstname"
                onChange={(e) => handleInputChange(e)}
                onBlur={(e) => handleInputBlur(e)}
              />
            </FormRow>
            <FormRow>
              <Label>E-mail</Label>
              <Textbox
                autoComplete="off"
                placeholder="Nhập e-mail"
                type="email"
                name="email"
                onChange={(e) => handleInputChange(e)}
                onBlur={(e) => handleInputBlur(e)}
              />
            </FormRow>
            <FormRow>
              <Label>Mật khẩu</Label>
              <Textbox
                autoComplete="new-password"
                placeholder="Nhập mật khẩu"
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
                placeholder="Nhập lại mật khẩu"
                type="password"
                name="confirmPassword"
                onChange={(e) => handleInputChange(e)}
                onBlur={(e) => handleInputBlur(e)}
              />
            </FormRow>
            <FormRow>
              <Label>Sinh nhật</Label>
              <BirthDateSelector
                name="birthDate"
                onDateChanged={(e) => handleInputChange(e)}
                onBlur={handleInputBlur}
              />
            </FormRow>
            <FormRow>
              <Label>Giới tính</Label>
              <Selection
                placeholder="Chọn giới tính Nam/Nữ"
                name="genderId"
                onChange={(e) => handleInputChange(e)}
                defaultValue={1}
              >
                <option value={1}>Nam</option>
                <option value={2}>Nữ</option>
              </Selection>
            </FormRow>
            <FormFooter>
              <SubmitButton
                type="submit"
                disabled={!props.isFormEnabled || !isFormCheckValid}
              >
                Ghi danh
              </SubmitButton>
            </FormFooter>
          </PanelBody>
        </div>
      </div>
    </form>
  );
};
