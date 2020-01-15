import React, { useState, useCallback } from "react";
import styled from "styled-components";
import { TextboxSecondary } from "../../../components/atoms/Textboxes";
import { PanelBody, PanelFooter } from "../../../components/atoms/Panels";
import { LabelNormal } from "../../../components/atoms/Labels";
import { ButtonPrimary } from "../../../components/atoms/Buttons/Buttons";
import ForgotAuthNavigation from "../../../components/organisms/NavigationMenu/ForgotAuthNavigation";
import AuthBanner from "../../../components/organisms/Banner/AuthBanner";
import ForgotPasswordModel from "../../../models/ForgotPasswordModel";
import { checkValidity } from "../../../utils/Validity";
import { PrimaryNotice } from "../../atoms/Notices/AlertNotice";
import { withRouter } from "react-router-dom";

const Textbox = styled(TextboxSecondary)`
  border-radius: ${p => p.theme.size.normal};
  border: 1px solid ${p => p.theme.color.primaryLight};
  background-color: ${p => p.theme.rgbaColor.darkLight};
  width: 100%;
  color: ${p => p.theme.color.dark};
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

const SubmitButton = styled(ButtonPrimary)`
  font-size: ${p => p.theme.fontSize.small};
  border: 1px solid ${p => p.theme.color.primaryLight};

  :hover {
    color: ${p => p.theme.color.light};
  }

  :disabled {
    background-color: ${p => p.theme.color.primaryLight};
    color: ${p => p.theme.color.neutral};
    cursor: auto;
  }
`;

export default withRouter(props => {
  const [, updateState] = useState();
  const forceUpdate = useCallback(() => updateState({}), []);
  const [isFormEnabled, setFormEnabled] = useState(false);
  const [isSubmitted, setSubmitted] = useState(false);
  let formData = ForgotPasswordModel;

  const handleInputBlur = evt => {
    const { name } = evt.target;
    if (!formData[name].isValid) {
      evt.target.classList.add("invalid");
    } else {
      evt.target.classList.remove("invalid");
    }
  };

  const handleInputChange = evt => {
    if (isSubmitted) {
      setSubmitted(false);
    }

    formData = formData || {};
    const { name, value } = evt.target;

    const prevValid = formData[name].isValid;
    // Validate when input
    formData[name].isValid = checkValidity(formData, value, name);
    formData[name].value = value;

    if (prevValid !== formData[name].isValid) {
      forceUpdate();
    }

    if (formData[name].isValid && !isFormEnabled) {
      setFormEnabled(true);
    }
  };

  const onUpdate = async e => {
    e.preventDefault();

    if (isFormEnabled) {
      setFormEnabled(false);
    }

    if (!isSubmitted) {
      setSubmitted(true);
    }

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
      const requestData = {};
      for (const formIdentifier in formData) {
        requestData[formIdentifier] = formData[formIdentifier].value;
      }

      await props
        .onForgotPassword(requestData)
        .then(() => {
          setFormEnabled(true);
        })
        .catch(() => {
          setFormEnabled(true);
        });
    }
  };

  return (
    <form onSubmit={e => onUpdate(e)} method="POST">
      <div className="row no-gutters">
        <div className="col col-12 col-sm-7">
          <AuthBanner icon="unlock-alt" title="Phục hồi mật khẩu" />
        </div>
        <div className="col col-12 col-sm-5">
          <ForgotAuthNavigation />
          <PanelBody>
            <FormRow>
              {isSubmitted ? (
                <PrimaryNotice>
                  Chúng tôi sẽ gửi một e-mail kích hoạt cho bạn, hãy vào email
                  kiểm tra, nếu không tìm thấy hãy vào thư mục spam để xem thử
                </PrimaryNotice>
              ) : null}
            </FormRow>
            <FormRow>
              <Label>E-mail</Label>
              <Textbox
                placeholder="Nhập e-mail"
                type="email"
                name="email"
                autoComplete="off"
                onChange={e => handleInputChange(e)}
                onBlur={e => handleInputBlur(e)}
              />
            </FormRow>
            <FormFooter>
              <SubmitButton disabled={!isFormEnabled} type="submit">
                Gửi Email
              </SubmitButton>
            </FormFooter>
          </PanelBody>
        </div>
      </div>
    </form>
  );
});
