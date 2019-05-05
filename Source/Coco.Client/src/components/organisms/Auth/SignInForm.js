import React, { Component } from "react";
import styled from "styled-components";
import { TextboxSecondary } from "../../../components/atoms/Textboxes";
import { PanelBody, PanelFooter } from "../../../components/atoms/Panels";
import { LabelNormal } from "../../../components/atoms/Labels";
import { Button } from "../../../components/atoms/Buttons";
import AuthNavigation from "../../../components/organisms/NavigationMenu/AuthNavigation";
import AuthBanner from "../../../components/organisms/Banner/AuthBanner";

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

export default class extends Component {
  render() {
    return (
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
              <Textbox placeholder="Nhập e-mail" type="email" />
            </FormRow>
            <FormRow>
              <Label>Mật khẩu</Label>
              <Textbox placeholder="Nhập mật khẩu" type="password" />
            </FormRow>
            <FormFooter>
              <SubmitButton type="submit">Đăng Nhập</SubmitButton>
            </FormFooter>
          </PanelBody>
        </div>
      </div>
    );
  }
}
