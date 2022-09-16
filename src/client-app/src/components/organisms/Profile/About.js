import React from "react";
import styled from "styled-components";
import { PanelBody } from "../../molecules/Panels";
import { VerticalList } from "../../molecules/List";
import { format } from "date-fns";
import TextEditable from "../Editable/TextEditable";
import SelectEditable from "../Editable/SelectEditable";
import DateTimeEditable from "../Editable/DateTimeEditable";
import TextAreaEditable from "../Editable/TextAreaEditable";
import LabelAndInfo from "../../molecules/InfoWithLabels/LabelAndInfo";
import { mapSelectOptions } from "../../../utils/SelectOptionUtils";

const MainPanel = styled(PanelBody)`
  border-radius: ${(p) => p.theme.borderRadius.normal};
  box-shadow: ${(p) => p.theme.shadow.BoxShadow};
  margin-bottom: ${(p) => p.theme.size.normal};
  background-color: ${(p) => p.theme.color.whiteBg};
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

  const { userInfo, canEdit, countrySelections, genderSelections } = props;

  return (
    <MainPanel>
      <Root>
        {userInfo ? (
          <InfoList>
            <LabelAndInfo label="Name">
              <div className="row">
                <div className="col-auto">{userInfo.lastname}</div>
                <div className="col-auto">-</div>
                <div className="col-auto">{userInfo.firstname}</div>
              </div>
            </LabelAndInfo>
            <LabelAndInfo label="Display Name">
              {userInfo.displayName}
            </LabelAndInfo>
            <LabelAndInfo label="Bio">
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
            <LabelAndInfo label="Phone number">
              <TextEditable
                value={userInfo.phoneNumber}
                primaryKey={userInfo.userIdentityId}
                name="phoneNumber"
                onUpdated={(e) => onEdited(e)}
                disabled={!canEdit}
              />
            </LabelAndInfo>
            <LabelAndInfo label="Sex">
              <SelectEditable
                value={userInfo.genderId}
                label={userInfo.genderLabel}
                primaryKey={userInfo.userIdentityId}
                name="genderId"
                emptyText="Your sex"
                onUpdated={(e) => onEdited(e)}
                disabled={!canEdit}
                selections={mapSelectOptions(genderSelections)}
              />
            </LabelAndInfo>
            <LabelAndInfo label="Address">
              <TextEditable
                value={userInfo.address}
                primaryKey={userInfo.userIdentityId}
                name="address"
                onUpdated={(e) => onEdited(e)}
                disabled={!canEdit}
              />
            </LabelAndInfo>
            <LabelAndInfo label="Country">
              <SelectEditable
                value={userInfo.countryId}
                label={userInfo.countryName}
                primaryKey={userInfo.userIdentityId}
                name="countryId"
                emptyText="Select your country"
                onUpdated={(e) => onEdited(e)}
                disabled={!canEdit}
                selections={mapSelectOptions(countrySelections)}
              />
              {userInfo.country}
            </LabelAndInfo>
            <LabelAndInfo label="Date of Birth">
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
            <LabelAndInfo label="Joined Date">
              {format(new Date(userInfo.createdDate), "MMMM, dd yyyy")}
            </LabelAndInfo>
            <LabelAndInfo label="Status">{userInfo.statusLabel}</LabelAndInfo>
          </InfoList>
        ) : null}
      </Root>
    </MainPanel>
  );
};
