import { gql } from "@apollo/client";

export const FILTER_PRODUCT_CATEGORIES = gql`
  mutation($criterias: SelectFilterModelInput) {
    categories: productCategories(criterias: $criterias) {
      id
      isSelected
      text
    }
  }
`;

export const UPDATE_PRODUCT = gql`
  mutation($criterias: ProductModelInput!) {
    updateProduct(criterias: $criterias) {
      id
    }
  }
`;

export const CREATE_PRODUCT = gql`
  mutation($criterias: ProductModelInput!) {
    createProduct(criterias: $criterias) {
      id
    }
  }
`;
