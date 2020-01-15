import React, { Fragment, useState } from "react";
import styled from "styled-components";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { PanelBody } from "../../atoms/Panels";
import { ButtonPrimary } from "../../../components/atoms/Buttons/Buttons";
import LabelAndTextbox from "../../molecules/InfoWithLabels/LabelAndTextbox";
import { checkValidity } from "../../../utils/Validity";
import { PanelFooter } from "../../../components/atoms/Panels";
import { QuaternaryHeading } from "../../atoms/Heading";

const MainPanel = styled(PanelBody)`
  border-radius: ${p => p.theme.borderRadius.normal};
  box-shadow: ${p => p.theme.shadow.BoxShadow};
  margin-bottom: ${p => p.theme.size.normal};
  background-color: ${p => p.theme.color.white};
`;

const FormGroup = styled.div`
  margin-bottom: ${p => p.theme.size.exTiny};
  border-bottom: 1px solid ${p => p.theme.color.lighter};
`;

const Heading = styled(QuaternaryHeading)`
  margin-bottom: ${p => p.theme.size.distance};
  margin-left: ${p => p.theme.size.exTiny};
`;

const SubmitButton = styled(ButtonPrimary)`
  font-size: ${p => p.theme.fontSize.small};
  cursor: pointer;

  :hover {
    color: ${p => p.theme.color.light};
  }

  :disabled {
    background-color: ${p => p.theme.color.primaryLight};
    color: ${p => p.theme.color.neutral};
    cursor: auto;
  }

  svg {
    margin-right: ${p => p.theme.size.exTiny};
  }
`;

const FormFooter = styled(PanelFooter)`
  padding-left: 0;
  padding-right: 0;
`;

export default props => {
  const { userInfo } = props;

  let model = {
    displayName: {
      value: userInfo.displayName,
      validation: {
        isRequired: true
      },
      isValid: !!userInfo.displayName
    },
    lastname: {
      value: userInfo.firstname,
      validation: {
        isRequired: true
      },
      isValid: !!userInfo.firstname
    },
    firstname: {
      value: userInfo.lastname,
      validation: {
        isRequired: true
      },
      isValid: !!userInfo.lastname
    }
  };

  const [formData, setFromData] = useState(model);

  const onTextboxChange = e => {
    let userData = formData || {};
    const { name, value } = e.target;

    // Validate when input
    userData[name].isValid = checkValidity(userData, value, name);
    userData[name].value = value;

    setFromData({
      ...userData
    });
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

  const onUpdate = e => {
    e.preventDefault();

    let isFormValid = true;
    for (let formIdentifier in formData) {
      isFormValid = formData[formIdentifier].isValid && isFormValid;

      if (!isFormValid) {
        props.showValidationError(
          "Thông tin bạn nhập có thể bị sai",
          "Có thể bạn nhập sai thông tin này, vui lòng kiểm tra và nhập lại",
          "error"
        );
      }
    }

    if (!!isFormValid) {
      const profileData = {};
      for (const formIdentifier in formData) {
        profileData[formIdentifier] = formData[formIdentifier].value;
      }

      props.onUpdate(profileData);
    }
  };

  const { displayName, firstname, lastname } = formData;
  const isFormValid = checkIsFormValid();

  return (
    <Fragment>
      <Heading>Cập nhật thông tin cá nhân</Heading>
      <MainPanel>
        <form onSubmit={e => onUpdate(e)} method="POST">
          {userInfo ? (
            <Fragment>
              <FormGroup>
                <LabelAndTextbox
                  label="Họ"
                  name="lastname"
                  value={lastname.value}
                  onChange={onTextboxChange}
                />
              </FormGroup>
              <FormGroup>
                <LabelAndTextbox
                  label="Tên"
                  name="firstname"
                  value={firstname.value}
                  onChange={onTextboxChange}
                />
              </FormGroup>
              <FormGroup>
                <LabelAndTextbox
                  label="Tên hiển thị"
                  name="displayName"
                  value={displayName.value}
                  onChange={onTextboxChange}
                />
              </FormGroup>
            </Fragment>
          ) : null}
          <FormFooter>
            <SubmitButton
              type="submit"
              size="sm"
              disabled={!props.isFormEnabled || !isFormValid}
            >
              <FontAwesomeIcon icon="pencil-alt" />
              Cập Nhật
            </SubmitButton>
          </FormFooter>
        </form>
      </MainPanel>
    </Fragment>
  );
};
