import { gql } from "@apollo/client";

export const GET_SELECT_USERS = gql`
  mutation ($criterias: UserFilterModelInput) {
    selectUsers(criterias: $criterias) {
      id
      text
      isSelected
    }
  }
`;
