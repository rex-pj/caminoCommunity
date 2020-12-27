import { gql } from "@apollo/client";

export const VALIDATE_IMAGE_URL = gql`
  mutation($criterias: ImageValidationModelInput!) {
    validateImageUrl(criterias: $criterias) {
      isSucceed
    }
  }
`;
