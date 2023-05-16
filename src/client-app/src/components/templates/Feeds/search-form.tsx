import * as React from "react";
import { Fragment, useState } from "react";
import { LabelPrimary } from "../../atoms/Labels";
import { SecondaryDarkHeading } from "../../atoms/Heading";
import { LightTextbox } from "../../atoms/Textboxes";
import styled from "styled-components";
import { ButtonPrimary, ButtonTransparent } from "../../atoms/Buttons/Buttons";
import Select, {
  ActionMeta,
  OnChangeValue,
  OptionsOrGroups,
} from "react-select";
import AsyncSelect from "react-select/async";
import { useParams } from "react-router-dom";
import { mapSelectOptions } from "../../../utils/SelectOptionUtils";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { FeedType } from "../../../utils/Enums";
import { IAdvancedSearchResult } from ".";

const SearchInput = styled(LightTextbox)`
  width: 100%;
`;

const Footer = styled.div`
  text-align: right;
`;

export interface ISearchFormParams {
  keyword?: string;
  userIdentityId?: string;
  hoursCreatedFrom?: any;
  hoursCreatedTo?: any;
  feedFilterType?: FeedType;
  [key: string]: any;
}

type Props = {
  selectUsers: (e: any) => Promise<any>;
  searchParams: ISearchFormParams;
  advancedSearchResult: IAdvancedSearchResult;
  onSearch: (e: ISearchFormParams) => void;
  feedTypeDictonaries: any;
  timeOptions: any[];
  baseUrl?: string;
};

const SearchForm = (props: Props) => {
  const {
    selectUsers,
    onSearch,
    searchParams,
    advancedSearchResult,
    feedTypeDictonaries,
    timeOptions,
  } = props;
  const { keyword } = useParams();

  const [searchData, setSearchData] = useState<ISearchFormParams>({
    keyword: keyword,
    userIdentityId: searchParams.userIdentityId,
    hoursCreatedFrom: searchParams.hoursCreatedFrom,
    hoursCreatedTo: searchParams.hoursCreatedTo,
    feedFilterType: searchParams.feedFilterType,
  });

  const handleSelectChange = (
    newValue: OnChangeValue<any, any>,
    actionMeta: ActionMeta<any>
  ) => {
    let searchFormData = searchData;
    const { name } = actionMeta;
    const { action } = actionMeta;
    if ((action === "clear" || action === "remove-value") && name) {
      searchFormData[name] = null;
    } else if (name) {
      searchFormData[name] = newValue.value;
    }

    setSearchData({
      ...searchFormData,
    });
  };

  const handleInputChange = (evt: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = evt.target;
    let searchFormData = searchData;
    searchFormData[name] = value;

    setSearchData({ ...searchFormData });
  };

  const loadUserSelected = () => {
    const { userIdentityId } = searchData;
    if (!userIdentityId) {
      return null;
    }

    const { userFilterByName } = advancedSearchResult;
    return {
      label: userFilterByName,
      value: userIdentityId,
    };
  };

  const loadTimeSelected = (hoursValue: any) => {
    if (!hoursValue) {
      return null;
    }
    return timeOptions.find((x) => x.value === parseInt(hoursValue));
  };

  const loadUserSelections = (
    inputValue: string,
    callback: (
      options: OptionsOrGroups<any, any>
    ) => Promise<OptionsOrGroups<any, any>> | void
  ) => {
    return selectUsers({
      variables: {
        criterias: { search: inputValue, page: 1 },
      },
    })
      .then((response) => {
        const { data } = response;
        const { selectUsers } = data;
        return mapSelectOptions(selectUsers);
      })
      .catch((error) => {
        return [];
      });
  };

  const executeSearch = () => {
    const { userIdentityId, hoursCreatedFrom, hoursCreatedTo, feedFilterType } =
      searchData;
    onSearch({
      keyword: searchData.keyword,
      userIdentityId,
      hoursCreatedFrom,
      hoursCreatedTo,
      feedFilterType,
    });
  };

  const loadFilterTypeLabel = (filterType: FeedType) => {
    return feedTypeDictonaries[filterType];
  };

  const onRemoveFeedType = () => {
    const { userIdentityId, hoursCreatedFrom, hoursCreatedTo } = searchData;
    onSearch({
      keyword: searchData.keyword ? searchData.keyword : "",
      userIdentityId,
      hoursCreatedFrom,
      hoursCreatedTo,
      feedFilterType: FeedType.Undefinded,
    });
  };

  const onEnterExecuteSearch = (evt: React.KeyboardEvent<HTMLInputElement>) => {
    if (evt.key === "Enter") {
      executeSearch();
    }
  };

  return (
    <Fragment>
      <SecondaryDarkHeading className="mb-3">
        Kết quả tìm kiếm cho <strong>"{keyword}"</strong>
      </SecondaryDarkHeading>
      <div className="row">
        <div className="mb-3 col-6 col-md-12">
          <LabelPrimary>Từ khóa</LabelPrimary>
          <SearchInput
            placeholder="Từ khóa"
            type="text"
            name="keyword"
            autoComplete="off"
            onKeyUpCapture={onEnterExecuteSearch}
            value={searchData.keyword}
            onChange={(e) => handleInputChange(e)}
          />
        </div>
        <div className="mb-3 col-6 col-md-12">
          <LabelPrimary>Được tạo bởi</LabelPrimary>
          <AsyncSelect
            className="cate-selection"
            cacheOptions
            defaultOptions
            name="userIdentityId"
            defaultValue={loadUserSelected()}
            onChange={(e, action) => handleSelectChange(e, action)}
            loadOptions={loadUserSelections}
            isClearable={true}
          />
        </div>
      </div>
      <div className="row">
        <div className="mb-3 col-6 col-md-12">
          <LabelPrimary>Từ</LabelPrimary>
          <Select
            options={timeOptions}
            name="hoursCreatedFrom"
            defaultValue={loadTimeSelected(searchData.hoursCreatedFrom)}
            onChange={(e, action) => handleSelectChange(e, action)}
            isClearable={true}
          />
        </div>
        <div className="mb-3 col-6 col-md-12">
          <LabelPrimary>Đến</LabelPrimary>
          <Select
            options={timeOptions}
            name="hoursCreatedTo"
            defaultValue={loadTimeSelected(searchData.hoursCreatedTo)}
            onChange={(e, action) => handleSelectChange(e, action)}
            isClearable={true}
          />
        </div>
      </div>

      {searchData.feedFilterType ? (
        <div className="mb-3">
          <LabelPrimary>Hiển thị kết quả cho</LabelPrimary>
          <div>
            {loadFilterTypeLabel(Number(searchData.feedFilterType))}
            <ButtonTransparent>
              <FontAwesomeIcon
                icon="times"
                onClick={onRemoveFeedType}
              ></FontAwesomeIcon>
            </ButtonTransparent>
          </div>
        </div>
      ) : null}
      <Footer className="mb-3">
        <ButtonPrimary size="xs" onClick={executeSearch}>
          <span className="me-1">Tìm</span>
          <FontAwesomeIcon icon="search"></FontAwesomeIcon>
        </ButtonPrimary>
      </Footer>
    </Fragment>
  );
};

export default SearchForm;
