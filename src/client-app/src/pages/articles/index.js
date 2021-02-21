import React, { useEffect } from "react";
import { DefaultLayout } from "../../components/templates/Layout";
import Article from "../../components/templates/Article";
import { UrlConstant } from "../../utils/Constants";
import { withRouter } from "react-router-dom";
import { useQuery, useMutation } from "@apollo/client";
import { articleQueries } from "../../graphql/fetching/queries";
import { articleMutations } from "../../graphql/fetching/mutations";
import Loading from "../../components/atoms/Loading";
import ErrorBlock from "../../components/atoms/ErrorBlock";
import { useStore } from "../../store/hook-store";
import { authClient } from "../../graphql/client";

export default withRouter(function (props) {
  const { match } = props;
  const { params } = match;
  const { pageNumber, pageSize } = params;
  const [state, dispatch] = useStore(false);
  const { loading, data, error, refetch } = useQuery(
    articleQueries.GET_ARTICLES,
    {
      variables: {
        criterias: {
          page: pageNumber ? parseInt(pageNumber) : 1,
          pageSize: pageSize ? parseInt(pageSize) : 10,
        },
      },
    }
  );

  const [deleteArticle] = useMutation(articleMutations.DELETE_ARTICLE, {
    client: authClient,
  });

  useEffect(() => {
    if (state.type === "ARTICLE_UPDATE" || state.type === "ARTICLE_DELETE") {
      refetch();
    }
  }, [state, refetch]);

  if (loading || !data) {
    return <Loading>Loading</Loading>;
  } else if (error) {
    return <ErrorBlock>Error!</ErrorBlock>;
  }

  const { articles: articlesResponse } = data;
  const { collections } = articlesResponse;
  const articles = collections.map((item) => {
    let article = { ...item };
    article.url = `${UrlConstant.Article.url}${article.id}`;
    if (article.picture.pictureId) {
      article.pictureUrl = `${process.env.REACT_APP_CDN_PHOTO_URL}${article.picture.pictureId}`;
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

  const baseUrl = "/articles";
  const { totalPage, filter } = articlesResponse;
  const { page } = filter;

  const breadcrumbs = [
    {
      isActived: true,
      title: "Article",
    },
  ];

  const onOpenDeleteConfirmation = (e) => {
    const { title, innerModal, message, id } = e;
    dispatch("OPEN_MODAL", {
      data: {
        title: title,
        children: message,
        id: id,
      },
      execution: { onDelete: onDelete },
      options: {
        isOpen: true,
        innerModal: innerModal,
        position: "fixed",
      },
    });
  };

  const onDelete = (id) => {
    deleteArticle({
      variables: {
        criterias: { id },
      },
    }).then(() => {
      refetch();
    });
  };

  return (
    <DefaultLayout>
      <Article
        onOpenDeleteConfirmation={onOpenDeleteConfirmation}
        articles={articles}
        breadcrumbs={breadcrumbs}
        totalPage={totalPage}
        baseUrl={baseUrl}
        currentPage={page}
      />
    </DefaultLayout>
  );
});
