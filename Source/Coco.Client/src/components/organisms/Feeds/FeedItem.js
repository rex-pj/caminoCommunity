import React from "react";
import ArticleListItem from "../Article/ArticleListItem";
import ProductListItem from "../Product/ProductListItem";
import FarmListItem from "../Farm/FarmListItem";
import FarmGroupListItem from "../FarmGroup/FarmGroupListItem";
import { ContentType } from "../../../utils/Enums";

export default function(props) {
  const { feed } = props;
  let result = null;

  if (feed.contentType === ContentType.Article) {
    result = <ArticleListItem key={feed.id} article={feed} />;
  } else if (feed.contentType === ContentType.Product) {
    result = <ProductListItem key={feed.id} product={feed} />;
  } else if (feed.contentType === ContentType.Farm) {
    result = <FarmListItem key={feed.id} farm={feed} />;
  } else if (feed.contentType === ContentType.FarmGroup) {
    result = <FarmGroupListItem key={feed.id} farmGroup={feed} />;
  }

  return result;
}
