import React, { Fragment, useEffect } from "react";
import { useMutation, useQuery } from "@apollo/client";
import Breadcrumb from "../../components/organisms/Navigation/Breadcrumb";
import Loading from "../../components/atoms/Loading";
import { withRouter } from "react-router-dom";
import ErrorBlock from "../../components/atoms/ErrorBlock";
import ArticleEditor from "../../components/organisms/ProfileEditors/ArticleEditor";
import graphqlClient from "../../utils/GraphQLClient/graphqlClient";
import {
  VALIDATE_IMAGE_URL,
  FILTER_ARTICLE_CATEGORIES,
  UPDATE_ARTICLE,
} from "../../utils/GraphQLQueries/mutations";
import { GET_ARTICLE_FOR_UPDATE } from "../../utils/GraphQLQueries/queries";
import { useStore } from "../../store/hook-store";
import { fileToBase64 } from "../../utils/Helper";
import articleCreationModel from "../../models/articleCreationModel";

export default withRouter(function (props) {
  const { match } = props;
  const { params } = match;
  const { id } = params;
  const dispatch = useStore(false)[1];
  const [validateImageUrl] = useMutation(VALIDATE_IMAGE_URL);
  const [updateArticle] = useMutation(UPDATE_ARTICLE, {
    client: graphqlClient,
  });
  const [articleCategories] = useMutation(FILTER_ARTICLE_CATEGORIES);

  const { loading, data, error, refetch, called } = useQuery(
    GET_ARTICLE_FOR_UPDATE,
    {
      variables: {
        criterias: {
          id: parseFloat(id),
        },
      },
    }
  );

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

  const mapSelectListItems = (response) => {
    var { data } = response;
    var { categories } = data;
    if (!categories) {
      return [];
    }
    return categories.map((cat) => {
      return {
        value: cat.id,
        label: cat.text,
      };
    });
  };

  const searchArticleCategories = async (inputValue) => {
    return await articleCategories({
      variables: {
        criterias: { query: inputValue },
      },
    })
      .then((response) => {
        return mapSelectListItems(response);
      })
      .catch((error) => {
        return [];
      });
  };

  const currentArticle = JSON.parse(JSON.stringify(articleCreationModel));
  for (const formIdentifier in currentArticle) {
    currentArticle[formIdentifier].value = article[formIdentifier];
    if (article[formIdentifier]) {
      currentArticle[formIdentifier].isValid = true;
    }
  }

  return (
    <Fragment>
      <Breadcrumb list={breadcrumbs} />
      <ArticleEditor
        height={350}
        currentArticle={currentArticle}
        convertImageCallback={convertImagefile}
        onImageValidate={onImageValidate}
        filterCategories={searchArticleCategories}
        onArticlePost={onArticlePost}
        showValidationError={showValidationError}
      ></ArticleEditor>
    </Fragment>
  );
});
