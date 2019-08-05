import React, { Component } from "react";
import styled from "styled-components";
import { Mutation } from "react-apollo";
import { PanelBody } from "../../../components/atoms/Panels";
import { VerticalList } from "../../atoms/List";
import { format } from "date-fns";
import TextEditable from "../../molecules/Editable/TextEditable";
import SelectEditable from "../../molecules/Editable/SelectEditable";
import DateTimeEditable from "../../molecules/Editable/DateTimeEditable";
import TextAreaEditable from "../../molecules/Editable/TextAreaEditable";
import LabelAndInfo from "../../molecules/InfoWithLabels/LabelAndInfo";
import { UPDATE_USER_INFO_PER_ITEM } from "../../../utils/GraphQLQueries";

const MainPanel = styled(PanelBody)`
  border-radius: ${p => p.theme.borderRadius.normal};
  box-shadow: ${p => p.theme.shadow.BoxShadow};
  margin-bottom: ${p => p.theme.size.normal};
  background-color: ${p => p.theme.color.white};
`;

const Root = styled.div`
  position: relative;
  border-radius: ${p => p.theme.borderRadius.normal};
`;

const InfoList = styled(VerticalList)`
  margin-bottom: 0;
`;

export default class extends Component {
  async onEditable(e, updateUserInfoItem) {
    return await this.props.onEdited(e, updateUserInfoItem);
  }

  render() {
    const { userInfo, canEdit } = this.props;
    return (
      <MainPanel>
        <Root>
          {userInfo ? (
            <Mutation mutation={UPDATE_USER_INFO_PER_ITEM}>
              {updateUserInfoItem => (
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
                      onUpdated={e => this.onEditable(e, updateUserInfoItem)}
                      disabled={!canEdit}
                    />
                  </LabelAndInfo>
                  <LabelAndInfo label="Điện thoại">
                    <TextEditable
                      value={userInfo.phoneNumber}
                      primaryKey={userInfo.userIdentityId}
                      name="phoneNumber"
                      onUpdated={e => this.onEditable(e, updateUserInfoItem)}
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
                      onUpdated={e => this.onEditable(e, updateUserInfoItem)}
                      disabled={!canEdit}
                      selections={userInfo.genderSelections}
                    />
                  </LabelAndInfo>
                  <LabelAndInfo label="Địa chỉ">
                    <TextEditable
                      value={userInfo.address}
                      primaryKey={userInfo.userIdentityId}
                      name="address"
                      onUpdated={e => this.onEditable(e, updateUserInfoItem)}
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
                      onUpdated={e => this.onEditable(e, updateUserInfoItem)}
                      disabled={!canEdit}
                      selections={userInfo.countrySelections}
                    />
                    {userInfo.country}
                  </LabelAndInfo>
                  <LabelAndInfo label="Sinh nhật">
                    <DateTimeEditable
                      value={userInfo.birthDate}
                      primaryKey={userInfo.userIdentityId}
                      name="birthDate"
                      onUpdated={e => this.onEditable(e, updateUserInfoItem)}
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
              )}
            </Mutation>
          ) : null}
        </Root>
      </MainPanel>
    );
  }
}
