import * as React from "react";
import { Fragment } from "react";
import ArticleEditor from "../Article/ArticleEditor";
import ProductEditor from "../Product/ProductEditor";
import FarmEditor from "../Farm/FarmEditor";
import EditorTabs from "./EditorTabs";

type Props = {
  convertImagefile: (e: any) => Promise<any>;
  onImageValidate: (e: any) => Promise<any>;
  searchArticleCategories: (e: any) => Promise<any>;
  onArticlePost: (e: any) => Promise<any>;
  showValidationError: (title: string, message: string) => void;
  searchProductCategories: (e: any) => Promise<any>;
  searchProductAttributes: (e: any) => Promise<any>;
  searchProductAttributeControlTypes: (e: any) => Promise<any>;
  onProductPost: (e: any) => Promise<number>;
  searchFarms: (e: any) => Promise<any>;
  onFarmPost: (e: any) => Promise<number>;
  searchFarmTypes: (e: any) => Promise<any>;
  editorMode: string;
  onToggleCreateMode: (editorMode: string) => void;
};

const ProfileEditorTabs = (props: Props) => {
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
