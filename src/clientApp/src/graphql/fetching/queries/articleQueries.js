import { gql } from "@apollo/client";

export const GET_USER_ARTICLES = gql`
  query($criterias: ArticleFilterModelInput) {
    userArticles(criterias: $criterias) {
      totalPage
      totalResult
      filter {
        page
        pageSize
        search
      }
      collections {
        id
        description
        name
        createdBy
        createdDate
        updatedDate
        thumbnail {
          pictureId
        }
        createdByIdentityId
        createdByPhotoCode
      }
    }
  }
`;

export const GET_ARTICLES = gql`
  query($criterias: ArticleFilterModelInput) {
    articles(criterias: $criterias) {
      totalPage
      totalResult
      filter {
        page
        pageSize
        search
      }
      collections {
        id
        description
        name
        createdBy
        createdDate
        updatedDate
        thumbnail {
          pictureId
        }
        createdByIdentityId
        createdByPhotoCode
      }
    }
  }
`;

export const GET_RELEVANT_ARTICLES = gql`
  query($criterias: ArticleFilterModelInput) {
    relevantArticles(criterias: $criterias) {
      id
      description
      name
      createdBy
      createdDate
      updatedDate
      thumbnail {
        pictureId
      }
      createdByIdentityId
      createdByPhotoCode
    }
  }
`;

export const GET_ARTICLE = gql`
  query($criterias: ArticleFilterModelInput) {
    article(criterias: $criterias) {
      id
      content
      name
      createdBy
      createdDate
      updatedDate
      thumbnail {
        pictureId
      }
      createdByIdentityId
      createdByPhotoCode
    }
  }
`;

export const GET_ARTICLE_FOR_UPDATE = gql`
  query($criterias: ArticleFilterModelInput) {
    article(criterias: $criterias) {
      id
      content
      name
      createdBy
      createdDate
      updatedDate
      thumbnail {
        pictureId
      }
      createdByIdentityId
      createdByPhotoCode
      articleCategoryId
      articleCategoryName
    }
  }
`;
