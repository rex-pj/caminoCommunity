import React, { useEffect, Fragment } from "react";
import { PanelDefault } from "../../atoms/Panels";
import ArticleDetail from "../../organisms/Article/ArticleDetail";
import ArticleItem from "../../organisms/Article/ArticleItem";
import styled from "styled-components";
import { TertiaryHeading } from "../../atoms/Heading";

const RelationBox = styled.div`
  margin-top: ${p => p.theme.size.distance};
`;

export default function(props) {
  const { article, relationArticles } = props;
  useEffect(function() {
    props.fetchRelationArticles();
  });

  return (
    <Fragment>
      <PanelDefault>
        <ArticleDetail article={article} />
      </PanelDefault>
      <RelationBox>
        <TertiaryHeading>Chủ đề khác</TertiaryHeading>
        <div className="row">
          {relationArticles
            ? relationArticles.map((item, index) => {
                return (
                  <div
                    key={index}
                    className="col col-12 col-sm-6 col-md-6 col-lg-6 col-xl-4"
                  >
                    <ArticleItem article={item} />
                  </div>
                );
              })
            : null}
        </div>
      </RelationBox>
    </Fragment>
  );
}
