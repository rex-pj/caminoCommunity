import * as React from "react";
import { Fragment } from "react";
import ArticleListItem from "../../organisms/Article/ArticleListItem";
import { IBreadcrumbItem } from "../../organisms/Navigation/Breadcrumb";

type Props = {
  articles: any[];
  baseUrl: string;
  breadcrumbs: IBreadcrumbItem[];
  onOpenDeleteConfirmation: (e: any, onDeleteFunc: void) => void;
};

const Index = (props: Props) => {
  const { articles, onOpenDeleteConfirmation } = props;

  return (
    <Fragment>
      {articles
        ? articles.map((item) => (
            <ArticleListItem
              key={item.id}
              article={item}
              onOpenDeleteConfirmationModal={onOpenDeleteConfirmation}
            />
          ))
        : null}
    </Fragment>
  );
};

export default Index;
