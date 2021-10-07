import React from "react";
import { Fragment } from "react";
import FeedItem from "../../organisms/Feeds/FeedItem";

export default (props) => {
  const {
    feeds,
    onDeleteArticle,
    onDeleteFarm,
    onDeleteProduct,
    onOpenDeleteConfirmation,
  } = props;

  return (
    <Fragment>
      {feeds.map((item, index) => (
        <FeedItem
          key={index}
          feed={item}
          onOpenDeleteConfirmation={onOpenDeleteConfirmation}
          onDeleteArticle={onDeleteArticle}
          onDeleteFarm={onDeleteFarm}
          onDeleteProduct={onDeleteProduct}
        />
      ))}
    </Fragment>
  );
};
