import React from "react";
import { Fragment } from "react";
import { Pager } from "../../organisms/Paging";
import SearchItem from "../../organisms/Feeds/SearchItem";

export default (props) => {
  const { feeds, totalPage, baseUrl, pageQuery, currentPage } = props;

  return (
    <Fragment>
      {feeds.map((item, index) => (
        <SearchItem key={index} feed={item} />
      ))}
      <Pager
        totalPage={totalPage}
        baseUrl={baseUrl}
        pageQuery={pageQuery}
        currentPage={currentPage}
        text="Xem thêm"
        bulletSize="xs"
      />
    </Fragment>
  );
};
