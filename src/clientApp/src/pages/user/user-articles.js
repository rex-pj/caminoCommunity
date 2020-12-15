import React, { Fragment, useContext, useEffect } from "react";
import { withRouter } from "react-router-dom";
import { useQuery, useMutation } from "@apollo/client";
import { UrlConstant } from "../../utils/Constants";
import { Pagination } from "../../components/organisms/Paging";
import { GET_USER_ARTICLES } from "../../utils/GraphQLQueries/queries";
import {
  VALIDATE_IMAGE_URL,
  CREATE_ARTICLE,
  FILTER_ARTICLE_CATEGORIES,
} from "../../utils/GraphQLQueries/mutations";
import Loading from "../../components/atoms/Loading";
import ErrorBlock from "../../components/atoms/ErrorBlock";
import { useStore } from "../../store/hook-store";
import { fileToBase64 } from "../../utils/Helper";
import graphqlClient from "../../utils/GraphQLClient/graphqlClient";
import ArticleEditor from "../../components/organisms/ProfileEditors/ArticleEditor";
import { SessionContext } from "../../store/context/SessionContext";
import ArticleListItemThumbnail from "../../components/organisms/Article/ArticleListItemThumbnail";

export default withRouter(function (props) {
  const { location, match, pageNumber, pageSize } = props;
  const { params } = match;
  const { userId } = params;
  const [state, dispatch] = useStore(false);
  const { user } = useContext(SessionContext);

  const [articleCategories] = useMutation(FILTER_ARTICLE_CATEGORIES);
  const [validateImageUrl] = useMutation(VALIDATE_IMAGE_URL);
  const {
    loading,
    data,
    error,
    refetch: fetchArticles,
    networkStatus,
  } = useQuery(GET_USER_ARTICLES, {
    variables: {
      criterias: {
        userIdentityId: userId,
        page: pageNumber ? parseInt(pageNumber) : 1,
        pageSize: pageSize ? parseInt(pageSize) : 10,
      },
    },
  });

  const [createArticle] = useMutation(CREATE_ARTICLE, {
    client: graphqlClient,
  });

  const searchArticleCategories = async (inputValue) => {
    return await articleCategories({
      variables: {
        criterias: { query: inputValue },
      },
    })
      .then((response) => {
        var { data } = response;
        var { categories } = data;
        if (!categories) {
          return [];
        }
        return categories.map((cat) => {
          return {
            value: cat.id,
            label: cat.text,
          };
        });
      })
      .catch((error) => {
        console.log(error);
        return [];
      });
  };

  const onArticlePost = async (data) => {
    return await createArticle({
      variables: {
        criterias: data,
      },
    }).then((response) => {
      return new Promise((resolve, reject) => {
        const { data } = response;
        const { createArticle: article } = data;
        fetchArticles();
        resolve({ article });
      });
    });
  };

  const showValidationError = (title, message) => {
    dispatch("NOTIFY", {
      title,
      message,
      type: "error",
    });
  };

  const onImageValidate = async (value) => {
    return await validateImageUrl({
      variables: {
        criterias: {
          url: value,
        },
      },
    });
  };

  const convertImagefile = async (file) => {
    const url = await fileToBase64(file);
    return {
      url,
      fileName: file.name,
    };
  };

  const articleEditor =
    user && user.isLogin ? (
      <ArticleEditor
        height={230}
        convertImageCallback={convertImagefile}
        onImageValidate={onImageValidate}
        filterCategories={searchArticleCategories}
        onArticlePost={onArticlePost}
        refetchNews={fetchArticles}
        showValidationError={showValidationError}
      />
    ) : null;

  useEffect(() => {
    if (state.type === "ARTICLE" && state.id) {
      fetchArticles();
    }
  }, [state, fetchArticles]);

  if (loading || !data || networkStatus === 1) {
    return (
      <Fragment>
        {articleEditor}
        <Loading>Loading...</Loading>
      </Fragment>
    );
  } else if (error) {
    return (
      <Fragment>
        {articleEditor}
        <ErrorBlock>Error!</ErrorBlock>
      </Fragment>
    );
  }

  const { userArticles } = data;
  const { collections } = userArticles;
  const articles = collections.map((item) => {
    let article = { ...item };
    article.url = `${UrlConstant.Article.url}${article.id}`;
    if (article.thumbnail.pictureId) {
      article.thumbnailUrl = `${process.env.REACT_APP_CDN_PHOTO_URL}${article.thumbnail.pictureId}`;
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
  const baseUrl = props.userUrl + "/articles";
  const { totalPage, filter } = userArticles;
  const { page } = filter;

  return (
    <Fragment>
      {articleEditor}
      {articles
        ? articles.map((item) => (
            <ArticleListItemThumbnail
              key={item.id}
              article={item}
              convertImageCallback={convertImagefile}
              onImageValidate={onImageValidate}
              filterCategories={searchArticleCategories}
              refetchNews={fetchArticles}
              showValidationError={showValidationError}
            />
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
