import React, { Fragment, useState, useContext } from "react";
import { withRouter } from "react-router-dom";
import FeedItem from "../../components/organisms/Feeds/FeedItem";
import { Pagination } from "../../components/organisms/Paging";
import { fileToBase64 } from "../../utils/Helper";
import { useMutation, useQuery } from "@apollo/client";
import graphqlClient from "../../utils/GraphQLClient/graphqlClient";
import {
  VALIDATE_IMAGE_URL,
  FILTER_ARTICLE_CATEGORIES,
  FILTER_PRODUCT_CATEGORIES,
  CREATE_ARTICLE,
  CREATE_PRODUCT,
  FILTER_FARM_TYPES,
  CREATE_FARM,
  FILTER_FARMS,
} from "../../utils/GraphQLQueries/mutations";
import { GET_USER_FEEDS } from "../../utils/GraphQLQueries/queries";
import ArticleEditor from "../../components/organisms/ProfileEditors/ArticleEditor";
import ProductEditor from "../../components/organisms/ProfileEditors/ProductEditor";
import FarmEditor from "../../components/organisms/ProfileEditors/FarmEditor";
import { useStore } from "../../store/hook-store";
import { UrlConstant } from "../../utils/Constants";
import { FeedType } from "../../utils/Enums";
import EditorTabs from "../../components/organisms/User/EditorTabs";
import Loading from "../../components/atoms/Loading";
import ErrorBlock from "../../components/atoms/ErrorBlock";
import { SessionContext } from "../../store/context/SessionContext";

export default withRouter((props) => {
  const [editorMode, setEditorMode] = useState("ARTICLE");
  const { location, pageNumber, match } = props;
  const { params } = match;
  const { userId } = params;
  const { user } = useContext(SessionContext);

  const {
    loading,
    data,
    error,
    refetch: feedsRefetch,
    networkStatus,
  } = useQuery(GET_USER_FEEDS, {
    variables: {
      criterias: {
        userIdentityId: userId,
        page: pageNumber,
      },
    },
  });

  const dispatch = useStore(false)[1];
  const [validateImageUrl] = useMutation(VALIDATE_IMAGE_URL);
  const [articleCategories] = useMutation(FILTER_ARTICLE_CATEGORIES);
  const [productCategories] = useMutation(FILTER_PRODUCT_CATEGORIES);
  const [farmTypes] = useMutation(FILTER_FARM_TYPES);
  const [farms] = useMutation(FILTER_FARMS);
  const [createArticle] = useMutation(CREATE_ARTICLE, {
    client: graphqlClient,
  });
  const [createProduct] = useMutation(CREATE_PRODUCT, {
    client: graphqlClient,
  });
  const [createFarm] = useMutation(CREATE_FARM, {
    client: graphqlClient,
  });

  const searchArticleCategories = async (inputValue) => {
    return await articleCategories({
      variables: {
        criterias: { query: inputValue },
      },
    })
      .then((response) => {
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
      })
      .catch((error) => {
        console.log(error);
        return [];
      });
  };

  const searchProductCategories = async (inputValue) => {
    return await productCategories({
      variables: {
        criterias: { query: inputValue },
      },
    })
      .then((response) => {
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
      })
      .catch((error) => {
        console.log(error);
        return [];
      });
  };

  const searchFarms = async (inputValue) => {
    return await farms({
      variables: {
        criterias: { query: inputValue },
      },
    })
      .then((response) => {
        var { data } = response;
        var { farms } = data;
        if (!farms) {
          return [];
        }
        return farms.map((cat) => {
          return {
            value: cat.id,
            label: cat.text,
          };
        });
      })
      .catch((error) => {
        console.log(error);
        return [];
      });
  };

  const searchFarmTypes = async (inputValue) => {
    return await farmTypes({
      variables: {
        criterias: { query: inputValue },
      },
    })
      .then((response) => {
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
      })
      .catch((error) => {
        console.log(error);
        return [];
      });
  };

  const refetchNewsFeed = () => {
    feedsRefetch();
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
    return await createArticle({
      variables: {
        criterias: data,
      },
    });
  };

  const onProductPost = async (data) => {
    return await createProduct({
      variables: {
        criterias: data,
      },
    });
  };

  const onFarmPost = async (data) => {
    return await createFarm({
      variables: {
        criterias: data,
      },
    });
  };

  const showValidationError = (title, message) => {
    dispatch("NOTIFY", {
      title,
      message,
      type: "error",
    });
  };

  const onToggleCreateMode = (name) => {
    setEditorMode(name);
  };

  let editor = null;
  if (editorMode === "ARTICLE") {
    editor = (
      <ArticleEditor
        height={230}
        convertImageCallback={convertImagefile}
        onImageValidate={onImageValidate}
        filterCategories={searchArticleCategories}
        onArticlePost={onArticlePost}
        refetchNews={refetchNewsFeed}
        showValidationError={showValidationError}
      />
    );
  } else if (editorMode === "PRODUCT") {
    editor = (
      <ProductEditor
        height={230}
        convertImageCallback={convertImagefile}
        onImageValidate={onImageValidate}
        filterCategories={searchProductCategories}
        onProductPost={onProductPost}
        showValidationError={showValidationError}
        refetchNews={refetchNewsFeed}
        filterFarms={searchFarms}
      />
    );
  } else {
    editor = (
      <FarmEditor
        height={230}
        convertImageCallback={convertImagefile}
        onImageValidate={onImageValidate}
        filterCategories={searchFarmTypes}
        onFarmPost={onFarmPost}
        refetchNews={refetchNewsFeed}
        showValidationError={showValidationError}
      />
    );
  }

  if (loading || !data || networkStatus === 1) {
    return (
      <Fragment>
        {user && user.isLogin ? (
          <Fragment>
            <EditorTabs
              onToggleCreateMode={onToggleCreateMode}
              editorMode={editorMode}
            ></EditorTabs>
            {editor}
          </Fragment>
        ) : null}

        <Loading>Loading...</Loading>
      </Fragment>
    );
  } else if (error) {
    return (
      <Fragment>
        {user && user.isLogin ? (
          <Fragment>
            <EditorTabs
              onToggleCreateMode={onToggleCreateMode}
              editorMode={editorMode}
            ></EditorTabs>
            {editor}
          </Fragment>
        ) : null}
        <ErrorBlock>Error!</ErrorBlock>
      </Fragment>
    );
  }

  const { userFeeds } = data;
  const { totalPage, filter, collections } = userFeeds;

  const feeds = collections.map((item) => {
    let feed = { ...item };
    if (feed.feedType === FeedType.Farm) {
      feed.url = `${UrlConstant.Farm.url}${feed.id}`;
    } else if (feed.feedType === FeedType.Article) {
      feed.url = `${UrlConstant.Article.url}${feed.id}`;
    } else if (feed.feedType === FeedType.Product) {
      feed.url = `${UrlConstant.Product.url}${feed.id}`;
    }

    if (feed.pictureId > 0) {
      feed.thumbnailUrl = `${process.env.REACT_APP_CDN_PHOTO_URL}${feed.pictureId}`;
    }

    feed.creator = {
      createdDate: item.createdDate,
      profileUrl: `/profile/${item.createdByIdentityId}`,
      name: item.createdByName,
    };

    if (item.createdByPhotoCode) {
      feed.creator.photoUrl = `${process.env.REACT_APP_CDN_AVATAR_API_URL}${item.createdByPhotoCode}`;
    }

    return feed;
  });

  const pageQuery = location.search;
  const { page } = filter;
  const baseUrl = props.userUrl + "/feeds";
  return (
    <Fragment>
      {user && user.isLogin ? (
        <Fragment>
          <EditorTabs
            onToggleCreateMode={onToggleCreateMode}
            editorMode={editorMode}
          ></EditorTabs>

          {editor}
        </Fragment>
      ) : null}

      {feeds
        ? feeds.map((item) => {
            const key = `${item.feedType}_${item.id}`;
            return <FeedItem key={key} feed={item} />;
          })
        : null}
      <Pagination
        totalPage={totalPage}
        baseUrl={baseUrl}
        pageQuery={pageQuery}
        currentPage={page}
      />
    </Fragment>
  );
});
