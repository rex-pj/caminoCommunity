import { gql } from "@apollo/client";

export const FILTER_ARTICLE_CATEGORIES = gql`
  mutation ($criterias: ArticleCategorySelectFilterModelInput) {
    selections: articleCategories(criterias: $criterias) {
      id
      isSelected
      text
    }
  }
`;

export const CREATE_ARTICLE = gql`
  mutation ($criterias: CreateArticleModelInput!) {
    createArticle(criterias: $criterias) {
      id
    }
  }
`;

export const UPDATE_ARTICLE = gql`
  mutation ($criterias: UpdateArticleModelInput!) {
    updateArticle(criterias: $criterias) {
      id
    }
  }
`;

export const DELETE_ARTICLE = gql`
  mutation ($criterias: ArticleIdFilterModelInput!) {
    deleteArticle(criterias: $criterias)
  }
`;
