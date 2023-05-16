import * as React from "react";
import { Fragment } from "react";
import FeedItem from "../../organisms/Feeds/FeedItem";

export interface IAdvancedSearchResult {
  farms: any[];
  products: any[];
  articles: any[];
  users: any[];
  totalFarmPage: number;
  totalProductPage: number;
  totalArticlePage: number;
  totalUserPage: number;
  page: number;
  userFilterByName?: string;
}

type Props = {
  feeds: any[];
  baseUrl: string;
  totalPage?: number;
  currentPage?: number;
  onOpenDeleteConfirmation?: (e: any, onDeleteFunc: any) => void;
  onDeleteArticle?: (id: number) => Promise<any>;
  onDeleteFarm?: (id: number) => Promise<any>;
  onDeleteProduct?: (id: number) => Promise<any>;
};

const index = (props: Props) => {
  const {
    feeds,
    onDeleteArticle,
    onDeleteFarm,
    onDeleteProduct,
    onOpenDeleteConfirmation,
  } = props;

  return (
    <Fragment>
      {feeds.map((item: any, index: number) => (
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

export default index;
