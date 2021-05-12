import React, { Fragment, useContext, useEffect, useState } from "react";
import { withRouter } from "react-router-dom";
import FeedItem from "../../components/organisms/Feeds/FeedItem";
import { Pagination } from "../../components/organisms/Paging";
import { fileToBase64 } from "../../utils/Helper";
import { useMutation, useQuery } from "@apollo/client";
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
import Loading from "../../components/atoms/Loading";
import ErrorBlock from "../../components/atoms/ErrorBlock";
import { SessionContext } from "../../store/context/session-context";
import { useWindowSize } from "../../store/hook-store/window-size-store";
import { usePrevious } from "../../store/hook-store/use-previous-store";

export default withRouter((props) => {
  const { location, pageNumber, pageSize, match } = props;
  const { params } = match;
  const { userId } = params;
  const { currentUser, isLogin } = useContext(SessionContext);
  const [state, dispatch] = useStore(false);
  const [windowSize] = useWindowSize();

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
  const {
    loading,
    data,
    error,
    refetch: feedsRefetch,
    networkStatus,
  } = useQuery(feedqueries.GET_USER_FEEDS, {
    variables: {
      criterias: {
        userIdentityId: userId,
        page: pageNumber ? parseInt(pageNumber) : 1,
        pageSize: pageSize ? parseInt(pageSize) : 10,
      },
    },
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

  const [editorMode, setEditorMode] = useState("ARTICLE");
  const prevEditorMode = usePrevious(editorMode);
  const onToggleCreateMode = (name) => {
    setEditorMode(name);
  };

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

  useEffect(() => {
    if (windowSize.isResized && prevEditorMode !== editorMode) {
      setEditorMode(prevEditorMode);
    }
  }, [windowSize, editorMode, prevEditorMode]);

  useEffect(() => {
    if (state.store === "UPDATE" && state.id) {
      feedsRefetch();
    }
  }, [state, feedsRefetch]);

  if (loading || !data || networkStatus === 1) {
    return (
      <Fragment>
        {currentUser && isLogin ? renderProfileEditorTabs() : null}
        <Loading>Loading...</Loading>
      </Fragment>
    );
  } else if (error) {
    return (
      <Fragment>
        {currentUser && isLogin ? renderProfileEditorTabs() : null}
        <ErrorBlock>Error!</ErrorBlock>
      </Fragment>
    );
  }

  const { userFeeds } = data;
  const { totalPage, filter, collections } = userFeeds;
  const { page } = filter;
  const baseUrl = props.userUrl + "/feeds";
  const pageQuery = location.search;

  const feeds = collections.map((item) => {
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

  return (
    <Fragment>
      {currentUser && isLogin ? renderProfileEditorTabs() : null}

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
      <Pagination
        totalPage={totalPage}
        baseUrl={baseUrl}
        pageQuery={pageQuery}
        currentPage={page}
      />
    </Fragment>
  );
});
