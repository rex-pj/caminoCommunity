import React, { Fragment } from "react";
import ArticleListItem from "../../organisms/Article/ArticleListItem";
import { Pagination } from "../../molecules/Paging";
import loadable from "@loadable/component";
const Breadcrumb = loadable(() => import("../../molecules/Breadcrumb"));

export default function(props) {
  const { articles, breadcrumbs, totalPage, baseUrl, currentPage } = props;

  return (
    <Fragment>
      <Breadcrumb list={breadcrumbs} />
      {articles
        ? articles.map(item => <ArticleListItem key={item.id} article={item} />)
        : null}
      <Pagination
        totalPage={totalPage}
        baseUrl={baseUrl}
        currentPage={currentPage}
      />
    </Fragment>
  );
}
