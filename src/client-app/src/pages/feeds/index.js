import React, { useEffect } from "react";
import { DefaultLayout } from "../../components/templates/Layout";
import Feeds from "../../components/templates/Feeds";
import { FeedType } from "../../utils/Enums";
import { UrlConstant } from "../../utils/Constants";
import { useQuery, useMutation } from "@apollo/client";
import { feedqueries } from "../../graphql/fetching/queries";
import {
  articleMutations,
  farmMutations,
  productMutations,
} from "../../graphql/fetching/mutations";
import { withRouter } from "react-router-dom";
import Loading from "../../components/atoms/Loading";
import ErrorBlock from "../../components/atoms/ErrorBlock";
import { useStore } from "../../store/hook-store";
import { authClient } from "../../graphql/client";

export default withRouter((props) => {
  const { match } = props;
  const { params } = match;

  const { pageNumber, pageSize } = params;
  const [state, dispatch] = useStore(true);
  const { loading, data, error, refetch } = useQuery(feedqueries.GET_FEEDS, {
    variables: {
      criterias: {
        page: pageNumber ? parseInt(pageNumber) : 1,
        pageSize: pageSize ? parseInt(pageSize) : 10,
      },
    },
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

  useEffect(() => {
    if (state.store === "UPDATE" && state.id) {
      refetch();
    }
  }, [state, refetch]);

  if (loading || !data) {
    return <Loading>Loading</Loading>;
  } else if (error) {
    return <ErrorBlock>Error!</ErrorBlock>;
  }

  const { feeds: dataFeeds } = data;
  const { totalPage, filter, collections } = dataFeeds;
  const { page } = filter;

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

  return (
    <DefaultLayout>
      <Feeds
        onOpenDeleteConfirmation={onOpenDeleteConfirmation}
        onDeleteArticle={onDeleteArticle}
        onDeleteFarm={onDeleteFarm}
        onDeleteProduct={onDeleteProduct}
        feeds={feeds}
        totalPage={totalPage}
        baseUrl="/feeds"
        currentPage={page}
      />
    </DefaultLayout>
  );
});
