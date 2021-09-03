import { gql } from "@apollo/client";

export const GET_SHORTCUTS = gql`
  query ($criterias: ShortcutFilterModelInput) {
    shortcuts(criterias: $criterias) {
      name
      url
      icon
      typeId
    }
  }
`;
