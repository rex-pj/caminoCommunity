import { gql } from "@apollo/client";

export const FILTER_PRODUCT_CATEGORIES = gql`
  mutation ($criterias: ProductCategorySelectFilterModelInput) {
    selections: productCategories(criterias: $criterias) {
      id
      isSelected
      text
    }
  }
`;

export const UPDATE_PRODUCT = gql`
  mutation ($criterias: ProductModelInput!) {
    updateProduct(criterias: $criterias) {
      id
    }
  }
`;

export const CREATE_PRODUCT = gql`
  mutation ($criterias: ProductModelInput!) {
    createProduct(criterias: $criterias) {
      id
    }
  }
`;

export const DELETE_PRODUCT = gql`
  mutation ($criterias: ProductIdFilterModelInput!) {
    deleteProduct(criterias: $criterias)
  }
`;

export const FILTER_PRODUCT_ATTRIBUTES = gql`
  mutation ($criterias: ProductAttributeSelectFilterModelInput) {
    selections: productAttributes(criterias: $criterias) {
      id
      isSelected
      text
    }
  }
`;

export const FILTER_PRODUCT_ATTRIBUTE_CONTROL_TYPES = gql`
  mutation ($criterias: ProductAttributeControlTypeSelectFilterModelInput) {
    selections: productAttributeControlTypes(criterias: $criterias) {
      id
      isSelected
      text
    }
  }
`;
