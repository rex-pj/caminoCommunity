import { gql } from "@apollo/client";

export const LIVE_SEARCH = gql`
  mutation ($criterias: FeedFilterModelInput) {
    liveSearch(criterias: $criterias) {
      articles {
        id
        name
        feedType
        pictureId
      }
      products {
        id
        name
        feedType
        pictureId
      }
      farms {
        id
        name
        feedType
        pictureId
      }
      users {
        id
        name
        feedType
        pictureId
      }
    }
  }
`;
