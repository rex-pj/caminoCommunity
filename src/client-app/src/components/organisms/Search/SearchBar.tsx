import * as React from "react";
import { useState, useEffect, useRef } from "react";
import { ApolloError, useLazyQuery } from "@apollo/client";
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
import { useParams, useNavigate } from "react-router-dom";
import { apiConfig } from "../../../config/api-config";
import { useTranslation } from "react-i18next";

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

interface ISearchData {
  searchResults?: any[];
  keyword?: string;
  isDropdownShown?: boolean;
  [index: string]: any;
}

type Props = {
  className?: string;
};

const SearchBar = (props: Props) => {
  const navigate = useNavigate();
  const { keyword: searchText } = useParams();
  const { t } = useTranslation();

  const [fetchResults] = useLazyQuery(feedqueries.LIVE_SEARCH, {
    onCompleted: (response) => onSearchCompleted(response),
    onError: (error) => onSearchError(error),
    fetchPolicy: "no-cache",
  });

  const [searchData, setSearchData] = useState<ISearchData>({
    searchResults: [],
    keyword: searchText,
    isDropdownShown: false,
  });
  const inputRef = useRef<any>({
    isSearching: false,
  });
  const dropdownRef = useRef<any>();

  const onSearchCompleted = (response: any) => {
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

  const parseSearchResults = (data: any[]) => {
    return data.map((rs: any) => {
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

      if (!result.pictureId) {
        return result;
      }

      if (result.feedType === FeedType.User) {
        result.pictureUrl = `${apiConfig.paths.userPhotos.get.getAvatar}/${result.pictureId}`;
      } else {
        result.pictureUrl = `${apiConfig.paths.pictures.get.getPicture}/${result.pictureId}`;
      }

      return result;
    });
  };

  const onSearchError = (error: ApolloError) => {
    inputRef.current.isSearching = false;
  };

  const onInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
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

  const onEnterExecuteSearch = (evt: React.KeyboardEvent<HTMLInputElement>) => {
    if (evt.keyCode === 13) {
      onRedirectToSearch();
    }
  };

  const onRedirectToSearch = () => {
    navigate(`/search/${searchData.keyword}`);
  };

  const onFetchResults = async (value: string) => {
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

  const onDropdownHide = (e: MouseEvent) => {
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
        placeholder={t("type_to_search").toString()}
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
};

export default SearchBar;
