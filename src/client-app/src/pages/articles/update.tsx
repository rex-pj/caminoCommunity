import * as React from "react";
import { useEffect } from "react";
import { useMutation, useQuery } from "@apollo/client";
import Breadcrumb from "../../components/organisms/Navigation/Breadcrumb";
import { useLocation, useNavigate, useParams } from "react-router-dom";
import ArticleEditor from "../../components/organisms/Article/ArticleEditor";
import { articleMutations } from "../../graphql/fetching/mutations";
import {
  articleQueries,
  farmQueries,
  userQueries,
} from "../../graphql/fetching/queries";
import { useStore } from "../../store/hook-store";
import { ArticleCreationModel } from "../../models/articleCreationModel";
import DetailLayout from "../../components/templates/Layout/DetailLayout";
import ArticleService from "../../services/articleService";

type Props = {};

const UpdatePage = (props: Props) => {
  const location = useLocation();
  const navigate = useNavigate();
  const { id } = useParams();
  const dispatch = useStore(false)[1];
  const [articleCategories] = useMutation(
    articleMutations.FILTER_ARTICLE_CATEGORIES
  );
  const articleService = new ArticleService();

  const { loading, data, error, refetch, called } = useQuery(
    articleQueries.GET_ARTICLE_FOR_UPDATE,
    {
      variables: {
        criterias: {
          id: Number(id),
        },
      },
      fetchPolicy: "cache-and-network",
    }
  );

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

  useEffect(() => {
    if (!loading && called) {
      window.scrollTo(0, 0);
      refetch();
    }
  }, [refetch, called, loading]);

  const article = data ? { ...data.article } : {};

  const breadcrumbs = [
    {
      title: "Articles",
      url: "/articles/",
    },
    {
      title: article.name,
      url: `/Articles/${article.id}`,
    },
    {
      isActived: true,
      title: "Update",
    },
  ];

  function showValidationError(title: string, message: string) {
    dispatch("NOTIFY", {
      title,
      message,
      type: "error",
    });
  }

  async function onArticlePost(data: any) {
    return await articleService
      .update(data, data.get("id"))
      .then((response) => {
        return new Promise((resolve) => {
          if (location.state && location.state.from) {
            const referrefUri = location.state.from;
            const articleUpdateUrl = `/articles/update/${data.id}`;
            if (referrefUri !== articleUpdateUrl) {
              navigate(referrefUri);
              resolve(response);
              return;
            }
          }

          navigate(`/articles/${article.id}`);
          resolve(response);
        });
      });
  }

  const currentArticle = { ...new ArticleCreationModel() };
  for (const formIdentifier in currentArticle) {
    currentArticle[formIdentifier].value = article[formIdentifier];
    if (article[formIdentifier]) {
      currentArticle[formIdentifier].isValid = true;
    }
  }

  const getAuthorInfo = () => {
    if (!authorData) {
      return {};
    }
    const { userInfo } = authorData;
    const authorInfo = { ...userInfo };
    if (authorData) {
      const { userPhotos } = authorData;
      const avatar = userPhotos.find(
        (item: any) => item.photoType === "AVATAR"
      );
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

  return (
    <DetailLayout
      author={getAuthorInfo()}
      isLoading={!!loading}
      hasData={true}
      hasError={!!error}
    >
      <Breadcrumb list={breadcrumbs} />
      <ArticleEditor
        height={350}
        currentArticle={currentArticle}
        filterCategories={articleCategories}
        onArticlePost={onArticlePost}
        showValidationError={showValidationError}
      ></ArticleEditor>
    </DetailLayout>
  );
};

export default UpdatePage;
