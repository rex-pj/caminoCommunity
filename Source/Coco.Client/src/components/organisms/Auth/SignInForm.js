import React, { Component } from "react";
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

export default class extends Component {
  constructor(props) {
    super(props);
    this._isMounted = false;

    this.state = {
      isFormValid: false,
      shouldRender: false
    };

    this.formData = SigninModel;
  }

  // #region Life Cycle
  componentDidMount() {
    this._isMounted = true;
  }

  componentWillUnmount() {
    this._isMounted = false;
  }
  // #endregion Life Cycle

  handleInputBlur = evt => {
    const { name } = evt.target;
    if (!this.formData[name].isValid) {
      evt.target.classList.add("invalid");
    } else {
      evt.target.classList.remove("invalid");
    }
  };

  handleInputChange = evt => {
    this.formData = this.formData || {};
    const { name, value } = evt.target;

    // Validate when input
    this.formData[name].isValid = checkValidity(this.formData, value, name);
    this.formData[name].value = value;

    if (!!this._isMounted) {
      this.setState({
        shouldRender: true
      });
    }
  };

  checkIsFormValid = () => {
    let isFormValid = false;
    for (let formIdentifier in this.formData) {
      isFormValid = this.formData[formIdentifier].isValid;
      if (!isFormValid) {
        break;
      }
    }

    return isFormValid;
  };

  onSignin = e => {
    e.preventDefault();

    let isFormValid = true;
    for (let formIdentifier in this.formData) {
      isFormValid = this.formData[formIdentifier].isValid && isFormValid;

      if (!isFormValid) {
        this.props.showValidationError(
          "Thông tin bạn nhập có thể bị sai",
          "Có thể bạn nhập sai thông tin này, vui lòng kiểm tra và nhập lại"
        );
      }
    }

    if (!!isFormValid) {
      const signinData = {};
      for (const formIdentifier in this.formData) {
        signinData[formIdentifier] = this.formData[formIdentifier].value;
      }

      this.props.onSignin(signinData);
    }
  };

  render() {
    const isFormValid = this.checkIsFormValid();

    return (
      <form onSubmit={e => this.onSignin(e)} method="POST">
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
                  onChange={e => this.handleInputChange(e)}
                  onBlur={e => this.handleInputBlur(e)}
                />
              </FormRow>
              <FormRow>
                <Label>Mật khẩu</Label>
                <Textbox
                  placeholder="Nhập mật khẩu"
                  type="password"
                  name="password"
                  onChange={e => this.handleInputChange(e)}
                  onBlur={e => this.handleInputBlur(e)}
                />
              </FormRow>
              <FormFooter>
                <SubmitButton
                  disabled={!this.props.isFormEnabled || !isFormValid}
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
}
