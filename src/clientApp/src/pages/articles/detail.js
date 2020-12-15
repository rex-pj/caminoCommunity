import React, { Fragment, useEffect } from "react";
import Breadcrumb from "../../components/organisms/Navigation/Breadcrumb";
import Detail from "../../components/templates/Article/Detail";
import { UrlConstant } from "../../utils/Constants";
import {
  GET_ARTICLE,
  GET_RELEVANT_ARTICLES,
} from "../../utils/GraphQLQueries/queries";
import { useQuery } from "@apollo/client";
import { withRouter } from "react-router-dom";
import Loading from "../../components/atoms/Loading";
import { TertiaryDarkHeading } from "../../components/atoms/Heading";
import ArticleItem from "../../components/organisms/Article/ArticleItem";
import styled from "styled-components";
import ErrorBlock from "../../components/atoms/ErrorBlock";
import { useStore } from "../../store/hook-store";

const RelationBox = styled.div`
  margin-top: ${(p) => p.theme.size.distance};
`;

export default withRouter(function (props) {
  const { match } = props;
  const { params } = match;
  const { id } = params;
  const [state] = useStore(true);
  const { loading, data, error, refetch } = useQuery(GET_ARTICLE, {
    variables: {
      criterias: {
        id: parseFloat(id),
      },
    },
  });

  const {
    relevantLoading,
    data: relevantData,
    error: relevantError,
  } = useQuery(GET_RELEVANT_ARTICLES, {
    variables: {
      criterias: {
        id: parseFloat(id),
        page: 1,
        pageSize: 8,
      },
    },
  });

  useEffect(() => {
    if (state.type === "ARTICLE" && state.id) {
      refetch();
    }
  }, [state, refetch]);

  if (loading || !data) {
    return <Loading>Loading...</Loading>;
  } else if (error) {
    return <ErrorBlock>Error!</ErrorBlock>;
  }

  const { article: articleResponse } = data;
  let article = { ...articleResponse };

  const breadcrumbs = [
    {
      title: "Articles",
      url: "/articles/",
    },
    {
      isActived: true,
      title: article.name,
    },
  ];

  if (article.thumbnail.pictureId) {
    article.thumbnailUrl = `${process.env.REACT_APP_CDN_PHOTO_URL}${article.thumbnail.pictureId}`;
  }

  const renderRelevants = (relevantLoading, relevantData, relevantError) => {
    if (relevantLoading || !relevantData) {
      return <Loading>Loading...</Loading>;
    } else if (relevantError) {
      return <ErrorBlock>Error!</ErrorBlock>;
    }
    const { relevantArticles } = relevantData;
    const relevants = relevantArticles.map((item) => {
      let articleItem = { ...item };
      articleItem.url = `${UrlConstant.Article.url}${articleItem.id}`;
      if (articleItem.thumbnail.pictureId) {
        articleItem.thumbnailUrl = `${process.env.REACT_APP_CDN_PHOTO_URL}${articleItem.thumbnail.pictureId}`;
      }

      articleItem.creator = {
        createdDate: item.createdDate,
        profileUrl: `/profile/${item.createdByIdentityId}`,
        name: item.createdBy,
      };

      if (item.createdByPhotoCode) {
        articleItem.creator.photoUrl = `${process.env.REACT_APP_CDN_AVATAR_API_URL}${item.createdByPhotoCode}`;
      }

      return articleItem;
    });

    return (
      <RelationBox>
        <TertiaryDarkHeading>Chủ đề khác</TertiaryDarkHeading>
        <div className="row">
          {relevants
            ? relevants.map((item, index) => {
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
    );
  };

  return (
    <Fragment>
      <Breadcrumb list={breadcrumbs} />
      <Detail article={article} />
      {renderRelevants(relevantLoading, relevantData, relevantError)}
    </Fragment>
  );
});
