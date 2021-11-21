import React, { useState, useEffect, useRef } from "react";
import { useLazyQuery } from "@apollo/client";
import styled from "styled-components";
import { PrimaryTextbox } from "../../atoms/Textboxes";
import { ButtonTransparent } from "../../atoms/Buttons/Buttons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faSearch } from "@fortawesome/free-solid-svg-icons";
import { feedqueries } from "../../../graphql/fetching/queries";
import Dropdown from "../../molecules/DropdownButton/Dropdown";
import { FeedType } from "../../../utils/Enums";
import { UrlConstant } from "../../../utils/Constants";
import { AnchorLink } from "../../atoms/Links";
import { ImageRound } from "../../atoms/Images";
import NoImage from "../../molecules/NoImages/no-image";
import { withRouter } from "react-router-dom";

const DropdownPanel = styled.div`
  position: absolute;
  top: 100%;
  background: ${(p) => p.theme.color.whiteBg};
  width: 100%;
  border-radius: ${(p) => p.theme.borderRadius.normal};
  box-shadow: ${(p) => p.theme.shadow.BoxShadow};
`;

const DropdownItem = styled.li`
  > a {
    padding: ${(p) => p.theme.size.exSmall} ${(p) => p.theme.size.exSmall};
    display: block;
    :hover {
      background: ${(p) => p.theme.color.lightBg};
    }
    border-radius: ${(p) => p.theme.borderRadius.normal};
    text-overflow: ellipsis;
    white-space: nowrap;
    overflow: hidden;
    width: 100%;
  }

  > a svg,
  > a path {
    color: ${(p) => p.theme.color.primaryLink};
  }

  ${ImageRound} {
    width: ${(p) => p.theme.size.small};
    height: ${(p) => p.theme.size.small};
    margin-right: ${(p) => p.theme.size.exTiny};
  }
`;

const SearchForm = styled.div`
  position: relative;
  border-radius: ${(p) => p.theme.size.normal};
  border: 0;
  background-color: ${(p) => p.theme.rgbaColor.dark};
  height: ${(p) => p.theme.size.normal};
  margin: 1px 0;
  :focus-within {
    background-color: ${(p) => p.theme.rgbaColor.darker};
  }
`;

const MoreSearchResultsFooter = styled.div`
  > a {
    border-radius: ${(p) => p.theme.borderRadius.normal};
    display: block;
    padding: ${(p) => p.theme.size.exSmall};
    :hover {
      background-color: ${(p) => p.theme.color.lightBg};
    }
  }

  > a,
  > a > strong,
  > a svg,
  > a path {
    color: ${(p) => p.theme.color.secondaryText};
  }
`;

const SearchInput = styled(PrimaryTextbox)`
  border-top-right-radius: 100%;
  border-bottom-right-radius: 100%;
  background: transparent;
  border: 0;
  width: 300px;
  height: calc(${(p) => p.theme.size.normal} - 2px);
  float: left;
  color: ${(p) => p.theme.color.neutralText};
  max-width: calc(100% - ${(p) => p.theme.size.normal});

  ::placeholder {
    color: ${(p) => p.theme.color.neutralText};
    font-size: ${(p) => p.theme.fontSize.small};
  }
`;

const SearchButton = styled(ButtonTransparent)`
  border-radius: 100%;
  height: calc(${(p) => p.theme.size.normal} - 6px);
  width: calc(${(p) => p.theme.size.normal} - 6px);
  padding: 0;
  border: 0;
  float: left;
  margin: 2px 0 2px 2px;

  :active,
  :hover,
  :focus-within {
    background-color: ${(p) => p.theme.rgbaColor.dark};
  }

  :disabled {
    background-color: ${(p) => p.theme.rgbaColor.light};
  }

  svg,
  path {
    color: ${(p) => p.theme.color.neutralText};
  }
`;

const ClearButton = styled(ButtonTransparent)`
  position: absolute;
  border-radius: 100%;
  height: calc(${(p) => p.theme.size.normal} - 4px);
  width: calc(${(p) => p.theme.size.normal} - 4px);
  padding: 0;
  border: 0;
  right: 1px;
  margin: 1px 0;

  svg,
  path {
    color: ${(p) => p.theme.color.neutralText};
  }
`;

const EmptyImage = styled(NoImage)`
  border-radius: ${(p) => p.theme.borderRadius.normal};
  width: 28px;
  height: 28px;
  font-size: 16px;
  display: inline-block;
  vertical-align: middle;
  margin-right: ${(p) => p.theme.size.exTiny};
`;

export default withRouter((props) => {
  const {
    match: {
      params: { keyword: searchText },
    },
    history,
  } = props;
  const [fetchResults] = useLazyQuery(feedqueries.LIVE_SEARCH, {
    onCompleted: (response) => onSearchCompleted(response),
    onError: (error) => onSearchError(error),
    fetchPolicy: "no-cache",
  });

  const [searchData, setSearchData] = useState({
    searchResults: [],
    keyword: searchText,
    isDropdownShown: false,
  });
  const inputRef = useRef({
    isSearching: false,
  });
  const dropdownRef = useRef();

  const onSearchCompleted = (response) => {
    const {
      liveSearch: { articles, products, farms, users },
    } = response;
    setSearchData({
      ...searchData,
      searchResults: parseSearchResults([
        ...users,
        ...products,
        ...farms,
        ...articles,
      ]),
      isDropdownShown: true,
    });
    inputRef.current.isSearching = false;
  };

  const parseSearchResults = (data) => {
    return data.map((rs) => {
      let result = { ...rs };
      if (result.feedType === FeedType.Farm) {
        result.url = `${UrlConstant.Farm.url}${result.id}`;
      } else if (result.feedType === FeedType.Article) {
        result.url = `${UrlConstant.Article.url}${result.id}`;
      } else if (result.feedType === FeedType.Product) {
        result.url = `${UrlConstant.Product.url}${result.id}`;
      } else if (result.feedType === FeedType.User) {
        result.url = `${UrlConstant.Profile.url}${result.id}`;
      }

      if (result.feedType === FeedType.User) {
        result.pictureUrl = `${process.env.REACT_APP_CDN_AVATAR_API_URL}${result.pictureId}`;
      } else {
        result.pictureUrl = `${process.env.REACT_APP_CDN_PHOTO_URL}${result.pictureId}`;
      }

      return result;
    });
  };

  const onSearchError = (error) => {
    inputRef.current.isSearching = false;
  };

  const onInputChange = (e) => {
    const { value, name } = e.target;
    if (!value) {
      inputRef.current.isSearching = false;
      setSearchData({ searchResults: [], isDropdownShown: false, keyword: "" });
      return;
    }

    if (!inputRef.current.isSearching) {
      inputRef.current.isSearching = true;
      onFetchResults(value);
    }

    let searchFormData = searchData;
    searchFormData[name] = value;
    setSearchData({
      ...searchData,
    });
  };

  const onEnterExecuteSearch = (evt) => {
    if (evt.keyCode === 13) {
      onRedirectToSearch();
    }
  };

  const onRedirectToSearch = () => {
    history.push(`/search/${searchData.keyword}`);
  };

  const onFetchResults = async (value) => {
    return await fetchResults({
      variables: {
        criterias: {
          search: value,
          page: 1,
        },
      },
    });
  };

  const onClear = () => {
    if (searchData.keyword) {
      setSearchData({ searchResults: [], isDropdownShown: true, keyword: "" });
    }
  };

  const onDropdownHide = (e) => {
    if (inputRef.current && inputRef.current.contains(e.target)) {
      return;
    }
    if (dropdownRef.current && !dropdownRef.current.contains(e.target)) {
      setSearchData({ ...searchData, isDropdownShown: false });
    }
  };

  const onInputFocus = () => {
    setSearchData({ ...searchData, isDropdownShown: true });
  };

  useEffect(() => {
    document.addEventListener("click", onDropdownHide, false);
    return () => {
      document.removeEventListener("click", onDropdownHide);
    };
  });

  const { searchResults, isDropdownShown, keyword } = searchData;
  return (
    <SearchForm className={props.className}>
      <SearchButton onClick={onRedirectToSearch}>
        <FontAwesomeIcon icon={faSearch} />
      </SearchButton>
      <SearchInput
        ref={inputRef}
        onFocus={onInputFocus}
        onChange={onInputChange}
        type="text"
        name="keyword"
        defaultValue={keyword}
        placeholder="Search"
        autoComplete="off"
        onKeyUp={onEnterExecuteSearch}
      />
      <ClearButton onClick={onClear}>
        <FontAwesomeIcon icon="times" />
      </ClearButton>
      {isDropdownShown && searchResults && searchResults.length ? (
        <DropdownPanel ref={dropdownRef}>
          <Dropdown>
            {searchResults.map((rs) => {
              return (
                <DropdownItem key={`${rs.id}${rs.feedType}`}>
                  <AnchorLink to={rs.url}>
                    {rs.pictureUrl ? (
                      <ImageRound src={rs.pictureUrl} alt="" />
                    ) : (
                      <EmptyImage />
                    )}
                    {rs.name}
                  </AnchorLink>
                </DropdownItem>
              );
            })}
          </Dropdown>
          <MoreSearchResultsFooter>
            <AnchorLink to={`/search/${keyword}`}>
              <FontAwesomeIcon className="me-1" icon="search" />
              Xem thêm kết quả của <strong>{keyword}</strong>
            </AnchorLink>
          </MoreSearchResultsFooter>
        </DropdownPanel>
      ) : null}
    </SearchForm>
  );
});
