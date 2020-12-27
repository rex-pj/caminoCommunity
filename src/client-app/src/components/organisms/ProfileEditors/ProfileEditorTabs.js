import React, { Fragment, useState } from "react";
import ArticleEditor from "./ArticleEditor";
import ProductEditor from "./ProductEditor";
import FarmEditor from "./FarmEditor";
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
    onProductPost,
    searchFarms,
    searchFarmTypes,
    onFarmPost,
  } = props;

  const [editorMode, setEditorMode] = useState("ARTICLE");
  const onToggleCreateMode = (name) => {
    setEditorMode(name);
  };

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
