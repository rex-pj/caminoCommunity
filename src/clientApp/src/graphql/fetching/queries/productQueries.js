import { gql } from "@apollo/client";

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
