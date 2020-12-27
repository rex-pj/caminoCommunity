import React from "react";
import ArticleListItem from "../Article/ArticleListItem";
import ProductListItem from "../Product/ProductListItem";
import FarmListItem from "../Farm/FarmListItem";
import CommunityListItem from "../Community/CommunityListItem";
import { FeedType } from "../../../utils/Enums";

export default function (props) {
  const { feed } = props;

  if (feed.feedType === FeedType.Article) {
    return <ArticleListItem article={feed} />;
  } else if (feed.feedType === FeedType.Product) {
    return <ProductListItem product={feed} />;
  } else if (feed.feedType === FeedType.Farm) {
    return <FarmListItem farm={feed} />;
  } else if (feed.feedType === FeedType.Community) {
    return <CommunityListItem community={feed} />;
  }
}
