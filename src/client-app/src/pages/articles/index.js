import React, { useEffect, useState, useRef } from "react";
import { DefaultLayout } from "../../components/templates/Layout";
import Article from "../../components/templates/Article";
import { UrlConstant } from "../../utils/Constants";
import { useParams } from "react-router-dom";
import { useLazyQuery, useMutation } from "@apollo/client";
import { articleQueries } from "../../graphql/fetching/queries";
import { articleMutations } from "../../graphql/fetching/mutations";
import { useStore } from "../../store/hook-store";
import { authClient } from "../../graphql/client";
import Breadcrumb from "../../components/organisms/Navigation/Breadcrumb";
import InfiniteScroll from "react-infinite-scroll-component";
import { apiConfig } from "../../config/api-config";
import { LoadingBar } from "../../components/molecules/NotificationBars";

export default (function (props) {
  const { pageNumber } = useParams();
  const [state, dispatch] = useStore(false);
  const [articles, setArticles] = useState([]);
  const pageRef = useRef({ pageNumber: pageNumber ? pageNumber : 1 });
  const [fetchArticles, { loading, data, error, refetch }] = useLazyQuery(
    articleQueries.GET_ARTICLES,
    {
      onCompleted: (data) => {
        setPageInfo(data);
        onFetchCompleted(data);
      },
      fetchPolicy: "cache-and-network",
    }
  );

  const [deleteArticle] = useMutation(articleMutations.DELETE_ARTICLE, {
    client: authClient,
  });

  const setPageInfo = (data) => {
    const {
      articles: {
        totalPage,
        totalResult,
        filter: { page },
      },
    } = data;
    pageRef.current.totalPage = totalPage;
    pageRef.current.currentPage = page;
    pageRef.current.totalResult = totalResult;
  };

  const onFetchCompleted = (data) => {
    const {
      articles: { collections },
    } = data;
    const articleCollections = parseCollections(collections);
    setArticles([...articles, ...articleCollections]);
  };

  useEffect(() => {
    if (state.type === "ARTICLE_UPDATE" || state.type === "ARTICLE_DELETE") {
      refetch();
    }
  }, [state, refetch]);

  useEffect(() => {
    const page = pageRef.current.pageNumber;
    fetchArticles({
      variables: {
        criterias: {
          page: page ? parseInt(page) : 1,
        },
      },
    });
  }, [fetchArticles]);

  const parseCollections = (collections) => {
    return collections.map((item) => {
      let article = { ...item };
      article.url = `${UrlConstant.Article.url}${article.id}`;
      if (article.picture.pictureId) {
        article.pictureUrl = `${apiConfig.paths.pictures.get.getPicture}/${article.picture.pictureId}`;
      }

      article.creator = {
        createdDate: item.createdDate,
        profileUrl: `/profile/${item.createdByIdentityId}`,
        name: item.createdBy,
      };

      if (item.createdByPhotoId) {
        article.creator.photoUrl = `${apiConfig.paths.userPhotos.get.getAvatar}/${item.createdByPhotoId}`;
      }

      return article;
    });
  };

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

  const fetchMoreData = () => {
    if (pageRef.current.pageNumber === pageRef.current.totalPage) {
      return;
    }
    pageRef.current.pageNumber += 1;
    fetchArticles({
      variables: {
        criterias: {
          page: pageRef.current.pageNumber,
        },
      },
    });
  };

  const checkHasData = () => {
    return data && pageRef.current.totalResult && articles.length >= 0;
  };

  return (
    <DefaultLayout
      isLoading={!!loading}
      hasData={checkHasData()}
      hasError={!!error}
    >
      <Breadcrumb list={breadcrumbs} className="px-2" />
      <InfiniteScroll
        style={{ overflowX: "hidden" }}
        dataLength={pageRef.current.totalResult ?? 0}
        next={fetchMoreData}
        hasMore={pageRef.current.currentPage < pageRef.current.totalPage}
        loader={<LoadingBar />}
      >
        <Article
          onOpenDeleteConfirmation={onOpenDeleteConfirmation}
          articles={articles}
          breadcrumbs={breadcrumbs}
          baseUrl="/articles"
        />
      </InfiniteScroll>
    </DefaultLayout>
  );
});
