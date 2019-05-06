import React, { Component } from "react";
import styled from "styled-components";
import { TextboxSecondary } from "../../../components/atoms/Textboxes";
import { SelectionSecondary } from "../../../components/atoms/Selections";
import { PanelBody, PanelFooter } from "../../../components/atoms/Panels";
import { LabelNormal } from "../../../components/atoms/Labels";
import { Button } from "../../../components/atoms/Buttons";
import AuthNavigation from "../../../components/organisms/NavigationMenu/AuthNavigation";
import AuthBanner from "../../../components/organisms/Banner/AuthBanner";
import DaySelector from "../../../components/organisms/DaySelector";
import { checkValidity } from "../../../utils/Helper";

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

const Selection = styled(SelectionSecondary)`
  border-radius: ${p => p.theme.size.normal};
  border: 1px solid ${p => p.theme.color.secondary};
  background-color: ${p => p.theme.rgbaColor.dark};
  width: 100%;
  color: ${p => p.theme.color.exLight};
  padding: 0 ${p => p.theme.size.tiny};
  font-size: ${p => p.theme.fontSize.small};

  :focus {
    background-color: ${p => p.theme.color.moreDark};
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
`;

const BirthDateSelector = styled(DaySelector)`
  select {
    border-radius: ${p => p.theme.size.normal};
    border: 1px solid ${p => p.theme.color.secondary};
    background-color: ${p => p.theme.rgbaColor.dark};
    color: ${p => p.theme.color.exLight};
    font-size: ${p => p.theme.fontSize.small};
  }

  &.invalid select {
    border: 1px solid ${p => p.theme.color.dangerLight};
    color: ${p => p.theme.color.dangerLight};
  }
`;

export default class SignUpForm extends Component {
  constructor(props) {
    super(props);
    this._isMounted = false;

    this.state = {
      birthDate: null,
      isFormValid: false,
      errors: {}
    };

    this.formData = {
      lastname: {
        value: "TestLastname",
        validation: {
          isRequired: true
        },
        isValid: true
      },
      firstname: {
        value: "TestFirstname",
        validation: {
          isRequired: true
        },
        isValid: true
      },
      email: {
        value: "trungle.it@gmail.com",
        validation: {
          isEmail: true
        },
        isValid: true
      },
      password: {
        value: "TestPassword",
        validation: {
          isRequired: true
        },
        isValid: true
      },
      confirmPassword: {
        value: "TestPassword",
        validation: {
          isRequired: true,
          sameRefProperty: "password"
        },
        isValid: true
      },
      genderId: {
        value: 1,
        validation: {
          isRequired: true
        },
        isValid: true
      },
      birthDate: {
        value: new Date(),
        validation: {
          isDate: true
        },
        isValid: true
      }
    };
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
    this.formData[name].isValid = checkValidity(
      this.formData,
      value,
      this.formData[name].validation
    );

    this.formData[name].value = value;
  };

  handleBirthDateBlur = evt => {
    let errors = {
      birthDate: {
        isValid: false
      }
    };

    if (this.formData["birthDate"].isValid) {
      errors["birthDate"].isValid = true;
    }

    this.setState({
      errors
    });
  };

  handleBirthdateChange = value => {
    this.formData = this.formData || {};
    this.formData["birthDate"].value = value;

    // Validate when input birthdate
    this.formData["birthDate"].isValid = checkValidity(
      this.formData,
      value,
      this.formData["birthDate"].validation
    );

    if (this._isMounted) {
      this.setState({ birthDate: value });
    }
  };

  onSignUp = e => {
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
      const signUpData = {};
      for (const formIdentifier in this.formData) {
        signUpData[formIdentifier] = this.formData[formIdentifier].value;
      }

      this.props.signUp(signUpData);
    }
  };

  render() {
    const { errors } = this.state;
    const isBirthDateInvalid =
      errors && errors["birthDate"] && !errors["birthDate"].isValid;

    return (
      <form onSubmit={e => this.onSignUp(e)} method="POST">
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
                  onChange={e => this.handleInputChange(e)}
                  onBlur={this.handleInputBlur}
                />
              </FormRow>
              <FormRow>
                <Label>Tên</Label>
                <Textbox
                  autoComplete="off"
                  placeholder="Nhập tên của bạn vào đây"
                  name="firstname"
                  onChange={e => this.handleInputChange(e)}
                  onBlur={this.handleInputBlur}
                />
              </FormRow>
              <FormRow>
                <Label>E-mail</Label>
                <Textbox
                  autoComplete="off"
                  placeholder="Nhập e-mail"
                  type="email"
                  name="email"
                  onChange={e => this.handleInputChange(e)}
                  onBlur={this.handleInputBlur}
                />
              </FormRow>
              <FormRow>
                <Label>Mật khẩu</Label>
                <Textbox
                  autoComplete="off"
                  placeholder="Nhập mật khẩu"
                  type="password"
                  name="password"
                  onChange={e => this.handleInputChange(e)}
                  onBlur={this.handleInputBlur}
                />
              </FormRow>
              <FormRow>
                <Label>Nhập lại mật khẩu</Label>
                <Textbox
                  autoComplete="off"
                  placeholder="Nhập lại mật khẩu"
                  type="password"
                  name="confirmPassword"
                  onChange={e => this.handleInputChange(e)}
                  onBlur={this.handleInputBlur}
                />
              </FormRow>
              <FormRow>
                <Label>Sinh nhật</Label>
                <BirthDateSelector
                  className={isBirthDateInvalid ? "invalid" : ""}
                  name="birthDate"
                  value={this.state.birthDate}
                  onDateChanged={date => this.handleBirthdateChange(date)}
                  onBlur={this.handleBirthDateBlur}
                />
              </FormRow>
              <FormRow>
                <Label>Giới tính</Label>
                <Selection
                  placeholder="Chọn giới tính Nam/Nữ"
                  name="genderId"
                  onChange={e => this.handleInputChange(e)}
                  defaultValue={1}
                >
                  <option value={1}>Nam</option>
                  <option value={2}>Nữ</option>
                </Selection>
              </FormRow>
              <FormFooter>
                <SubmitButton type="submit">Ghi danh</SubmitButton>
              </FormFooter>
            </PanelBody>
          </div>
        </div>
      </form>
    );
  }
}
