import React, { Fragment } from "react";
import ArticleListItem from "../../organisms/Article/ArticleListItem";
import { Pagination } from "../../organisms/Paging";
import Breadcrumb from "../../organisms/Navigation/Breadcrumb";

export default function (props) {
  const {
    articles,
    breadcrumbs,
    totalPage,
    baseUrl,
    currentPage,
    onOpenDeleteConfirmation,
  } = props;

  return (
    <Fragment>
      <Breadcrumb list={breadcrumbs} />
      {articles
        ? articles.map((item) => (
            <ArticleListItem
              key={item.id}
              article={item}
              onOpenDeleteConfirmationModal={onOpenDeleteConfirmation}
            />
          ))
        : null}
      <Pagination
        totalPage={totalPage}
        baseUrl={baseUrl}
        currentPage={currentPage}
      />
    </Fragment>
  );
}
