import React from "react";
import ArticleListItem from "../Article/ArticleListItem";
import ProductListItem from "../Product/ProductListItem";
import FarmListItem from "../Farm/FarmListItem";
import AssociationListItem from "../Association/AssociationListItem";
import { FeedType } from "../../../utils/Enums";

export default function (props) {
  const { feed } = props;

  if (feed.feedType === FeedType.Article) {
    return <ArticleListItem article={feed} />;
  } else if (feed.feedType === FeedType.Product) {
    return <ProductListItem product={feed} />;
  } else if (feed.feedType === FeedType.Farm) {
    return <FarmListItem farm={feed} />;
  } else if (feed.feedType === FeedType.Association) {
    return <AssociationListItem association={feed} />;
  }
}
