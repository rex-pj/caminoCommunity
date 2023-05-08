import React, { Fragment } from "react";
import ArticleListItem from "../../organisms/Article/ArticleListItem";

export default function (props) {
  const { articles, onOpenDeleteConfirmation } = props;

  return (
    <Fragment>
      {articles
        ? articles.map((item) => (
            <ArticleListItem
              key={item.id}
              article={item}
              onOpenDeleteConfirmationModal={onOpenDeleteConfirmation}
            />
          ))
        : null}
    </Fragment>
  );
}
