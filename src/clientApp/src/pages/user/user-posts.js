import React, { Fragment } from "react";
import { withRouter } from "react-router-dom";
import { useQuery } from "@apollo/client";
// import { UrlConstant } from "../../utils/Constants";
import { Pagination } from "../../components/molecules/Paging";
import ArticleListItem from "../../components/organisms/Article/ArticleListItem";
import { GET_USER_ARTICLES } from "../../utils/GraphQLQueries/queries";

export default withRouter(function (props) {
  const { location, pageNumber } = props;
  const { match } = props;
  const { params } = match;
  const { userId } = params;

  const { loading, data } = useQuery(GET_USER_ARTICLES, {
    variables: {
      criterias: {
        userIdentityId: userId,
        page: pageNumber,
      },
    },
  });

  if (loading) {
    return <div></div>;
  }

  const { userArticles } = data;
  const { collections: articles } = userArticles;

  var state = {
    totalPage: 10,
    pageQuery: location.search,
    baseUrl: props.userUrl + "/posts",
    currentPage: pageNumber ? pageNumber : 1,
  };

  const { totalPage, baseUrl, currentPage, pageQuery } = state;

  return (
    <Fragment>
      {articles
        ? articles.map((item) => (
            <ArticleListItem key={item.id} article={item} />
          ))
        : null}
      <Pagination
        totalPage={totalPage}
        baseUrl={baseUrl}
        pageQuery={pageQuery}
        currentPage={currentPage}
      />
    </Fragment>
  );
});
