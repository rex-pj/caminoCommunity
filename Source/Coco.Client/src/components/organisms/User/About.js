import React, { Component } from "react";
import styled from "styled-components";
import { Mutation } from "react-apollo";
import { PanelBody } from "../../../components/atoms/Panels";
import { VerticalList } from "../../atoms/List";
import { format } from "date-fns";
import TextEditable from "../../molecules/Editable/TextEditable";
import SelectEditable from "../../molecules/Editable/SelectEditable";
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
  constructor(props) {
    super(props);
    const { userInfo } = props;

    this.state = {
      ...userInfo
    };
  }

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

  handleInputChange(e) {
    const targetName = e.target.name;

    this.setState({
      [targetName]: e.target.value
    });
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
                          value={this.state.lastname}
                          onChange={e => this.handleInputChange(e)}
                          primaryKey={this.state.userHashedId}
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
                          value={this.state.firstname}
                          onChange={e => this.handleInputChange(e)}
                          primaryKey={this.state.userHashedId}
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
                      value={this.state.displayName}
                      onChange={e => this.handleInputChange(e)}
                      primaryKey={this.state.userHashedId}
                      name="displayName"
                      onUpdated={e => this.onEditable(e, updateUserInfoItem)}
                      disabled={!canEdit}
                    />
                  </UnserInfoWWithLabel>
                  <UnserInfoWWithLabel label="Về bản thân">
                    <TextEditable
                      value={this.state.description}
                      onChange={e => this.handleInputChange(e)}
                      primaryKey={this.state.userHashedId}
                      name="description"
                      onUpdated={e => this.onEditable(e, updateUserInfoItem)}
                      disabled={!canEdit}
                    />
                  </UnserInfoWWithLabel>
                  <UnserInfoWWithLabel label="Điện thoại">
                    <TextEditable
                      value={this.state.phoneNumber}
                      onChange={e => this.handleInputChange(e)}
                      primaryKey={this.state.userHashedId}
                      name="phoneNumber"
                      onUpdated={e => this.onEditable(e, updateUserInfoItem)}
                      disabled={!canEdit}
                    />
                  </UnserInfoWWithLabel>
                  <UnserInfoWWithLabel label="Giới tính">
                    <SelectEditable
                      value={this.state.genderId}
                      onChange={e => this.handleInputChange(e)}
                      primaryKey={this.state.userHashedId}
                      name="genderId"
                      onUpdated={e => this.onEditable(e, updateUserInfoItem)}
                      disabled={!canEdit}
                      selections={this.state.genderSelections}
                    />
                  </UnserInfoWWithLabel>
                  <UnserInfoWWithLabel label="Địa chỉ">
                    <TextEditable
                      value={this.state.address}
                      onChange={e => this.handleInputChange(e)}
                      primaryKey={this.state.userHashedId}
                      name="address"
                      onUpdated={e => this.onEditable(e, updateUserInfoItem)}
                      disabled={!canEdit}
                    />
                  </UnserInfoWWithLabel>
                  <UnserInfoWWithLabel label="Quốc gia">
                    {this.state.country}
                  </UnserInfoWWithLabel>
                  <UnserInfoWWithLabel label="Sinh nhật">
                    {format(this.state.birthDate, "MMMM, DD YYYY")}
                  </UnserInfoWWithLabel>
                  <UnserInfoWWithLabel label="Email" isEmail={true}>
                    {this.state.email}
                  </UnserInfoWWithLabel>
                  <UnserInfoWWithLabel label="Ngày tham gia">
                    {format(this.state.createdDate, "MMMM, DD YYYY")}
                  </UnserInfoWWithLabel>
                  <UnserInfoWWithLabel label="Trạng thái">
                    {this.state.statusLabel}
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
