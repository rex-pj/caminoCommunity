import * as React from "react";
import { Fragment, useContext, useState, useRef, useEffect } from "react";
import { useParams } from "react-router-dom";
import { useLazyQuery, useMutation } from "@apollo/client";
import { UrlConstant } from "../../utils/Constants";
import { articleQueries } from "../../graphql/fetching/queries";
import { articleMutations } from "../../graphql/fetching/mutations";
import {
  ErrorBar,
  LoadingBar,
  NoDataBar,
} from "../../components/molecules/NotificationBars";
import { useStore } from "../../store/hook-store";
import ArticleEditor from "../../components/organisms/Article/ArticleEditor";
import { SessionContext } from "../../store/context/session-context";
import ArticleListItem from "../../components/organisms/Article/ArticleListItem";
import InfiniteScroll from "react-infinite-scroll-component";
import { apiConfig } from "../../config/api-config";
import ArticleService from "../../services/articleService";

interface Props {
  pageNumber?: number;
}

const UserArticles = (props: Props) => {
  const { userId } = useParams();
  const { pageNumber } = props;
  const [state, dispatch] = useStore(false);
  const { currentUser, isLogin } = useContext(SessionContext);
  const articleService = new ArticleService();

  const [articleCategories] = useMutation(
    articleMutations.FILTER_ARTICLE_CATEGORIES
  );

  const [articles, setArticles] = useState<any[]>([]);
  const pageRef = useRef<any>({
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

  const onArticlePost = async (data: any) => {
    return await articleService.create(data).then((response) => {
      const { data: id } = response;
      resetArticles();

      return Promise.resolve(id);
    });
  };

  const showValidationError = (title: string, message: string) => {
    dispatch("NOTIFY", {
      title,
      message,
      type: "error",
    });
  };

  const onOpenDeleteConfirmation = (e: any) => {
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

  const onDelete = async (id: number) => {
    await articleService.delete(id).then(() => {
      resetArticles();
    });
  };

  const resetArticles = () => {
    setArticles([]);
    fetchArticles({
      variables: {
        criterias: {
          userIdentityId: pageRef.current.userId,
          page: 1,
        },
      },
    });
  };

  const renderArticleEditor = () => {
    if (currentUser && isLogin) {
      return (
        <ArticleEditor
          height={230}
          filterCategories={articleCategories}
          onArticlePost={onArticlePost}
          showValidationError={showValidationError}
        />
      );
    }
    return null;
  };

  const setPageInfo = (data: any) => {
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

  const onFetchCompleted = (data: any) => {
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

  const parseCollections = (collections: any[]): any[] => {
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

  if ((loading || networkStatus === 1) && articles.length === 0) {
    return (
      <Fragment>
        {currentUser && isLogin ? renderArticleEditor() : null}
        <LoadingBar />
      </Fragment>
    );
  }
  if (!(data && pageRef.current.totalResult && articles.length > 0)) {
    return (
      <Fragment>
        {currentUser && isLogin ? renderArticleEditor() : null}
        <NoDataBar />
      </Fragment>
    );
  }
  if (error) {
    return (
      <Fragment>
        {currentUser && isLogin ? renderArticleEditor() : null}
        <ErrorBar />
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
        dataLength={pageRef.current.totalResult ?? 0}
        next={fetchMoreData}
        hasMore={pageRef.current.currentPage < pageRef.current.totalPage}
        loader={<LoadingBar />}
      >
        {articles
          ? articles.map((item) => (
              <ArticleListItem
                key={item.id}
                article={item}
                onOpenDeleteConfirmationModal={onOpenDeleteConfirmation}
              />
            ))
          : null}
      </InfiniteScroll>
    </Fragment>
  );
};

export default UserArticles;
