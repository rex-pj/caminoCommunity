import * as React from "react";
import ArticleSearchedItem from "../Article/ArticleSearchedItem";
import ProductSearchedItem from "../Product/ProductSearchedItem";
import FarmSearchedItem from "../Farm/FarmSearchedItem";
import UserSearchedItem from "../Profile/UserSearchedItem";
import { FeedType } from "../../../utils/Enums";

type Props = {
  feed: any;
};

const SearchItem = (props: Props) => {
  const { feed } = props;

  if (feed.feedType === FeedType.Product) {
    return <ProductSearchedItem product={feed} />;
  }

  if (feed.feedType === FeedType.Farm) {
    return <FarmSearchedItem farm={feed} />;
  }

  if (feed.feedType === FeedType.User) {
    return (
      <div className="col col-12 col-sm-6 col-md-4 col-lg-4 col-xl-3">
        <UserSearchedItem user={feed} />
      </div>
    );
  }

  return <ArticleSearchedItem article={feed} />;
};

export default SearchItem;
