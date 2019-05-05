import React, { Component, Fragment } from "react";
import ArticleListItem from "../../organisms/Article/ArticleListItem";
import { Pagination } from "../../molecules/Paging";
import Breadcrumb from "../../molecules/Breadcrumb";

export default class Article extends Component {
  render() {
    const {
      articles,
      breadcrumbs,
      totalPage,
      baseUrl,
      currentPage
    } = this.props;

    return (
      <Fragment>
        <Breadcrumb list={breadcrumbs} />
        {articles
          ? articles.map(item => (
              <ArticleListItem key={item.id} article={item} />
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
}
