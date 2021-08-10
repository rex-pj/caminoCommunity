import { gql } from "@apollo/client";

export const Live_Search = gql`
  mutation ($criterias: FeedFilterModelInput) {
    liveSearch(criterias: $criterias) {
      articles {
        id
        description
        name
        createdDate
        feedType
        pictureId
      }
      products {
        id
        description
        name
        createdDate
        feedType
        pictureId
      }
      farms {
        id
        description
        name
        createdDate
        feedType
        pictureId
      }
      users {
        id
        description
        name
        createdDate
        feedType
        pictureId
      }
    }
  }
`;
