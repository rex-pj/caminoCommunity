import * as React from "react";
import {
  Fragment,
  useState,
  useRef,
  useEffect,
  useContext,
  useMemo,
} from "react";
import { useParams } from "react-router-dom";
import { useMutation, useLazyQuery } from "@apollo/client";
import {
  articleMutations,
  farmMutations,
  productMutations,
} from "../../graphql/fetching/mutations";
import {
  farmQueries,
  feedqueries,
  productQueries,
} from "../../graphql/fetching/queries";
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
import { apiConfig } from "../../config/api-config";
import ArticleService from "../../services/articleService";
import FarmService from "../../services/farmService";
import ProductService from "../../services/productService";
import FeedItem from "../../components/organisms/Feeds/FeedItem";

interface Props {
  pageNumber?: number;
  editorMode: string;
  onToggleCreateMode: (editorMode: string) => void;
}

const UserFeeds = (props: Props) => {
  const { userId } = useParams();
  const { pageNumber } = props;
  const { editorMode, onToggleCreateMode } = props;
  const pageRef = useRef<any>({
    pageNumber: pageNumber ? pageNumber : 1,
    userId: userId,
  });
  const { currentUser, isLogin } = useContext(SessionContext);
  const [state, dispatch] = useStore(false);
  const [feeds, setFeeds] = useState<any[]>([]);
  const articleService = new ArticleService();
  const farmService = new FarmService();
  const productService = new ProductService();

  // Mutations
  const [articleCategories] = useMutation(
    articleMutations.FILTER_ARTICLE_CATEGORIES
  );
  const [fetchProductCategories] = useLazyQuery(
    productQueries.FILTER_PRODUCT_CATEGORIES,
    {
      variables: {},
    }
  );
  const [productAttributes] = useMutation(
    productMutations.FILTER_PRODUCT_ATTRIBUTES
  );
  const [productAttributeControlTypes] = useMutation(
    productMutations.FILTER_PRODUCT_ATTRIBUTE_CONTROL_TYPES
  );
  const [farmTypes] = useMutation(farmMutations.FILTER_FARM_TYPES);
  const [fetchUserFarms] = useLazyQuery(farmQueries.SELECT_USER_FARMS, {
    variables: {},
  });
  const getUserFarms = useMemo(() => fetchUserFarms, [fetchUserFarms]);

  const getProductCategories = useMemo(
    () => fetchProductCategories,
    [fetchProductCategories]
  );
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

  const onArticlePost = async (data: any) => {
    return await articleService.create(data).then((response) => {
      const { data: id } = response;
      resetFeeds();

      return Promise.resolve(id);
    });
  };

  const onProductPost = async (data: any) => {
    return await productService.create(data).then((response) => {
      const { data: id } = response;
      resetFeeds();
      return Promise.resolve(id);
    });
  };

  const onFarmPost = async (data: any) => {
    return await farmService.create(data).then((response) => {
      const { data: id } = response;
      resetFeeds();
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

  function onOpenDeleteConfirmation(e: any, onDelete: any) {
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
  }

  const onDeleteArticle = async (id: number) => {
    await articleService.delete(id);
    resetFeeds();
  };

  const onDeleteFarm = async (id: number) => {
    return farmService.delete(id).then(() => {
      resetFeeds();
    });
  };

  const onDeleteProduct = (id: number) => {
    return productService.delete(id).then(() => {
      resetFeeds();
    });
  };

  const resetFeeds = () => {
    setFeeds([]);
    fetchFeeds({
      variables: {
        criterias: {
          userIdentityId: userId,
          page: 1,
        },
      },
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
      searchArticleCategories={articleCategories}
      onArticlePost={onArticlePost}
      showValidationError={showValidationError}
      searchProductCategories={getProductCategories}
      searchProductAttributes={productAttributes}
      searchProductAttributeControlTypes={productAttributeControlTypes}
      onProductPost={onProductPost}
      searchFarms={getUserFarms}
      searchFarmTypes={farmTypes}
      onFarmPost={onFarmPost}
      editorMode={editorMode}
      onToggleCreateMode={onToggleCreateMode}
    ></ProfileEditorTabs>
  );

  const onFetchCompleted = (data: any) => {
    const {
      userFeeds: { collections },
    } = data;
    const feedCollections = parseCollections(collections);
    setFeeds([...feeds, ...feedCollections]);
  };

  const setPageInfo = (data: any) => {
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

  const parseCollections = (collections: any[]): any[] => {
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
        feed.pictureUrl = `${apiConfig.paths.pictures.get.getPicture}/${feed.pictureId}`;
      }

      feed.creator = {
        createdDate: item.createdDate,
        profileUrl: `/profile/${item.createdByIdentityId}`,
        name: item.createdByName,
      };

      if (item.createdByPhotoId) {
        feed.creator.photoUrl = `${apiConfig.paths.userPhotos.get.getAvatar}/${item.createdByPhotoId}`;
      }

      return feed;
    });
  };

  if ((loading || networkStatus === 1) && feeds.length === 0) {
    return (
      <Fragment>
        {currentUser && isLogin ? renderProfileEditorTabs() : null}
        <LoadingBar />
      </Fragment>
    );
  }

  if (!(data && pageRef.current.totalResult && feeds.length > 0)) {
    return (
      <Fragment>
        {currentUser && isLogin ? renderProfileEditorTabs() : null}
        <NoDataBar />
      </Fragment>
    );
  }
  if (error) {
    return (
      <Fragment>
        {currentUser && isLogin ? renderProfileEditorTabs() : null}
        <ErrorBar />
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
        dataLength={pageRef.current.totalResult ?? 0}
        next={fetchMoreData}
        hasMore={pageRef.current.currentPage < pageRef.current.totalPage}
        loader={<LoadingBar />}
      >
        {feeds
          ? feeds.map((item: any) => {
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

export default UserFeeds;
