import * as React from "react";
import ArticleListItem from "../Article/ArticleListItem";
import ProductListItem from "../Product/ProductListItem";
import FarmListItem from "../Farm/FarmListItem";
import CommunityListItem from "../Community/CommunityListItem";
import { FeedType } from "../../../utils/Enums";

export default function (props) {
  const {
    feed,
    onOpenDeleteConfirmation,
    onDeleteArticle,
    onDeleteFarm,
    onDeleteProduct,
  } = props;

  const onOpenDeleteFarmConfirmation = (e) => {
    onOpenDeleteConfirmation(e, onDeleteFarm);
  };

  const onOpenDeleteArticleConfirmation = (e) => {
    onOpenDeleteConfirmation(e, onDeleteArticle);
  };

  const onOpenDeleteProductConfirmation = (e) => {
    onOpenDeleteConfirmation(e, onDeleteProduct);
  };

  if (feed.feedType === FeedType.Article) {
    return (
      <ArticleListItem
        article={feed}
        onOpenDeleteConfirmationModal={onOpenDeleteArticleConfirmation}
      />
    );
  } else if (feed.feedType === FeedType.Product) {
    return (
      <ProductListItem
        product={feed}
        onOpenDeleteConfirmationModal={onOpenDeleteProductConfirmation}
      />
    );
  } else if (feed.feedType === FeedType.Farm) {
    return (
      <FarmListItem
        farm={feed}
        onOpenDeleteConfirmationModal={onOpenDeleteFarmConfirmation}
      />
    );
  } else if (feed.feedType === FeedType.Community) {
    return <CommunityListItem community={feed} />;
  }
}
