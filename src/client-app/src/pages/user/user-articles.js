import React, {
  Fragment,
  useContext,
  useState,
  useRef,
  useEffect,
} from "react";
import { withRouter } from "react-router-dom";
import { useLazyQuery, useMutation } from "@apollo/client";
import { UrlConstant } from "../../utils/Constants";
import { articleQueries } from "../../graphql/fetching/queries";
import {
  articleMutations,
  mediaMutations,
} from "../../graphql/fetching/mutations";
import {
  ErrorBar,
  LoadingBar,
  NoDataBar,
} from "../../components/molecules/NotificationBars";
import { useStore } from "../../store/hook-store";
import { fileToBase64 } from "../../utils/Helper";
import authClient from "../../graphql/client/authClient";
import ArticleEditor from "../../components/organisms/Article/ArticleEditor";
import { SessionContext } from "../../store/context/session-context";
import ArticleListItem from "../../components/organisms/Article/ArticleListItem";
import InfiniteScroll from "react-infinite-scroll-component";

export default withRouter(function (props) {
  const {
    match: {
      params: { userId },
    },
    pageNumber,
  } = props;
  const [state, dispatch] = useStore(false);
  const { currentUser, isLogin } = useContext(SessionContext);

  const [articleCategories] = useMutation(
    articleMutations.FILTER_ARTICLE_CATEGORIES
  );
  const [validateImageUrl] = useMutation(mediaMutations.VALIDATE_IMAGE_URL);

  const [articles, setArticles] = useState([]);
  const pageRef = useRef({
    pageNumber: pageNumber ? pageNumber : 1,
    userId: userId,
  });
  const [
    fetchArticles,
    { loading, data, error, refetch: refetchArticles, networkStatus },
  ] = useLazyQuery(articleQueries.GET_USER_ARTICLES, {
    onCompleted: (data) => {
      setPageInfo(data);
      onFetchCompleted(data);
    },
    fetchPolicy: "cache-and-network",
  });

  const [createArticle] = useMutation(articleMutations.CREATE_ARTICLE, {
    client: authClient,
  });

  const [deleteArticle] = useMutation(articleMutations.DELETE_ARTICLE, {
    client: authClient,
  });

  const onArticlePost = async (data) => {
    return await createArticle({
      variables: {
        criterias: data,
      },
    }).then((response) => {
      return new Promise((resolve, reject) => {
        const { data } = response;
        const { createArticle: article } = data;
        resolve(article);
        refetchArticles();
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
      refetchArticles();
    });
  };

  const renderArticleEditor = () => {
    if (currentUser && isLogin) {
      return (
        <ArticleEditor
          height={230}
          convertImageCallback={convertImagefile}
          onImageValidate={onImageValidate}
          filterCategories={articleCategories}
          onArticlePost={onArticlePost}
          refetchNews={refetchArticles}
          showValidationError={showValidationError}
        />
      );
    }
    return null;
  };

  const setPageInfo = (data) => {
    const {
      userArticles: {
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
      userArticles: { collections },
    } = data;
    const articleCollections = parseCollections(collections);
    setArticles([...articles, ...articleCollections]);
  };

  useEffect(() => {
    if (state.type === "ARTICLE_UPDATE" || state.type === "ARTICLE_DELETE") {
      refetchArticles();
    }
  }, [state, refetchArticles]);

  useEffect(() => {
    const page = pageRef.current.pageNumber;
    fetchArticles({
      variables: {
        criterias: {
          userIdentityId: pageRef.current.userId,
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
  };

  if ((loading || networkStatus === 1) && articles.length === 0) {
    return (
      <Fragment>
        {currentUser && isLogin ? renderArticleEditor() : null}
        <LoadingBar>Loading...</LoadingBar>
      </Fragment>
    );
  }
  if ((!data || !pageRef.current.totalResult) && articles.length === 0) {
    return (
      <Fragment>
        {currentUser && isLogin ? renderArticleEditor() : null}
        <NoDataBar>No Data!</NoDataBar>
      </Fragment>
    );
  }
  if (error) {
    return (
      <Fragment>
        {currentUser && isLogin ? renderArticleEditor() : null}
        <ErrorBar>Error!</ErrorBar>
      </Fragment>
    );
  }

  const fetchMoreData = () => {
    if (pageRef.current.pageNumber === pageRef.current.totalPage) {
      return;
    }
    pageRef.current.pageNumber += 1;
    fetchArticles({
      variables: {
        criterias: {
          userId: pageRef.current.userId,
          page: pageRef.current.pageNumber,
        },
      },
    });
  };

  return (
    <Fragment>
      {renderArticleEditor()}
      <InfiniteScroll
        style={{ overflowX: "hidden" }}
        dataLength={pageRef.current.totalResult}
        next={fetchMoreData}
        hasMore={pageRef.current.currentPage < pageRef.current.totalPage}
        loader={<h4>Loading...</h4>}
      >
        {articles
          ? articles.map((item) => (
              <ArticleListItem
                key={item.id}
                article={item}
                convertImageCallback={convertImagefile}
                onImageValidate={onImageValidate}
                filterCategories={articleCategories}
                refetchNews={refetchArticles}
                showValidationError={showValidationError}
                onOpenDeleteConfirmationModal={onOpenDeleteConfirmation}
              />
            ))
          : null}
      </InfiniteScroll>
    </Fragment>
  );
});
