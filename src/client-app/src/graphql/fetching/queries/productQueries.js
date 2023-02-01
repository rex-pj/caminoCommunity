import { gql } from "@apollo/client";

export const GET_USER_PRODUCTS = gql`
  query ($criterias: ProductFilterModelInput) {
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
        createdByPhotoId
        pictures {
          pictureId
        }
        farms {
          id
          name
        }
      }
    }
  }
`;

export const GET_PRODUCTS = gql`
  query ($criterias: ProductFilterModelInput) {
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
        createdByPhotoId
        pictures {
          pictureId
        }
        farms {
          id
          name
        }
      }
    }
  }
`;

export const GET_PRODUCT = gql`
  query ($criterias: ProductIdFilterModelInput) {
    product(criterias: $criterias) {
      id
      description
      name
      createdBy
      createdDate
      updatedDate
      price
      createdByIdentityId
      createdByPhotoId
      pictures {
        pictureId
      }
      farms {
        id
        name
      }
      productAttributes {
        name
        controlTypeName
        controlTypeId
        displayOrder
        id
        isRequired
        textPrompt
        attributeRelationValues {
          id
          displayOrder
          name
          priceAdjustment
          pricePercentageAdjustment
          quantity
        }
      }
    }
  }
`;

export const GET_PRODUCT_FOR_UPDATE = gql`
  query ($criterias: ProductIdFilterModelInput) {
    product(criterias: $criterias) {
      id
      description
      name
      createdBy
      createdDate
      updatedDate
      price
      createdByIdentityId
      createdByPhotoId
      categories {
        id
        name
      }
      pictures {
        pictureId
      }
      farms {
        id
        name
      }
      productAttributes {
        attributeId
        name
        controlTypeId
        controlTypeName
        displayOrder
        id
        isRequired
        textPrompt
        attributeRelationValues {
          id
          displayOrder
          name
          priceAdjustment
          pricePercentageAdjustment
          quantity
        }
      }
    }
  }
`;

export const GET_RELEVANT_PRODUCTS = gql`
  query ($criterias: ProductFilterModelInput) {
    relevantProducts(criterias: $criterias) {
      id
      description
      name
      createdBy
      createdDate
      updatedDate
      price
      createdByIdentityId
      createdByPhotoId
      pictures {
        pictureId
      }
      farms {
        id
        name
      }
    }
  }
`;

export const FILTER_PRODUCT_CATEGORIES = gql`
  query ($criterias: ProductCategorySelectFilterModelInput) {
    selections: productCategories(criterias: $criterias) {
      id
      isSelected
      text
    }
  }
`;
