import React, { useEffect } from "react";
import Feeds from "../../components/templates/Feeds";
import { FeedType } from "../../utils/Enums";
import { UrlConstant } from "../../utils/Constants";
import { useQuery } from "@apollo/client";
import { feedqueries } from "../../graphql/fetching/queries";
import { withRouter } from "react-router-dom";
import Loading from "../../components/atoms/Loading";
import ErrorBlock from "../../components/atoms/ErrorBlock";
import { useStore } from "../../store/hook-store";

export default withRouter((props) => {
  const { match } = props;
  const { params } = match;
  const { pageNumber, pageSize } = params;
  const [state] = useStore(true);
  const { loading, data, error, refetch } = useQuery(feedqueries.GET_FEEDS, {
    variables: {
      criterias: {
        page: pageNumber ? parseInt(pageNumber) : 1,
        pageSize: pageSize ? parseInt(pageSize) : 10,
      },
    },
  });

  useEffect(() => {
    if (state.id) {
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
      feed.thumbnailUrl = `${process.env.REACT_APP_CDN_PHOTO_URL}${feed.pictureId}`;
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

  const baseUrl = "/feeds";
  return (
    <Feeds
      feeds={feeds}
      totalPage={totalPage}
      baseUrl={baseUrl}
      currentPage={page}
    />
  );
});
