import React, { useEffect, useState, useRef } from "react";
import { DefaultLayout } from "../../components/templates/Layout";
import { withRouter } from "react-router-dom";
import Feeds from "../../components/templates/Feeds";
import { FeedType } from "../../utils/Enums";
import { UrlConstant } from "../../utils/Constants";
import { useMutation, useLazyQuery } from "@apollo/client";
import { feedqueries } from "../../graphql/fetching/queries";
import {
  articleMutations,
  farmMutations,
  productMutations,
} from "../../graphql/fetching/mutations";

import {
  ErrorBar,
  LoadingBar,
  NoDataBar,
} from "../../components/molecules/NotificationBars";
import { useStore } from "../../store/hook-store";
import { authClient } from "../../graphql/client";
import InfiniteScroll from "react-infinite-scroll-component";

export default withRouter(function (props) {
  const {
    match: {
      params: { pageNumber },
    },
  } = props;
  const [state, dispatch] = useStore(true);
  const pageRef = useRef({ pageNumber: pageNumber ? pageNumber : 1 });
  const [feeds, setFeeds] = useState([]);

  const [deleteArticle] = useMutation(articleMutations.DELETE_ARTICLE, {
    client: authClient,
  });
  const [deleteFarm] = useMutation(farmMutations.DELETE_FARM, {
    client: authClient,
  });
  const [deleteProduct] = useMutation(productMutations.DELETE_PRODUCT, {
    client: authClient,
  });

  const [fetchFeeds, { loading, data, error, refetch }] = useLazyQuery(
    feedqueries.GET_FEEDS,
    {
      onCompleted: (data) => {
        setPageInfo(data);
        onFetchCompleted(data);
      },
      fetchPolicy: "cache-and-network",
    }
  );

  useEffect(() => {
    if (state.store === "UPDATE" && state.id) {
      refetch();
    }
  }, [state, refetch]);

  useEffect(() => {
    const page = pageRef.current.pageNumber;
    fetchFeeds({
      variables: {
        criterias: {
          page: page ? parseInt(page) : 1,
        },
      },
    });
  }, [fetchFeeds]);

  const setPageInfo = (data) => {
    if (!data) {
      return;
    }
    const {
      feeds: {
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
    if (!data) {
      return;
    }
    const {
      feeds: { collections },
    } = data;
    const feedCollections = parseCollections(collections);
    setFeeds([...feeds, ...feedCollections]);
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

  if (loading && feeds.length === 0) {
    return <LoadingBar>Loading</LoadingBar>;
  }
  if ((!data || !pageRef.current.totalResult) && feeds.length === 0) {
    return <NoDataBar>No data</NoDataBar>;
  }
  if (error) {
    return <ErrorBar>Error!</ErrorBar>;
  }

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
      refetch();
    });
  };

  const onDeleteFarm = (id) => {
    deleteFarm({
      variables: {
        criterias: { id },
      },
    }).then(() => {
      refetch();
    });
  };

  const onDeleteProduct = (id) => {
    deleteProduct({
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
    fetchFeeds({
      variables: {
        criterias: {
          page: pageRef.current.pageNumber,
        },
      },
    });
  };

  return (
    <DefaultLayout>
      <InfiniteScroll
        dataLength={pageRef.current.totalResult}
        next={fetchMoreData}
        hasMore={pageRef.current.currentPage < pageRef.current.totalPage}
        loader={<h4>Loading...</h4>}
      >
        <Feeds
          onOpenDeleteConfirmation={onOpenDeleteConfirmation}
          onDeleteArticle={onDeleteArticle}
          onDeleteFarm={onDeleteFarm}
          onDeleteProduct={onDeleteProduct}
          feeds={feeds}
          baseUrl="/feeds"
        />
      </InfiniteScroll>
    </DefaultLayout>
  );
});
