import React, { Fragment, useContext } from "react";
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
import ProfileEditorTabs from "../../components/organisms/ProfileEditors/ProfileEditorTabs";
import { useStore } from "../../store/hook-store";
import { UrlConstant } from "../../utils/Constants";
import { FeedType } from "../../utils/Enums";
import Loading from "../../components/atoms/Loading";
import ErrorBlock from "../../components/atoms/ErrorBlock";
import { SessionContext } from "../../store/context/SessionContext";

export default withRouter((props) => {
  const { location, pageNumber, pageSize, match } = props;
  const { params } = match;
  const { userId } = params;
  const { user } = useContext(SessionContext);
  const dispatch = useStore(false)[1];

  // Mutations
  const [validateImageUrl] = useMutation(VALIDATE_IMAGE_URL);
  const [articleCategories] = useMutation(FILTER_ARTICLE_CATEGORIES);
  const [productCategories] = useMutation(FILTER_PRODUCT_CATEGORIES);
  const [farmTypes] = useMutation(FILTER_FARM_TYPES);
  const [userFarms] = useMutation(FILTER_FARMS);
  const [createArticle] = useMutation(CREATE_ARTICLE, {
    client: graphqlClient,
  });
  const [createProduct] = useMutation(CREATE_PRODUCT, {
    client: graphqlClient,
  });
  const [createFarm] = useMutation(CREATE_FARM, {
    client: graphqlClient,
  });

  // Queries
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
        page: pageNumber ? parseInt(pageNumber) : 1,
        pageSize: pageSize ? parseInt(pageSize) : 10,
      },
    },
  });

  // Selections mapping
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

  // Search selections
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

  const searchProductCategories = async (inputValue) => {
    return await productCategories({
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

  const searchFarms = async (inputValue) => {
    return await userFarms({
      variables: {
        criterias: { query: inputValue },
      },
    })
      .then((response) => {
        var { data } = response;
        var { userFarms } = data;
        if (!userFarms) {
          return [];
        }
        return userFarms.map((cat) => {
          return {
            value: cat.id,
            label: cat.text,
          };
        });
      })
      .catch((error) => {
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
        return mapSelectListItems(response);
      })
      .catch((error) => {
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

  const onImageValidate = async (value) =>
    await validateImageUrl({
      variables: {
        criterias: {
          url: value,
        },
      },
    });

  const onArticlePost = async (data) => {
    return await createArticle({
      variables: {
        criterias: data,
      },
    }).then((response) => {
      return new Promise((resolve) => {
        const { data } = response;
        const { createArticle: article } = data;
        refetchNewsFeed();
        resolve({ article });
      });
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

  const renderProfileEditorTabs = () => (
    <ProfileEditorTabs
      convertImagefile={convertImagefile}
      onImageValidate={onImageValidate}
      searchArticleCategories={searchArticleCategories}
      onArticlePost={onArticlePost}
      refetchNewsFeed={refetchNewsFeed}
      showValidationError={showValidationError}
      searchProductCategories={searchProductCategories}
      onProductPost={onProductPost}
      searchFarms={searchFarms}
      searchFarmTypes={searchFarmTypes}
      onFarmPost={onFarmPost}
    ></ProfileEditorTabs>
  );

  if (loading || !data || networkStatus === 1) {
    return (
      <Fragment>
        {user && user.isLogin ? renderProfileEditorTabs() : null}
        <Loading>Loading...</Loading>
      </Fragment>
    );
  } else if (error) {
    return (
      <Fragment>
        {user && user.isLogin ? renderProfileEditorTabs() : null}
        <ErrorBlock>Error!</ErrorBlock>
      </Fragment>
    );
  }

  const { userFeeds } = data;
  const { totalPage, filter, collections } = userFeeds;
  const { page } = filter;
  const baseUrl = props.userUrl + "/feeds";
  const pageQuery = location.search;

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

  return (
    <Fragment>
      {user && user.isLogin ? renderProfileEditorTabs() : null}

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
