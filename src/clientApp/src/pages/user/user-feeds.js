import React, { Fragment, useState } from "react";
import { withRouter } from "react-router-dom";
import FeedItem from "../../components/organisms/Feeds/FeedItem";
import { Pagination } from "../../components/organisms/Paging";
import { fileToBase64 } from "../../utils/Helper";
import { useMutation } from "@apollo/client";
import styled from "styled-components";
import graphqlClient from "../../utils/GraphQLClient/graphqlClient";
import {
  VALIDATE_IMAGE_URL,
  FILTER_ARTICLE_CATEGORIES,
  FILTER_PRODUCT_CATEGORIES,
  CREATE_ARTICLE,
  CREATE_PRODUCT,
  FILTER_FARM_TYPES,
  CREATE_FARM
} from "../../utils/GraphQLQueries/mutations";
import ArticleEditor from "../../components/organisms/ProfileEditors/ArticleEditor";
import ProductEditor from "../../components/organisms/ProfileEditors/ProductEditor";
import FarmEditor from "../../components/organisms/ProfileEditors/FarmEditor";
import { useStore } from "../../store/hook-store";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { ButtonSecondary } from "../../components/atoms/Buttons/Buttons";

const EditorTabs = styled.div`
  margin-bottom: ${(p) => p.theme.size.exTiny};
  .tabs-bar button {
    border-radius: ${(p) => p.theme.borderRadius.large};
    color: ${(p) => p.theme.color.light};
    background-color: transparent;
    font-weight: normal;
    border-color: transparent;
  }
  .tabs-bar button.actived {
    color: ${(p) => p.theme.color.neutral};
    background-color: ${(p) => p.theme.color.light};
  }
`;

export default withRouter((props) => {
  const feeds = [];
  const [editorMode, setEditorMode] = useState("ARTICLE");

  const { location, pageNumber, userUrl } = props;

  const [pageOptions] = useState({
    totalPage: 10,
    pageQuery: location.search,
    baseUrl: userUrl + "/feeds",
    currentPage: pageNumber ? pageNumber : 1,
  });

  const dispatch = useStore(false)[1];
  const [validateImageUrl] = useMutation(VALIDATE_IMAGE_URL);
  const [articleCategories] = useMutation(FILTER_ARTICLE_CATEGORIES);
  const [productCategories] = useMutation(FILTER_PRODUCT_CATEGORIES);
  const [farmTypes] = useMutation(FILTER_FARM_TYPES);
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
        showValidationError={showValidationError}
      />
    );
  } else if(editorMode === "PRODUCT") {
    editor = (
      <ProductEditor
        height={230}
        convertImageCallback={convertImagefile}
        onImageValidate={onImageValidate}
        filterCategories={searchProductCategories}
        onProductPost={onProductPost}
        showValidationError={showValidationError}
      />
    );
  }
  else{
    editor = (
      <FarmEditor
        height={230}
        convertImageCallback={convertImagefile}
        onImageValidate={onImageValidate}
        filterCategories={searchFarmTypes}
        onFarmPost={onFarmPost}
        showValidationError={showValidationError}
      />
    );
  }

  const { totalPage, currentPage, baseUrl, pageQuery } = pageOptions;
  return (
    <Fragment>
      <EditorTabs>
        <div className="tabs-bar">
          <ButtonSecondary
            size="sm"
            className={`mr-1${editorMode === "ARTICLE" ? " actived" : ""}`}
            onClick={()=> onToggleCreateMode("ARTICLE")}
          >
            <span>
              <FontAwesomeIcon
                icon="newspaper"
                className="mr-1"
              ></FontAwesomeIcon>
              Create Post
            </span>
          </ButtonSecondary>
          <ButtonSecondary
            size="sm"
            onClick={()=> onToggleCreateMode("PRODUCT")}
            className={`mr-1${editorMode === "PRODUCT" ? " actived" : ""}`}
          >
            <span>
              <FontAwesomeIcon
                icon="apple-alt"
                className="mr-1"
              ></FontAwesomeIcon>
              Create Product
            </span>
          </ButtonSecondary>
          <ButtonSecondary
            size="sm"
            onClick={()=> onToggleCreateMode("FARM")}
            className={`mr-1${editorMode === "FARM" ? " actived" : ""}`}
          >
            <span>
              <FontAwesomeIcon
                icon="warehouse"
                className="mr-1"
              ></FontAwesomeIcon>
              Create Farm
            </span>
          </ButtonSecondary>
        </div>
      </EditorTabs>

      {editor}

      {feeds
        ? feeds.map((item, index) => <FeedItem key={index} feed={item} />)
        : null}
      <Pagination
        totalPage={totalPage}
        baseUrl={baseUrl}
        pageQuery={pageQuery}
        currentPage={currentPage}
      />
    </Fragment>
  );
});
