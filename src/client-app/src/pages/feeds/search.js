import React from "react";
import { useQuery, useMutation } from "@apollo/client";
import { feedqueries } from "../../graphql/fetching/queries";
import { userMutations } from "../../graphql/fetching/mutations";
import { withRouter } from "react-router-dom";
import Loading from "../../components/atoms/Loading";
import ErrorBlock from "../../components/atoms/ErrorBlock";
import styled from "styled-components";
import { getParameters, generateQueryParameters } from "../../utils/Helper";
import SearchFeed from "../../components/templates/Feeds/search-feed";
import SearchForm from "../../components/templates/Feeds/search-form";
import { authClient } from "../../graphql/client";

const Wrapper = styled.div`
  > .row {
    margin-left: -12px;
    margin-right: -12px;
  }

  > .row > .col {
    padding: 0 12px;
  }
`;

export default withRouter((props) => {
  const { match, location, history } = props;
  const { params } = match;
  const { keyword } = params;
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
    return <Loading>Loading</Loading>;
  } else if (error) {
    return <ErrorBlock>Error!</ErrorBlock>;
  }

  const onSearch = (searchParams) => {
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
    history.push(pathName);
  };

  const { advancedSearch } = data;
  const baseUrl = `/search/`;
  return (
    <Wrapper className="container-lg container-fluid px-lg-5 mt-3">
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
    </Wrapper>
  );
});
