import * as React from "react";
import { useQuery, useMutation } from "@apollo/client";
import { feedqueries } from "../../graphql/fetching/queries";
import { userMutations } from "../../graphql/fetching/mutations";
import { useLocation, useNavigate, useParams } from "react-router-dom";
import {
  ErrorBar,
  LoadingBar,
} from "../../components/molecules/NotificationBars";
import { getParameters, generateQueryParameters } from "../../utils/Helper";
import SearchFeed from "../../components/templates/Feeds/search-feed";
import SearchForm, {
  ISearchFormParams,
} from "../../components/templates/Feeds/search-form";
import { authClient } from "../../graphql/client";
import { MiddleRightLayout } from "../../components/templates/Layout";

interface Props {}

const Search = (props: Props) => {
  const navigate = useNavigate();
  const location = useLocation();
  const { keyword } = useParams();
  const timeOptions = [
    { value: 1, label: "Một tiếng trước" },
    { value: 24, label: "24 tiếng trước" },
    { value: 169, label: "Tuần trước" },
    { value: 720, label: "Tháng trước" },
    { value: 8760, label: "Năm trước" },
  ];

  const feedTypeDictonaries = {
    1: "Bài viết",
    2: "Sản phẩm",
    3: "Nông trại",
    4: "Nông hội",
    5: "Thành viên",
  };
  const {
    feedFilterType,
    userIdentityId,
    page,
    hoursCreatedFrom,
    hoursCreatedTo,
  } = getParameters(location.search);
  const { loading, data, error } = useQuery(feedqueries.ADVANCED_SEARCH, {
    variables: {
      criterias: {
        page: page ? parseInt(page) : 1,
        search: keyword,
        userIdentityId: userIdentityId,
        hoursCreatedFrom: hoursCreatedFrom ? parseInt(hoursCreatedFrom) : null,
        hoursCreatedTo: hoursCreatedFrom ? parseInt(hoursCreatedTo) : null,
        filterType: feedFilterType ? parseInt(feedFilterType) : null,
      },
    },
  });
  const [selectUsers] = useMutation(userMutations.GET_SELECT_USERS, {
    client: authClient,
  });

  if (loading || !data) {
    return <LoadingBar />;
  } else if (error) {
    return <ErrorBar />;
  }

  const onSearch = (searchParams: ISearchFormParams) => {
    const { userIdentityId, hoursCreatedFrom, hoursCreatedTo, keyword } =
      searchParams;
    let pathName = `${baseUrl}${keyword}`;
    const query = generateQueryParameters({
      userIdentityId,
      hoursCreatedFrom,
      hoursCreatedTo,
      feedFilterType: searchParams.feedFilterType,
    });

    if (query) {
      pathName += `?${query}`;
    }
    navigate(pathName);
  };

  const { advancedSearch } = data;
  const checkHasData = () => {
    return !!(
      advancedSearch &&
      (advancedSearch.farms ||
        advancedSearch.products ||
        advancedSearch.articles)
    );
  };

  const baseUrl = `/search/`;
  return (
    <MiddleRightLayout
      isLoading={!!loading}
      hasData={checkHasData()}
      hasError={!!error}
    >
      <div className="row px-lg-3">
        <div className="col col-12 col-sm-12 col-md-3 col-lg-3">
          <SearchForm
            timeOptions={timeOptions}
            feedTypeDictonaries={feedTypeDictonaries}
            advancedSearchResult={advancedSearch}
            selectUsers={selectUsers}
            baseUrl={baseUrl}
            searchParams={{
              hoursCreatedFrom,
              hoursCreatedTo,
              userIdentityId,
              feedFilterType,
            }}
            onSearch={onSearch}
          ></SearchForm>
        </div>
        <div className="col col-12 col-sm-12 col-md-9 col-lg-9">
          <SearchFeed
            keyword={keyword}
            advancedSearchResult={advancedSearch}
            baseUrl={baseUrl}
          ></SearchFeed>
        </div>
      </div>
    </MiddleRightLayout>
  );
};

export default Search;
