import React, {
  Fragment,
  useContext,
  useEffect,
  useState,
  useRef,
} from "react";
import { useLocation, useNavigate } from "react-router-dom";
import FeedItem from "../../components/organisms/Feeds/FeedItem";
import { fileToBase64 } from "../../utils/Helper";
import { useMutation, useLazyQuery } from "@apollo/client";
import authClient from "../../graphql/client/authClient";
import {
  articleMutations,
  farmMutations,
  productMutations,
  mediaMutations,
} from "../../graphql/fetching/mutations";
import { feedqueries } from "../../graphql/fetching/queries";
import ProfileEditorTabs from "../../components/organisms/Profile/ProfileEditorTabs";
import { useStore } from "../../store/hook-store";
import { UrlConstant } from "../../utils/Constants";
import { FeedType } from "../../utils/Enums";
import {
  ErrorBar,
  LoadingBar,
  NoDataBar,
} from "../../components/molecules/NotificationBars";
import { SessionContext } from "../../store/context/session-context";
import InfiniteScroll from "react-infinite-scroll-component";

export default (props) => {
  const {
    pageNumber,
    match: {
      params: { userId },
    },
    editorMode,
    onToggleCreateMode,
  } = props;
  const pageRef = useRef({
    pageNumber: pageNumber ? pageNumber : 1,
    userId: userId,
  });
  const { currentUser, isLogin } = useContext(SessionContext);
  const [state, dispatch] = useStore(false);
  const [feeds, setFeeds] = useState([]);

  // Mutations
  const [validateImageUrl] = useMutation(mediaMutations.VALIDATE_IMAGE_URL);
  const [articleCategories] = useMutation(
    articleMutations.FILTER_ARTICLE_CATEGORIES
  );
  const [productCategories] = useMutation(
    productMutations.FILTER_PRODUCT_CATEGORIES
  );
  const [productAttributes] = useMutation(
    productMutations.FILTER_PRODUCT_ATTRIBUTES
  );
  const [productAttributeControlTypes] = useMutation(
    productMutations.FILTER_PRODUCT_ATTRIBUTE_CONTROL_TYPES
  );
  const [farmTypes] = useMutation(farmMutations.FILTER_FARM_TYPES);
  const [userFarms] = useMutation(farmMutations.FILTER_FARMS);
  const [createArticle] = useMutation(articleMutations.CREATE_ARTICLE, {
    client: authClient,
  });
  const [createProduct] = useMutation(productMutations.CREATE_PRODUCT, {
    client: authClient,
  });
  const [createFarm] = useMutation(farmMutations.CREATE_FARM, {
    client: authClient,
  });
  const [deleteArticle] = useMutation(articleMutations.DELETE_ARTICLE, {
    client: authClient,
  });
  const [deleteFarm] = useMutation(farmMutations.DELETE_FARM, {
    client: authClient,
  });
  const [deleteProduct] = useMutation(productMutations.DELETE_PRODUCT, {
    client: authClient,
  });

  // Queries
  const [
    fetchFeeds,
    { loading, data, error, refetch: feedsRefetch, networkStatus },
  ] = useLazyQuery(feedqueries.GET_USER_FEEDS, {
    onCompleted: (data) => {
      setPageInfo(data);
      onFetchCompleted(data);
    },
    fetchPolicy: "cache-and-network",
  });

  const convertImagefile = async (file) => {
    const url = await fileToBase64(file);
    return {
      url,
      fileName: file.name,
    };
  };

  const onImageValidate = async (value) =>
    await validateImageUrl({
      variables: {
        criterias: {
          url: value,
        },
      },
    });

  const onArticlePost = async (data) => {
    return await createArticle({
      variables: {
        criterias: data,
      },
    }).then((response) => {
      return new Promise((resolve) => {
        const { data } = response;
        const { createArticle: article } = data;
        resolve(article);
        feedsRefetch();
      });
    });
  };

  const onProductPost = async (data) => {
    return await createProduct({
      variables: {
        criterias: data,
      },
    }).then((response) => {
      return new Promise((resolve) => {
        const { data } = response;
        const { createProduct: product } = data;
        resolve(product);
        feedsRefetch();
      });
    });
  };

  const onFarmPost = async (data) => {
    return await createFarm({
      variables: {
        criterias: data,
      },
    }).then((response) => {
      return new Promise((resolve) => {
        const { data } = response;
        const { createFarm: farm } = data;
        resolve(farm);
        feedsRefetch();
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

  const onOpenDeleteConfirmation = (e, onDelete) => {
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

  const onDeleteArticle = (id) => {
    deleteArticle({
      variables: {
        criterias: { id },
      },
    }).then(() => {
      feedsRefetch();
    });
  };

  const onDeleteFarm = (id) => {
    deleteFarm({
      variables: {
        criterias: { id },
      },
    }).then(() => {
      feedsRefetch();
    });
  };

  const onDeleteProduct = (id) => {
    deleteProduct({
      variables: {
        criterias: { id },
      },
    }).then(() => {
      feedsRefetch();
    });
  };

  useEffect(() => {
    if (state.store === "UPDATE" && state.id) {
      feedsRefetch();
    }
  }, [state, feedsRefetch]);

  useEffect(() => {
    const page = pageRef.current.pageNumber;
    const userId = pageRef.current.userId;
    fetchFeeds({
      variables: {
        criterias: {
          userIdentityId: userId,
          page: page ? parseInt(page) : 1,
        },
      },
    });
  }, [fetchFeeds]);

  const renderProfileEditorTabs = () => (
    <ProfileEditorTabs
      convertImagefile={convertImagefile}
      onImageValidate={onImageValidate}
      searchArticleCategories={articleCategories}
      onArticlePost={onArticlePost}
      refetchNewsFeed={feedsRefetch}
      showValidationError={showValidationError}
      searchProductCategories={productCategories}
      searchProductAttributes={productAttributes}
      searchProductAttributeControlTypes={productAttributeControlTypes}
      onProductPost={onProductPost}
      searchFarms={userFarms}
      searchFarmTypes={farmTypes}
      onFarmPost={onFarmPost}
      editorMode={editorMode}
      onToggleCreateMode={onToggleCreateMode}
    ></ProfileEditorTabs>
  );

  const onFetchCompleted = (data) => {
    const {
      userFeeds: { collections },
    } = data;
    const feedCollections = parseCollections(collections);
    setFeeds([...feeds, ...feedCollections]);
  };

  const setPageInfo = (data) => {
    const {
      userFeeds: {
        totalPage,
        totalResult,
        filter: { page },
      },
    } = data;
    pageRef.current.totalPage = totalPage;
    pageRef.current.currentPage = page;
    pageRef.current.totalResult = totalResult;
  };

  const parseCollections = (collections) => {
    return collections.map((item) => {
      let feed = { ...item };
      if (feed.feedType === FeedType.Farm) {
        feed.url = `${UrlConstant.Farm.url}${feed.id}`;
      } else if (feed.feedType === FeedType.Article) {
        feed.url = `${UrlConstant.Article.url}${feed.id}`;
      } else if (feed.feedType === FeedType.Product) {
        feed.url = `${UrlConstant.Product.url}${feed.id}`;
      }

      if (feed.pictureId > 0) {
        feed.pictureUrl = `${process.env.REACT_APP_CDN_PHOTO_URL}${feed.pictureId}`;
      }

      feed.creator = {
        createdDate: item.createdDate,
        profileUrl: `/profile/${item.createdByIdentityId}`,
        name: item.createdByName,
      };

      if (item.createdByPhotoCode) {
        feed.creator.photoUrl = `${process.env.REACT_APP_CDN_AVATAR_API_URL}${item.createdByPhotoCode}`;
      }

      return feed;
    });
  };

  if ((loading || networkStatus === 1) && feeds.length === 0) {
    return (
      <Fragment>
        {currentUser && isLogin ? renderProfileEditorTabs() : null}
        <LoadingBar>Loading...</LoadingBar>
      </Fragment>
    );
  }
  if ((!data || !pageRef.current.totalResult) && feeds.length === 0) {
    return (
      <Fragment>
        {currentUser && isLogin ? renderProfileEditorTabs() : null}
        <NoDataBar>No Data!</NoDataBar>
      </Fragment>
    );
  }
  if (error) {
    return (
      <Fragment>
        {currentUser && isLogin ? renderProfileEditorTabs() : null}
        <ErrorBar>Error!</ErrorBar>
      </Fragment>
    );
  }

  const fetchMoreData = () => {
    if (pageRef.current.pageNumber === pageRef.current.totalPage) {
      return;
    }
    pageRef.current.pageNumber += 1;
    fetchFeeds({
      variables: {
        criterias: {
          userIdentityId: userId,
          page: pageRef.current.pageNumber,
        },
      },
    });
  };

  return (
    <Fragment>
      {currentUser && isLogin ? renderProfileEditorTabs() : null}
      <InfiniteScroll
        dataLength={pageRef.current.totalResult}
        next={fetchMoreData}
        hasMore={pageRef.current.currentPage < pageRef.current.totalPage}
        loader={<h4>Loading...</h4>}
      >
        {feeds
          ? feeds.map((item) => {
              const key = `${item.feedType}_${item.id}`;
              return (
                <FeedItem
                  key={key}
                  feed={item}
                  onOpenDeleteConfirmation={onOpenDeleteConfirmation}
                  onDeleteArticle={onDeleteArticle}
                  onDeleteFarm={onDeleteFarm}
                  onDeleteProduct={onDeleteProduct}
                />
              );
            })
          : null}
      </InfiniteScroll>
    </Fragment>
  );
};
