import React, { Fragment } from "react";
import ArticleEditor from "../Article/ArticleEditor";
import ProductEditor from "../Product/ProductEditor";
import FarmEditor from "../Farm/FarmEditor";
import EditorTabs from "./EditorTabs";

export default (props) => {
  const {
    convertImagefile,
    onImageValidate,
    searchArticleCategories,
    onArticlePost,
    refetchNewsFeed,
    showValidationError,
    searchProductCategories,
    searchProductAttributes,
    searchProductAttributeControlTypes,
    onProductPost,
    searchFarms,
    searchFarmTypes,
    onFarmPost,
    editorMode,
    onToggleCreateMode,
  } = props;

  if (editorMode === "ARTICLE") {
    return (
      <Fragment>
        <EditorTabs
          onToggleCreateMode={onToggleCreateMode}
          editorMode={editorMode}
        ></EditorTabs>
        <ArticleEditor
          height={230}
          convertImageCallback={convertImagefile}
          onImageValidate={onImageValidate}
          filterCategories={searchArticleCategories}
          onArticlePost={onArticlePost}
          refetchNews={refetchNewsFeed}
          showValidationError={showValidationError}
        />
      </Fragment>
    );
  }

  if (editorMode === "PRODUCT") {
    return (
      <Fragment>
        <EditorTabs
          onToggleCreateMode={onToggleCreateMode}
          editorMode={editorMode}
        ></EditorTabs>
        <ProductEditor
          height={230}
          convertImageCallback={convertImagefile}
          onImageValidate={onImageValidate}
          filterCategories={searchProductCategories}
          onProductPost={onProductPost}
          showValidationError={showValidationError}
          refetchNews={refetchNewsFeed}
          filterFarms={searchFarms}
          filterAttributes={searchProductAttributes}
          filterProductAttributeControlTypes={
            searchProductAttributeControlTypes
          }
        />
      </Fragment>
    );
  }

  return (
    <Fragment>
      <EditorTabs
        onToggleCreateMode={onToggleCreateMode}
        editorMode={editorMode}
      ></EditorTabs>
      <FarmEditor
        height={230}
        convertImageCallback={convertImagefile}
        onImageValidate={onImageValidate}
        filterCategories={searchFarmTypes}
        onFarmPost={onFarmPost}
        refetchNews={refetchNewsFeed}
        showValidationError={showValidationError}
      />
    </Fragment>
  );
};
