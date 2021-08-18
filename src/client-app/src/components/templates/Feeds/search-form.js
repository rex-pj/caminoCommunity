import React, { Fragment, useState } from "react";
import { LabelSecondary } from "../../atoms/Labels";
import { SecondaryDarkHeading } from "../../atoms/Heading";
import { LightTextbox } from "../../atoms/Textboxes";
import styled from "styled-components";
import { ButtonPrimary, ButtonTransparent } from "../../atoms/Buttons/Buttons";
import Select from "react-select";
import AsyncSelect from "react-select/async";
import { withRouter } from "react-router-dom";
import { mapSelectOptions } from "../../../utils/SelectOptionUtils";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

const SearchInput = styled(LightTextbox)`
  width: 100%;
`;

const Footer = styled.div`
  text-align: right;
`;

export default withRouter((props) => {
  const {
    match,
    selectUsers,
    onSearch,
    searchParams,
    advancedSearchResult,
    feedTypeDictonaries,
    timeOptions,
  } = props;
  const { params } = match;
  const { keyword } = params;

  const [searchData, setSearchData] = useState({
    keyword: keyword,
    userIdentityId: searchParams.userIdentityId,
    hoursCreatedFrom: searchParams.hoursCreatedFrom,
    hoursCreatedTo: searchParams.hoursCreatedTo,
    feedFilterType: searchParams.feedFilterType,
  });

  const handleSelectChange = (evt, method) => {
    let searchFormData = searchData;
    const { name } = method;
    const { action } = method;
    if (action === "clear" || action === "remove-value") {
      searchFormData[name] = null;
    } else {
      searchFormData[name] = evt.value;
    }

    setSearchData({
      ...searchFormData,
    });
  };

  const handleInputChange = (evt) => {
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

  const loadTimeSelected = (hoursValue) => {
    if (!hoursValue) {
      return null;
    }
    return timeOptions.find((x) => x.value === parseInt(hoursValue));
  };

  const loadUserSelections = (value) => {
    return selectUsers({
      variables: {
        criterias: { search: value },
      },
    })
      .then((response) => {
        var { data } = response;
        var { selectUsers } = data;
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

  const loadFilterTypeLabel = (filterType) => {
    return feedTypeDictonaries[filterType];
  };

  const onRemoveFeedType = () => {
    const { userIdentityId, hoursCreatedFrom, hoursCreatedTo } = searchData;
    onSearch({
      keyword: searchData.keyword ? searchData.keyword : "",
      userIdentityId,
      hoursCreatedFrom,
      hoursCreatedTo,
      feedFilterType: null,
    });
  };

  const onEnterExecuteSearch = (evt) => {
    if (evt.keyCode === 13) {
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
          <LabelSecondary>Từ khóa</LabelSecondary>
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
          <LabelSecondary>Được tạo bởi</LabelSecondary>
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
          <LabelSecondary>Từ</LabelSecondary>
          <Select
            options={timeOptions}
            name="hoursCreatedFrom"
            defaultValue={loadTimeSelected(searchData.hoursCreatedFrom)}
            onChange={(e, action) => handleSelectChange(e, action)}
            isClearable={true}
          />
        </div>
        <div className="mb-3 col-6 col-md-12">
          <LabelSecondary>Đến</LabelSecondary>
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
          <LabelSecondary>Hiển thị kết quả cho</LabelSecondary>
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
});
