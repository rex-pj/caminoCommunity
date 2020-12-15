import React from "react";
import { Fragment } from "react";
import { Pagination } from "../../organisms/Paging";
import FeedItem from "../../organisms/Feeds/FeedItem";

export default (props) => {
  const { feeds, totalPage, baseUrl, currentPage } = props;
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
};
