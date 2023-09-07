import * as React from "react";
import { useEffect, useState, useRef } from "react";
import { DefaultLayout } from "../../components/templates/Layout";
import { useParams } from "react-router-dom";
import Feeds from "../../components/templates/Feeds";
import { FeedType } from "../../utils/Enums";
import { UrlConstant } from "../../utils/Constants";
import { useLazyQuery } from "@apollo/client";
import { feedqueries } from "../../graphql/fetching/queries";
import { useStore } from "../../store/hook-store";
import InfiniteScroll from "react-infinite-scroll-component";
import { apiConfig } from "../../config/api-config";
import { LoadingBar } from "../../components/molecules/NotificationBars";
import ArticleService from "../../services/articleService";
import ProductService from "../../services/productService";
import FarmService from "../../services/farmService";
import { Helmet } from "react-helmet-async";

interface Props {}

const FeedPage = (props: Props) => {
  const articleService = new ArticleService();
  const productService = new ProductService();
  const farmService = new FarmService();
  const { pageNumber } = useParams();
  const [state, dispatch] = useStore(true);
  const pageRef = useRef<any>({
    pageNumber: pageNumber ? pageNumber : 1,
  });
  const [feeds, setFeeds] = useState<any[]>([]);

  const [fetchFeeds, { loading, data, error, refetch }] = useLazyQuery(feedqueries.GET_FEEDS, {
    onCompleted: (data) => {
      setPageInfo(data);
      onFetchCompleted(data);
    },
    fetchPolicy: "cache-and-network",
  });

  useEffect(() => {
    const page = pageRef.current.pageNumber;
    fetchFeeds({
      variables: {
        criterias: {
          page: page ? page : 1,
        },
      },
    });
  }, [fetchFeeds]);

  useEffect(() => {
    if (state.store === "UPDATE" && state.id) {
      refetch();
    }
  }, [state, refetch]);

  const setPageInfo = (data: any) => {
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

  const onFetchCompleted = (data: any) => {
    if (!data) {
      return;
    }
    const {
      feeds: { collections },
    } = data;
    const feedCollections = parseCollections(collections);
    setFeeds([...feeds, ...feedCollections]);
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

  const onOpenDeleteConfirmation = (e: any, onDelete: any) => {
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

  const onDeleteArticle = (id: number) => {
    return articleService.delete(id).then(() => {
      refetch();
    });
  };

  const onDeleteFarm = (id: number) => {
    return farmService.delete(id).then(() => {
      refetch();
    });
  };

  const onDeleteProduct = (id: number) => {
    return productService.delete(id).then(() => {
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

  const checkHasData = () => {
    return !!(data && pageRef.current.totalResult && feeds.length > 0);
  };

  return (
    <>
      <Helmet>
        <meta charSet="utf-8" />
        <title>Tổng hợp | Nông Trại LỒ Ồ</title>
        <meta property="og:title" content="Tổng hợp | Nông Trại LỒ Ồ" />
        <meta property="og:description" content="Tổng hợp" />
        {/* Google SEO */}
        <meta name="description" content="Tổng hợp" />
      </Helmet>
      <DefaultLayout isLoading={!!loading} hasData={checkHasData()} hasError={!!error}>
        <InfiniteScroll dataLength={pageRef.current.totalResult ?? 0} next={fetchMoreData} hasMore={pageRef.current.currentPage < pageRef.current.totalPage} loader={<LoadingBar />}>
          <Feeds onOpenDeleteConfirmation={onOpenDeleteConfirmation} onDeleteArticle={onDeleteArticle} onDeleteFarm={onDeleteFarm} onDeleteProduct={onDeleteProduct} feeds={feeds} baseUrl="/feeds" />
        </InfiniteScroll>
      </DefaultLayout>
    </>
  );
};

export default FeedPage;
