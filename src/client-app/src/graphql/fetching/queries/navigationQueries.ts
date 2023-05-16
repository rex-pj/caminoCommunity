import { gql } from "@apollo/client";

export const GET_SHORTCUTS = gql`
  query ($criterias: ShortcutFilterModelInput) {
    shortcuts(criterias: $criterias) {
      id
      name
      url
      icon
      typeId
    }
  }
`;
