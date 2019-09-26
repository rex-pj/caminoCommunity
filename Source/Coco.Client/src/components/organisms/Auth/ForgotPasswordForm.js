import React, { Component } from "react";
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

export default class extends Component {
  constructor(props) {
    super(props);
    this._isMounted = false;

    this.state = {
      isFormValid: false,
      shouldRender: false,
      isSubmitted: false
    };

    this.formData = ForgotPasswordModel;
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
      this.setState({
        isSubmitted: false
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

  onUpdate = e => {
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
      const requestData = {};
      for (const formIdentifier in this.formData) {
        requestData[formIdentifier] = this.formData[formIdentifier].value;
      }

      const isSendMail = this.props.onForgotPassword(requestData);

      this.setState({
        isSubmitted: isSendMail
      });
    }
  };

  render() {
    const { isSubmitted } = this.state;
    const isFormValid = this.checkIsFormValid();

    return (
      <form onSubmit={e => this.onUpdate(e)} method="POST">
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
                  onChange={e => this.handleInputChange(e)}
                  onBlur={e => this.handleInputBlur(e)}
                />
              </FormRow>
              <FormFooter>
                <SubmitButton
                  disabled={!this.props.isFormEnabled || !isFormValid}
                  type="submit"
                >
                  Gửi Email
                </SubmitButton>
              </FormFooter>
            </PanelBody>
          </div>
        </div>
      </form>
    );
  }
}
