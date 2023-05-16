import { gql } from "@apollo/client";

export const UPDATE_PRODUCT = gql`
  mutation ($criterias: UpdateProductModelInput!) {
    updateProduct(criterias: $criterias) {
      id
    }
  }
`;

export const CREATE_PRODUCT = gql`
  mutation ($criterias: CreateProductModelInput!) {
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
  mutation ($criterias: AttributeSelectFilterModelInput) {
    selections: productAttributes(criterias: $criterias) {
      id
      isSelected
      text
    }
  }
`;

export const FILTER_PRODUCT_ATTRIBUTE_CONTROL_TYPES = gql`
  mutation ($criterias: AttributeControlTypeSelectFilterModelInput) {
    selections: productAttributeControlTypes(criterias: $criterias) {
      id
      isSelected
      text
    }
  }
`;
