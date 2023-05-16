import * as React from "react";
import ArticleListItem from "../Article/ArticleListItem";
import ProductListItem from "../Product/ProductListItem";
import FarmListItem from "../Farm/FarmListItem";
import CommunityListItem from "../Community/CommunityListItem";
import { FeedType } from "../../../utils/Enums";

type Props = {
  feed: any;
  onOpenDeleteConfirmation?: (e: any, onDeleteFunc: any) => void;
  onDeleteArticle?: (id: number) => Promise<any>;
  onDeleteFarm?: (id: number) => Promise<any>;
  onDeleteProduct?: (id: number) => Promise<any>;
};

const FeedItem = (props: Props) => {
  const {
    feed,
    onOpenDeleteConfirmation,
    onDeleteArticle,
    onDeleteFarm,
    onDeleteProduct,
  } = props;

  const onOpenDeleteFarmConfirmation = (e: any) => {
    if (onOpenDeleteConfirmation) {
      onOpenDeleteConfirmation(e, onDeleteFarm);
    }
  };

  const onOpenDeleteArticleConfirmation = (e: any) => {
    if (onOpenDeleteConfirmation) {
      onOpenDeleteConfirmation(e, onDeleteArticle);
    }
  };

  const onOpenDeleteProductConfirmation = (e: any) => {
    if (onOpenDeleteConfirmation) {
      onOpenDeleteConfirmation(e, onDeleteProduct);
    }
  };

  if (feed.feedType === FeedType.Article) {
    return (
      <ArticleListItem
        article={feed}
        onOpenDeleteConfirmationModal={onOpenDeleteArticleConfirmation}
      />
    );
  }
  if (feed.feedType === FeedType.Product) {
    return (
      <ProductListItem
        product={feed}
        onOpenDeleteConfirmationModal={onOpenDeleteProductConfirmation}
      />
    );
  }
  if (feed.feedType === FeedType.Farm) {
    return (
      <FarmListItem
        farm={feed}
        onOpenDeleteConfirmationModal={onOpenDeleteFarmConfirmation}
      />
    );
  }

  return <CommunityListItem community={feed} />;
};

export default FeedItem;
