import React, { useEffect } from "react";
import { useMutation, useQuery } from "@apollo/client";
import Breadcrumb from "../../components/organisms/Navigation/Breadcrumb";
import Loading from "../../components/atoms/Loading";
import { withRouter } from "react-router-dom";
import ErrorBlock from "../../components/atoms/ErrorBlock";
import ArticleEditor from "../../components/organisms/Article/ArticleEditor";
import authClient from "../../graphql/client/authClient";
import {
  articleMutations,
  mediaMutations,
} from "../../graphql/fetching/mutations";
import {
  articleQueries,
  farmQueries,
  userQueries,
} from "../../graphql/fetching/queries";
import { useStore } from "../../store/hook-store";
import { fileToBase64 } from "../../utils/Helper";
import articleCreationModel from "../../models/articleCreationModel";
import DetailLayout from "../../components/templates/Layout/DetailLayout";

export default withRouter(function (props) {
  const { match } = props;
  const { params } = match;
  const { id } = params;
  const dispatch = useStore(false)[1];
  const [validateImageUrl] = useMutation(mediaMutations.VALIDATE_IMAGE_URL);
  const [updateArticle] = useMutation(articleMutations.UPDATE_ARTICLE, {
    client: authClient,
  });
  const [articleCategories] = useMutation(
    articleMutations.FILTER_ARTICLE_CATEGORIES
  );

  const { loading, data, error, refetch, called } = useQuery(
    articleQueries.GET_ARTICLE_FOR_UPDATE,
    {
      variables: {
        criterias: {
          id: parseFloat(id),
        },
      },
    }
  );

  const userIdentityId = data?.farm?.createdByIdentityId;
  const { data: authorData } = useQuery(userQueries.GET_USER_INFO, {
    skip: !userIdentityId,
    variables: {
      criterias: {
        userId: userIdentityId,
      },
    },
  });

  const { data: userFarmData } = useQuery(farmQueries.SELECT_USER_FARMS, {
    skip: !userIdentityId,
    variables: {
      criterias: {
        userIdentityId: userIdentityId,
        page: 1,
      },
    },
  });

  useEffect(() => {
    if (!loading && called) {
      window.scrollTo(0, 0);
      refetch();
    }
  }, [refetch, called, loading]);

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
      title: article.name,
      url: `/Articles/${article.id}`,
    },
    {
      isActived: true,
      title: "Update",
    },
  ];

  const showValidationError = (title, message) => {
    dispatch("NOTIFY", {
      title,
      message,
      type: "error",
    });
  };

  const convertImagefile = async (file) => {
    const url = await fileToBase64(file);
    return {
      url,
      fileName: file.name,
    };
  };

  const onImageValidate = async (value) => {
    return await validateImageUrl({
      variables: {
        criterias: {
          url: value,
        },
      },
    });
  };

  const onArticlePost = async (data) => {
    return await updateArticle({
      variables: {
        criterias: data,
      },
    }).then((response) => {
      return new Promise((resolve) => {
        const { data } = response;
        const { updateArticle: article } = data;
        if (props.location.state && props.location.state.from) {
          const referrefUri = props.location.state.from;
          const articleUpdateUrl = `/articles/update/${article.id}`;
          if (referrefUri !== articleUpdateUrl) {
            raiseArticleUpdatedNotify(article);
            props.history.push(referrefUri);
            resolve({ article });
            return;
          }
        }

        raiseArticleUpdatedNotify(article);
        props.history.push(`/articles/${article.id}`);
        resolve({ article });
      });
    });
  };

  const raiseArticleUpdatedNotify = (article) => {
    dispatch("ARTICLE_UPDATE", {
      id: article.id,
    });
  };

  const currentArticle = JSON.parse(JSON.stringify(articleCreationModel));
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
      const avatar = userPhotos.find((item) => item.photoType === "AVATAR");
      if (avatar) {
        authorInfo.userAvatar = avatar;
      }
      const cover = userPhotos.find((item) => item.photoType === "COVER");
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
    <DetailLayout author={getAuthorInfo()}>
      <Breadcrumb list={breadcrumbs} />
      <ArticleEditor
        height={350}
        currentArticle={currentArticle}
        convertImageCallback={convertImagefile}
        onImageValidate={onImageValidate}
        filterCategories={articleCategories}
        onArticlePost={onArticlePost}
        showValidationError={showValidationError}
      ></ArticleEditor>
    </DetailLayout>
  );
});
