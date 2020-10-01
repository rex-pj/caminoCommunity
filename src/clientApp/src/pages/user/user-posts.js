import React, { Fragment } from "react";
import { withRouter } from "react-router-dom";
import { useQuery } from "@apollo/client";
import { UrlConstant } from "../../utils/Constants";
import { Pagination } from "../../components/molecules/Paging";
import ArticleListItem from "../../components/organisms/Article/ArticleListItem";
import { GET_USER_ARTICLES } from "../../utils/GraphQLQueries/queries";

export default withRouter(function (props) {
  const { location, match, pageNumber } = props;
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
  const { collections } = userArticles;
  const articles = collections.map((item) => {
    let article = { ...item };
    article.url = `${UrlConstant.Article.url}${article.id}`;
    if (article.thumbnailId) {
      article.thumbnailUrl = `${process.env.REACT_APP_CDN_PHOTO_URL}${article.thumbnailId}`;
    }

    article.creator = {
      createdDate: item.createdDate,
      profileUrl: `/profile/${item.createdByIdentityId}`,
      name: item.createdBy,
    };

    if (item.createdByPhotoCode) {
      article.creator.photoUrl = `${process.env.REACT_APP_CDN_AVATAR_API_URL}${item.createdByPhotoCode}`;
    }

    return article;
  });

  const pageQuery = location.search;
  const baseUrl = props.userUrl + "/posts";
  const { totalPage, filter } = userArticles;
  const { page } = filter;

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
        currentPage={page}
      />
    </Fragment>
  );
});
