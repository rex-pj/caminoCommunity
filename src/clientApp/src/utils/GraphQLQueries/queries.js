import { gql } from "@apollo/client";

export const GET_LOGGED_USER = gql`
  query {
    currentUser: loggedUser {
      lastname
      firstname
      email
      displayName
      userIdentityId
    }
    userPhotos {
      photoType
      code
    }
  }
`;

export const LOGOUT = gql`
  query logout {
    logout {
      isSucceed
      errors {
        code
        message
      }
    }
  }
`;

export const GET_USER_INFO = gql`
  query($criterias: FindUserModelInput!) {
    userInfo: fullUserInfo(criterias: $criterias) {
      canEdit
      email
      displayName
      userIdentityId
      address
      birthDate
      countryName
      description
      createdDate
      phoneNumber
    }
    userPhotos(criterias: $criterias) {
      code
      photoType
    }
  }
`;

export const GET_USER_IDENTIFY = gql`
  query($criterias: FindUserModelInput!) {
    userIdentityInfo: fullUserInfo(criterias: $criterias) {
      canEdit
      email
      firstname
      lastname
      displayName
      userIdentityId
    }
  }
`;

export const GET_FULL_USER_INFO = gql`
  query($criterias: FindUserModelInput!) {
    fullUserInfo(criterias: $criterias) {
      canEdit
      birthDate
      displayName
      email
      firstname
      lastname
      createdDate
      description
      address
      phoneNumber
      genderLabel
      genderId
      countryName
      countryId
      statusLabel
      userIdentityId
    }
    countrySelections {
      id
      text
    }
    genderSelections {
      id
      text
    }
  }
`;

export const ACTIVE = gql`
  query($criterias: ActiveUserModelInput!) {
    active(criterias: $criterias) {
      isSucceed
      errors {
        code
        message
      }
    }
  }
`;

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
        createdById
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

export const GET_USER_PRODUCTS = gql`
  query($criterias: ProductFilterModelInput) {
    userProducts(criterias: $criterias) {
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
        createdById
        createdBy
        createdDate
        updatedDate
        price
        createdByIdentityId
        createdByPhotoCode
        thumbnails {
          pictureId
        }
        productFarms {
          id
          farmId
          farmName
        }
      }
    }
  }
`;

export const GET_USER_FARMS = gql`
  query($criterias: FarmFilterModelInput) {
    userFarms(criterias: $criterias) {
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
        createdById
        createdBy
        createdDate
        updatedDate
        createdByIdentityId
        createdByPhotoCode
        address
        thumbnails {
          pictureId
        }
      }
    }
  }
`;

export const GET_USER_FEEDS = gql`
  query($criterias: FeedFilterModelInput) {
    userFeeds(criterias: $criterias) {
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
        createdById
        createdDate
        createdByIdentityId
        address
        feedType
        createdByName
        pictureId
        price
        createdByPhotoCode
      }
    }
  }
`;

export const GET_FEEDS = gql`
  query($criterias: FeedFilterModelInput) {
    feeds(criterias: $criterias) {
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
        createdById
        createdDate
        createdByIdentityId
        address
        feedType
        createdByName
        pictureId
        price
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
        createdById
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
      createdById
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
      createdById
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
      createdById
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

export const GET_PRODUCTS = gql`
  query($criterias: ProductFilterModelInput) {
    products(criterias: $criterias) {
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
        createdById
        createdBy
        createdDate
        updatedDate
        price
        createdByIdentityId
        createdByPhotoCode
        thumbnails {
          pictureId
        }
        productFarms {
          id
          farmId
          farmName
        }
      }
    }
  }
`;

export const GET_PRODUCT = gql`
  query($criterias: ProductFilterModelInput) {
    product(criterias: $criterias) {
      id
      description
      name
      createdById
      createdBy
      createdDate
      updatedDate
      price
      createdByIdentityId
      createdByPhotoCode
      thumbnails {
        pictureId
      }
      productFarms {
        id
        farmId
        farmName
      }
    }
  }
`;

export const GET_PRODUCT_FOR_UPDATE = gql`
  query($criterias: ProductFilterModelInput) {
    product(criterias: $criterias) {
      id
      description
      name
      createdById
      createdBy
      createdDate
      updatedDate
      price
      createdByIdentityId
      createdByPhotoCode
      productCategories {
        id
        name
      }
      thumbnails {
        pictureId
      }
      productFarms {
        id
        farmId
        farmName
      }
    }
  }
`;

export const GET_RELEVANT_PRODUCTS = gql`
  query($criterias: ProductFilterModelInput) {
    relevantProducts(criterias: $criterias) {
      id
      description
      name
      createdById
      createdBy
      createdDate
      updatedDate
      price
      createdByIdentityId
      createdByPhotoCode
      thumbnails {
        pictureId
      }
      productFarms {
        id
        farmId
        farmName
      }
    }
  }
`;

export const GET_FARMS = gql`
  query($criterias: FarmFilterModelInput) {
    farms(criterias: $criterias) {
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
        createdById
        createdBy
        createdDate
        updatedDate
        createdByIdentityId
        createdByPhotoCode
        address
        thumbnails {
          pictureId
        }
      }
    }
  }
`;

export const GET_FARM = gql`
  query($criterias: FarmFilterModelInput) {
    farm(criterias: $criterias) {
      id
      description
      name
      createdById
      createdBy
      createdDate
      updatedDate
      address
      createdByIdentityId
      createdByPhotoCode
      thumbnails {
        pictureId
      }
    }
  }
`;

export const GET_FARM_FOR_UPDATE = gql`
  query($criterias: FarmFilterModelInput) {
    farm(criterias: $criterias) {
      id
      description
      name
      createdById
      createdBy
      createdDate
      updatedDate
      address
      createdByIdentityId
      createdByPhotoCode
      farmTypeName
      farmTypeId
      thumbnails {
        pictureId
      }
    }
  }
`;
