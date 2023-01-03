import React, { Fragment } from "react";
import ArticleEditor from "../Article/ArticleEditor";
import ProductEditor from "../Product/ProductEditor";
import FarmEditor from "../Farm/FarmEditor";
import EditorTabs from "./EditorTabs";

const ProfileEditorTabs = (props) => {
  const {
    convertImagefile,
    onImageValidate,
    searchArticleCategories,
    onArticlePost,
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
        showValidationError={showValidationError}
      />
    </Fragment>
  );
};

export default ProfileEditorTabs;
