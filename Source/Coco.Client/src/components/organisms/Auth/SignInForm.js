import React, { useEffect, useState } from "react";
import styled from "styled-components";
import { TextboxSecondary } from "../../../components/atoms/Textboxes";
import { PanelBody, PanelFooter } from "../../../components/atoms/Panels";
import { LabelNormal } from "../../../components/atoms/Labels";
import { Button } from "../../../components/atoms/Buttons";
import AuthNavigation from "../../../components/organisms/NavigationMenu/AuthNavigation";
import AuthBanner from "../../../components/organisms/Banner/AuthBanner";
import SigninModel from "../../../models/SigninModel";
import { checkValidity } from "../../../utils/Validity";

const Textbox = styled(TextboxSecondary)`
  border-radius: ${p => p.theme.size.normal};
  border: 1px solid ${p => p.theme.color.secondary};
  background-color: ${p => p.theme.rgbaColor.dark};
  width: 100%;
  color: ${p => p.theme.color.exLight};
  padding: ${p => p.theme.size.tiny};

  ::placeholder {
    color: ${p => p.theme.color.light};
    font-size: ${p => p.theme.fontSize.small};
  }

  :focus {
    background-color: ${p => p.theme.color.moreDark};
  }

  &.invalid {
    border: 1px solid ${p => p.theme.color.dangerLight};
  }
`;

const FormFooter = styled(PanelFooter)`
  text-align: center;
`;

const Label = styled(LabelNormal)`
  margin-left: ${p => p.theme.size.tiny};
  margin-bottom: 0;
  font-size: ${p => p.theme.fontSize.small};
  font-weight: 600;
`;

const FormRow = styled.div`
  margin-bottom: ${p => p.theme.size.tiny};
`;

const SubmitButton = styled(Button)`
  font-size: ${p => p.theme.fontSize.small};
  border: 1px solid ${p => p.theme.color.secondary};

  :hover {
    color: ${p => p.theme.color.light};
  }

  :disabled {
    background-color: ${p => p.theme.color.secondary};
    color: ${p => p.theme.color.normal};
    cursor: auto;
  }
`;

export default function(props) {
  let formData = SigninModel;
  const [isValidForm, setIsFormValid] = useState(false);

  useEffect(function() {
    const isFormValid = checkIsFormValid();
    setIsFormValid(isFormValid);
  });

  function handleInputBlur(evt) {
    const { name } = evt.target;
    if (!formData[name].isValid) {
      evt.target.classList.add("invalid");
    } else {
      evt.target.classList.remove("invalid");
    }
  }

  function handleInputChange(evt) {
    formData = formData || {};
    const { name, value } = evt.target;

    // Validate when input
    formData[name].isValid = checkValidity(formData, value, name);
    formData[name].value = value;

    setIsFormValid(formData[name].isValid);
  }

  function checkIsFormValid() {
    let isFormValid = false;
    for (let formIdentifier in formData) {
      isFormValid = formData[formIdentifier].isValid;
      if (!isFormValid) {
        break;
      }
    }

    return isFormValid;
  }

  function onSignin(e) {
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
      const signinData = {};
      for (const formIdentifier in formData) {
        signinData[formIdentifier] = formData[formIdentifier].value;
      }

      props.onSignin(signinData);
    }
  }

  return (
    <form onSubmit={e => onSignin(e)} method="POST">
      <div className="row no-gutters">
        <div className="col col-12 col-sm-7">
          <AuthBanner
            imageUrl={`${process.env.PUBLIC_URL}/images/logo.png`}
            title="Đăng Nhập"
            instruction="Tham gia để cùng kết nối với nhiều nhà nông khác"
          />
        </div>
        <div className="col col-12 col-sm-5">
          <AuthNavigation />
          <PanelBody>
            <FormRow>
              <Label>E-mail</Label>
              <Textbox
                placeholder="Nhập e-mail"
                type="email"
                name="username"
                onChange={e => handleInputChange(e)}
                onBlur={e => handleInputBlur(e)}
              />
            </FormRow>
            <FormRow>
              <Label>Mật khẩu</Label>
              <Textbox
                placeholder="Nhập mật khẩu"
                type="password"
                name="password"
                onChange={e => handleInputChange(e)}
                onBlur={e => handleInputBlur(e)}
              />
            </FormRow>
            <FormFooter>
              <SubmitButton
                disabled={!props.isFormEnabled || !isValidForm}
                type="submit"
              >
                Đăng Nhập
              </SubmitButton>
            </FormFooter>
          </PanelBody>
        </div>
      </div>
    </form>
  );
}
