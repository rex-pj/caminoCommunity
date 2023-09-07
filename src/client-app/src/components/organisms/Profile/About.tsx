import { useContext } from "react";
import styled from "styled-components";
import { PanelBody } from "../../molecules/Panels";
import { VerticalList } from "../../molecules/List";
import { format } from "date-fns";
import TextEditable from "../Editable/TextEditable";
import SelectEditable from "../Editable/SelectEditable";
import DateTimeEditable from "../Editable/DateTimeEditable";
import TextAreaEditable from "../Editable/TextAreaEditable";
import LabelAndInfo from "../../molecules/InfoWithLabels/LabelAndInfo";
import { SelectOption, mapSelectOptions } from "../../../utils/SelectOptionUtils";
import { SessionContext } from "../../../store/context/session-context";
import { useTranslation } from "react-i18next";

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

type Props = {
  onEdited: (e: any) => Promise<any>;
  userInfo: any;
  canEdit?: boolean;
  countrySelections?: SelectOption[];
  genderSelections?: SelectOption[];
};

const About = (props: Props) => {
  const { t } = useTranslation();
  const { currentUser } = useContext(SessionContext);
  const onEdited = async (e: any) => {
    await props.onEdited(e);
  };

  const { userInfo, canEdit, countrySelections, genderSelections } = props;

  const isLoggedUserId = userInfo && currentUser && userInfo?.userIdentityId === currentUser?.userIdentityId;
  return (
    <MainPanel>
      <Root>
        {userInfo ? (
          <InfoList>
            <LabelAndInfo label={t("name_label")}>
              <div className="row">
                <div className="col-auto">{userInfo.lastname}</div>
                <div className="col-auto">-</div>
                <div className="col-auto">{userInfo.firstname}</div>
              </div>
            </LabelAndInfo>
            <LabelAndInfo label={t("display_name_label")}>{userInfo.displayName}</LabelAndInfo>
            <LabelAndInfo label={t("bio_label")}>
              <TextAreaEditable rows={5} cols={50} value={userInfo.description} primaryKey={userInfo.userIdentityId} name="description" onUpdated={(e) => onEdited(e)} disabled={!canEdit} />
            </LabelAndInfo>
            <LabelAndInfo label={t("phone_number_label")}>
              <TextEditable value={userInfo.phoneNumber} primaryKey={userInfo.userIdentityId} name="phoneNumber" onUpdated={(e) => onEdited(e)} disabled={!canEdit} />
            </LabelAndInfo>
            <LabelAndInfo label={t("sex_label")}>
              <SelectEditable value={userInfo.genderId} label={userInfo.genderLabel} primaryKey={userInfo.userIdentityId} name="genderId" emptyText={t("your_sex")} onUpdated={(e) => onEdited(e)} disabled={!canEdit} selections={mapSelectOptions(genderSelections)} />
            </LabelAndInfo>
            <LabelAndInfo label={t("address_label")}>
              <TextEditable value={userInfo.address} primaryKey={userInfo.userIdentityId} name="address" onUpdated={(e) => onEdited(e)} disabled={!canEdit} />
            </LabelAndInfo>
            <LabelAndInfo label={t("country_label")}>
              <SelectEditable value={userInfo.countryId} label={userInfo.countryName} primaryKey={userInfo.userIdentityId} name="countryId" emptyText={t("select_your_country")} onUpdated={(e) => onEdited(e)} disabled={!canEdit} selections={mapSelectOptions(countrySelections)} />
              {userInfo.country}
            </LabelAndInfo>
            <LabelAndInfo label={t("date_of_birth_label")}>
              <DateTimeEditable value={userInfo.birthDate} primaryKey={userInfo.userIdentityId} name="birthDate" onUpdated={(e) => onEdited(e)} disabled={!canEdit} />
            </LabelAndInfo>
            {isLoggedUserId ? (
              <LabelAndInfo label={t("email_or_phone_label")} isEmail={true}>
                {userInfo.email}
              </LabelAndInfo>
            ) : null}
            <LabelAndInfo label={t("joined_date_label")}>{format(new Date(userInfo.createdDate), "MMMM, dd yyyy")}</LabelAndInfo>
            <LabelAndInfo label={t("status_label")}>{userInfo.statusLabel}</LabelAndInfo>
          </InfoList>
        ) : null}
      </Root>
    </MainPanel>
  );
};

export default About;
