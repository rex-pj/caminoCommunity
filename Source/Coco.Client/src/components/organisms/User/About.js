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

export default class extends Component {
  async onEditable(e, updateUserInfoItem) {
    const { canEdit } = this.props;
    if (updateUserInfoItem) {
      return await updateUserInfoItem({
        variables: {
          criterias: {
            key: e.primaryKey,
            value: e.value,
            propertyName: e.propertyName,
            canEdit
          }
        }
      });
    }
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
                  <UnserInfoWWithLabel label="Họ &amp; Tên">
                    <div className="row">
                      <div className="col-auto">
                        <TextEditable
                          value={userInfo.lastname}
                          primaryKey={userInfo.userHashedId}
                          name="lastname"
                          onUpdated={e =>
                            this.onEditable(e, updateUserInfoItem)
                          }
                          disabled={!canEdit}
                        />
                      </div>
                      <div className="col-auto">-</div>
                      <div className="col-auto">
                        <TextEditable
                          value={userInfo.firstname}
                          primaryKey={userInfo.userHashedId}
                          name="firstname"
                          onUpdated={e =>
                            this.onEditable(e, updateUserInfoItem)
                          }
                          disabled={!canEdit}
                        />
                      </div>
                    </div>
                  </UnserInfoWWithLabel>
                  <UnserInfoWWithLabel label="Tên hiển thị">
                    <TextEditable
                      value={userInfo.displayName}
                      primaryKey={userInfo.userHashedId}
                      name="displayName"
                      onUpdated={e => this.onEditable(e, updateUserInfoItem)}
                      disabled={!canEdit}
                    />
                  </UnserInfoWWithLabel>
                  <UnserInfoWWithLabel label="Về bản thân">
                    <TextAreaEditable
                      rows={5}
                      cols={50}
                      value={userInfo.description}
                      primaryKey={userInfo.userHashedId}
                      name="description"
                      onUpdated={e => this.onEditable(e, updateUserInfoItem)}
                      disabled={!canEdit}
                    />
                  </UnserInfoWWithLabel>
                  <UnserInfoWWithLabel label="Điện thoại">
                    <TextEditable
                      value={userInfo.phoneNumber}
                      primaryKey={userInfo.userHashedId}
                      name="phoneNumber"
                      onUpdated={e => this.onEditable(e, updateUserInfoItem)}
                      disabled={!canEdit}
                    />
                  </UnserInfoWWithLabel>
                  <UnserInfoWWithLabel label="Giới tính">
                    <SelectEditable
                      value={userInfo.genderId}
                      primaryKey={userInfo.userHashedId}
                      name="genderId"
                      emptyText="Chọn giới tính"
                      onUpdated={e => this.onEditable(e, updateUserInfoItem)}
                      disabled={!canEdit}
                      selections={userInfo.genderSelections}
                    />
                  </UnserInfoWWithLabel>
                  <UnserInfoWWithLabel label="Địa chỉ">
                    <TextEditable
                      value={userInfo.address}
                      primaryKey={userInfo.userHashedId}
                      name="address"
                      onUpdated={e => this.onEditable(e, updateUserInfoItem)}
                      disabled={!canEdit}
                    />
                  </UnserInfoWWithLabel>
                  <UnserInfoWWithLabel label="Quốc gia">
                    <SelectEditable
                      value={userInfo.countryId}
                      primaryKey={userInfo.userHashedId}
                      name="countryId"
                      emptyText="Chọn quốc gia của bạn"
                      onUpdated={e => this.onEditable(e, updateUserInfoItem)}
                      disabled={!canEdit}
                      selections={userInfo.countrySelections}
                    />
                    {userInfo.country}
                  </UnserInfoWWithLabel>
                  <UnserInfoWWithLabel label="Sinh nhật">
                    <DateTimeEditable
                      value={userInfo.birthDate}
                      primaryKey={userInfo.userHashedId}
                      name="birthDate"
                      onUpdated={e => this.onEditable(e, updateUserInfoItem)}
                      disabled={!canEdit}
                    />
                  </UnserInfoWWithLabel>
                  <UnserInfoWWithLabel label="Email" isEmail={true}>
                    {userInfo.email}
                  </UnserInfoWWithLabel>
                  <UnserInfoWWithLabel label="Ngày tham gia">
                    {format(userInfo.createdDate, "MMMM, DD YYYY")}
                  </UnserInfoWWithLabel>
                  <UnserInfoWWithLabel label="Trạng thái">
                    {userInfo.statusLabel}
                  </UnserInfoWWithLabel>
                </InfoList>
              )}
            </Mutation>
          ) : null}
        </Root>
      </MainPanel>
    );
  }
}
