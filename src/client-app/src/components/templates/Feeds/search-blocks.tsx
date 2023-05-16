import * as React from "react";
import { Fragment } from "react";
import { Pager } from "../../organisms/Paging";
import SearchItem from "../../organisms/Feeds/SearchItem";
import { IBreadcrumbItem } from "../../organisms/Navigation/Breadcrumb";

type Props = {
  feeds: any[];
  breadcrumbs?: IBreadcrumbItem[];
  totalPage?: number;
  baseUrl: string;
  currentPage?: number;
  pageQuery: string;
};

const SearchBlocks = (props: Props) => {
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

export default SearchBlocks;
