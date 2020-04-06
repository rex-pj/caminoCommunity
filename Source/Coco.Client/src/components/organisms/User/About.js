import React from "react";
import styled from "styled-components";
import { PanelBody } from "../../../components/atoms/Panels";
import { VerticalList } from "../../atoms/List";
import { format } from "date-fns";
import TextEditable from "../../molecules/Editable/TextEditable";
import SelectEditable from "../../molecules/Editable/SelectEditable";
import DateTimeEditable from "../../molecules/Editable/DateTimeEditable";
import TextAreaEditable from "../../molecules/Editable/TextAreaEditable";
import LabelAndInfo from "../../molecules/InfoWithLabels/LabelAndInfo";

const MainPanel = styled(PanelBody)`
  border-radius: ${(p) => p.theme.borderRadius.normal};
  box-shadow: ${(p) => p.theme.shadow.BoxShadow};
  margin-bottom: ${(p) => p.theme.size.normal};
  background-color: ${(p) => p.theme.color.white};
`;

const Root = styled.div`
  position: relative;
  border-radius: ${(p) => p.theme.borderRadius.normal};
`;

const InfoList = styled(VerticalList)`
  margin-bottom: 0;
`;

export default (props) => {
  const onEdited = async (e) => {
    return await props.onEdited(e);
  };

  const { userInfo, canEdit } = props;
  const { countrySelections } = userInfo;
  let countries = [];
  if (countrySelections) {
    countries = countrySelections.map((country) => {
      return {
        id: country.id,
        text: country.name,
      };
    });
  }
  return (
    <MainPanel>
      <Root>
        {userInfo ? (
          <InfoList>
            <LabelAndInfo label="Họ &amp; Tên">
              <div className="row">
                <div className="col-auto">{userInfo.lastname}</div>
                <div className="col-auto">-</div>
                <div className="col-auto">{userInfo.firstname}</div>
              </div>
            </LabelAndInfo>
            <LabelAndInfo label="Tên hiển thị">
              {userInfo.displayName}
            </LabelAndInfo>
            <LabelAndInfo label="Về bản thân">
              <TextAreaEditable
                rows={5}
                cols={50}
                value={userInfo.description}
                primaryKey={userInfo.userIdentityId}
                name="description"
                onUpdated={(e) => onEdited(e)}
                disabled={!canEdit}
              />
            </LabelAndInfo>
            <LabelAndInfo label="Điện thoại">
              <TextEditable
                value={userInfo.phoneNumber}
                primaryKey={userInfo.userIdentityId}
                name="phoneNumber"
                onUpdated={(e) => onEdited(e)}
                disabled={!canEdit}
              />
            </LabelAndInfo>
            <LabelAndInfo label="Giới tính">
              <SelectEditable
                value={userInfo.genderId}
                text={userInfo.genderLabel}
                primaryKey={userInfo.userIdentityId}
                name="genderId"
                emptyText="Chọn giới tính"
                onUpdated={(e) => onEdited(e)}
                disabled={!canEdit}
                selections={userInfo.genderSelections}
              />
            </LabelAndInfo>
            <LabelAndInfo label="Địa chỉ">
              <TextEditable
                value={userInfo.address}
                primaryKey={userInfo.userIdentityId}
                name="address"
                onUpdated={(e) => onEdited(e)}
                disabled={!canEdit}
              />
            </LabelAndInfo>
            <LabelAndInfo label="Quốc gia">
              <SelectEditable
                value={userInfo.countryId}
                text={userInfo.countryName}
                primaryKey={userInfo.userIdentityId}
                name="countryId"
                emptyText="Chọn quốc gia của bạn"
                onUpdated={(e) => onEdited(e)}
                disabled={!canEdit}
                selections={countries}
              />
              {userInfo.country}
            </LabelAndInfo>
            <LabelAndInfo label="Sinh nhật">
              <DateTimeEditable
                value={userInfo.birthDate}
                primaryKey={userInfo.userIdentityId}
                name="birthDate"
                onUpdated={(e) => onEdited(e)}
                disabled={!canEdit}
              />
            </LabelAndInfo>
            <LabelAndInfo label="Email" isEmail={true}>
              {userInfo.email}
            </LabelAndInfo>
            <LabelAndInfo label="Ngày tham gia">
              {format(userInfo.createdDate, "MMMM, DD YYYY")}
            </LabelAndInfo>
            <LabelAndInfo label="Trạng thái">
              {userInfo.statusLabel}
            </LabelAndInfo>
          </InfoList>
        ) : null}
      </Root>
    </MainPanel>
  );
};
