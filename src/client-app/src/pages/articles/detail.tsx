import * as React from "react";
import { useContext, useEffect, useMemo } from "react";
import Breadcrumb from "../../components/organisms/Navigation/Breadcrumb";
import Detail from "../../components/templates/Article/Detail";
import { UrlConstant } from "../../utils/Constants";
import { articleQueries, userQueries, farmQueries } from "../../graphql/fetching/queries";

import { ErrorBar, LoadingBar } from "../../components/molecules/NotificationBars";
import { useQuery } from "@apollo/client";
import { useLocation, useParams, useNavigate } from "react-router-dom";
import { TertiaryDarkHeading } from "../../components/atoms/Heading";
import ArticleItem from "../../components/organisms/Article/ArticleItem";
import styled from "styled-components";
import { useStore } from "../../store/hook-store";
import DetailLayout from "../../components/templates/Layout/DetailLayout";
import { apiConfig } from "../../config/api-config";
import ArticleService from "../../services/articleService";
import { removeHtmlTags } from "../../utils/Helper";
import { Helmet } from "react-helmet-async";

const RelationBox = styled.div`
  margin-top: ${(p) => p.theme.size.distance};
`;

interface Props {}

const ArticleDetail = (props: Props) => {
  const articleService = useMemo(() => new ArticleService(), []);
  const location = useLocation();
  const navigate = useNavigate();
  const { id } = useParams();
  const [state, dispatch] = useStore(true);
  const { loading, data, error, refetch } = useQuery(articleQueries.GET_ARTICLE, {
    variables: {
      criterias: {
        id: parseFloat(id ?? ""),
      },
    },
  });

  const userIdentityId = data?.article?.createdByIdentityId;
  const { data: authorData } = useQuery(userQueries.GET_USER_INFO, {
    skip: !userIdentityId,
    variables: {
      criterias: {
        userId: userIdentityId,
      },
    },
  });

  const { data: userFarmData } = useQuery(farmQueries.GET_USER_FARMS, {
    skip: !userIdentityId,
    variables: {
      criterias: {
        userIdentityId: userIdentityId,
        page: 1,
        pageSize: 5,
      },
    },
  });

  const {
    loading: relevantLoading,
    data: relevantData,
    error: relevantError,
    refetch: refetchRelevants,
  } = useQuery(articleQueries.GET_RELEVANT_ARTICLES, {
    variables: {
      criterias: {
        id: Number(id),
        page: 1,
        pageSize: 8,
      },
    },
  });

  const onOpenDeleteConfirmation = (e: any, onDeleteData: (id: number) => Promise<any>) => {
    const { title, innerModal, message, id } = e;
    dispatch("OPEN_MODAL", {
      data: {
        title: title,
        children: message,
        id: id,
      },
      execution: { onDelete: onDeleteData },
      options: {
        isOpen: true,
        innerModal: innerModal,
        position: "fixed",
      },
    });
  };

  const onOpenDeleteMainConfirmation = (e: any) => {
    onOpenDeleteConfirmation(e, onDeleteMain);
  };

  function onOpenDeleteRelevantConfirmation(e: any) {
    onOpenDeleteConfirmation(e, onDeleteRelevant);
  }

  const onDeleteMain = (id: number) => {
    return articleService.delete(id).then(() => {
      if (location.state?.from) {
        dispatch("ARTICLE_DELETE", {
          id: id,
        });
        navigate({
          pathname: location.state.from,
        });
        return;
      }
      navigate({
        pathname: `/`,
      });
    });
  };

  const onDeleteRelevant = (id: number) => {
    return articleService.delete(id).then(() => {
      dispatch("ARTICLE_DELETE", {
        id: id,
      });
      refetchRelevants();
    });
  };

  useEffect(() => {
    if (state.type === "ARTICLE_UPDATE" && state.id) {
      refetch();
    }
  }, [state, refetch]);

  const renderRelevants = (relevantLoading: boolean, relevantData: any, relevantError: any) => {
    if (relevantLoading || !relevantData) {
      return <LoadingBar />;
    } else if (relevantError) {
      return <ErrorBar />;
    }
    const { relevantArticles } = relevantData;
    const relevants = relevantArticles.map((item: any) => {
      let articleItem = { ...item };
      articleItem.url = `${UrlConstant.Article.url}${articleItem.id}`;
      if (articleItem.picture.pictureId) {
        articleItem.pictureUrl = `${apiConfig.paths.pictures.get.getPicture}/${articleItem.picture.pictureId}`;
      }

      articleItem.creator = {
        createdDate: item.createdDate,
        profileUrl: `/profile/${item.createdByIdentityId}`,
        name: item.createdBy,
      };

      if (item.createdByPhotoId) {
        articleItem.creator.photoUrl = `${apiConfig.paths.userPhotos.get.getAvatar}/${item.createdByPhotoId}`;
      }

      return articleItem;
    });

    return (
      <RelationBox>
        <TertiaryDarkHeading>Chủ đề khác</TertiaryDarkHeading>
        <div className="row">
          {relevants
            ? relevants.map((item: any, index: number) => {
                return (
                  <div key={index} className="col col-12 col-sm-6 col-md-6 col-lg-6 col-xl-4">
                    <ArticleItem article={item} onOpenDeleteConfirmationModal={onOpenDeleteRelevantConfirmation} />
                  </div>
                );
              })
            : null}
        </div>
      </RelationBox>
    );
  };

  const getAuthorInfo = () => {
    if (!authorData) {
      return {};
    }
    const { userInfo } = authorData;
    const authorInfo = { ...userInfo };
    if (authorData) {
      const { userPhotos } = authorData;
      const avatar = userPhotos.find((item: any) => item.photoType === "AVATAR");
      if (avatar) {
        authorInfo.userAvatar = avatar;
      }
      const cover = userPhotos.find((item: any) => item.photoType === "COVER");
      if (cover) {
        authorInfo.userCover = cover;
      }
    }

    if (userFarmData) {
      const { userFarms } = userFarmData;
      const { collections } = userFarms;
      authorInfo.farms = collections;
    }
    return authorInfo;
  };

  const article = data ? { ...data.article } : {};

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

  if (article.picture?.pictureId) {
    article.pictureUrl = `${apiConfig.paths.pictures.get.getPicture}/${article.picture.pictureId}`;
  }

  const metaTitle = `${article?.name} ${"| Nông Trại LỒ Ồ"}`;
  const metaDescription = removeHtmlTags(article.content);
  return (
    <>
      <Helmet>
        {metaTitle ? <title>{metaTitle}</title> : null}
        {metaTitle ? <meta property="og:title" content={metaTitle} /> : null}
        {metaDescription ? <meta property="og:description" content={metaDescription} /> : null}
        {article?.pictureUrl ? <meta property="og:image" content={article.pictureUrl} /> : null}
        {/* Google SEO */}
        {metaDescription ? <meta name="description" content={metaDescription} /> : null}
      </Helmet>
      <DetailLayout author={getAuthorInfo()} isLoading={!!loading} hasData={true} hasError={!!error}>
        <Breadcrumb list={breadcrumbs} />
        <Detail article={article} onOpenDeleteConfirmationModal={onOpenDeleteMainConfirmation} />
        {renderRelevants(relevantLoading, relevantData, relevantError)}
      </DetailLayout>
    </>
  );
};

export default ArticleDetail;
