import React from "react";
import { Component, Fragment } from "react";
import { Pagination } from "../../organisms/Paging";
import FeedItem from "../../organisms/Feeds/FeedItem";

export default class extends Component {
  render() {
    const { feeds, totalPage, baseUrl, currentPage } = this.props;
    return (
      <Fragment>
        {feeds.map((item, index) => (
          <FeedItem key={index} feed={item} />
        ))}
        <Pagination
          totalPage={totalPage}
          baseUrl={baseUrl}
          currentPage={currentPage}
        />
      </Fragment>
    );
  }
}
