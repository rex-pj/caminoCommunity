import React from "react";
import styled from "styled-components";
import { PanelBody } from "../../../components/atoms/Panels";
import { VerticalList } from "../../atoms/List";
import { format } from "date-fns";
import TextEditable from "../../molecules/Editable/TextEditable";

const MainPanel = styled(PanelBody)`
  border-radius: ${p => p.theme.borderRadius.normal};
  box-shadow: ${p => p.theme.shadow.BoxShadow};
  margin-bottom: ${p => p.theme.size.normal};
`;

const Root = styled.div`
  position: relative;
  border-radius: ${p => p.theme.borderRadius.normal};
`;

const ChildItem = styled.li`
  font-size: ${p => p.theme.fontSize.small};
  color: ${p => p.theme.color.dark};
  margin-bottom: ${p => p.theme.size.tiny};
  padding-bottom: ${p => p.theme.size.tiny};
  min-height: ${p => p.theme.size.normal};
  border-bottom: 1px solid ${p => p.theme.color.exLight};

  label {
    color: ${p => p.theme.color.normal};
  }

  div {
    font-weight: 600;
    font-size: ${p => p.theme.fontSize.small};
  }

  p {
    font-size: ${p => p.theme.fontSize.small};
    font-weight: 600;
    display: block;
    margin-bottom: 0;
    color: ${p => p.theme.color.link};
    text-decoration: underline;
  }

  :last-child {
    border-bottom: 0;
    margin-bottom: 0;
    padding-bottom: 0;
  }
`;

const InfoList = styled(VerticalList)`
  margin-bottom: 0;
`;

const UnserInfoWWithLabel = props => {
  const { className, children, label, isEmail } = props;
  return (
    <ChildItem className={className}>
      {label ? <label>{label}</label> : null}
      {!!isEmail ? (
        <p>{children}</p>
      ) : children ? (
        <div>{children}</div>
      ) : (
        <div>...</div>
      )}
    </ChildItem>
  );
};

export default function(props) {
  const { userInfo } = props;

  function onEditable(e) {
    console.log(e);
  }

  return (
    <MainPanel>
      <Root>
        {userInfo ? (
          <InfoList>
            <UnserInfoWWithLabel label="Họ &amp; Tên">
              <div className="row">
                <div className="col-auto">{userInfo.lastname}</div>
                <div className="col-auto">{userInfo.firstname}</div>
              </div>
            </UnserInfoWWithLabel>
            <UnserInfoWWithLabel label="Về bản thân">
              <TextEditable
                value={userInfo.description}
                primaryKey={userInfo.userHashedId}
                propertyName="Description"
                onUpdated={onEditable}
              />
            </UnserInfoWWithLabel>
            <UnserInfoWWithLabel label="Giới tính">
              <TextEditable
                value={userInfo.genderLabel}
                primaryKey={userInfo.userHashedId}
                propertyName="GenderLabel"
                onUpdated={onEditable}
              />
            </UnserInfoWWithLabel>
            <UnserInfoWWithLabel label="Địa chỉ">
              <TextEditable
                value={userInfo.address}
                primaryKey={userInfo.userHashedId}
                propertyName="Adreess"
                onUpdated={onEditable}
              />
            </UnserInfoWWithLabel>
            <UnserInfoWWithLabel label="Quốc gia">
              {userInfo.country}
            </UnserInfoWWithLabel>
            <UnserInfoWWithLabel label="Sinh nhật">
              {format(userInfo.birthDate, "MMMM, DD YYYY")}
            </UnserInfoWWithLabel>
            <UnserInfoWWithLabel label="Ngày tham gia">
              {format(userInfo.createdDate, "MMMM, DD YYYY")}
            </UnserInfoWWithLabel>
            <UnserInfoWWithLabel label="Email" isEmail={true}>
              {userInfo.email}
            </UnserInfoWWithLabel>
            <UnserInfoWWithLabel label="Điện thoại">
              {userInfo.mobile}
            </UnserInfoWWithLabel>
            <UnserInfoWWithLabel label="Trạng thái">
              {userInfo.statusLabel}
            </UnserInfoWWithLabel>
          </InfoList>
        ) : null}
      </Root>
    </MainPanel>
  );
}
