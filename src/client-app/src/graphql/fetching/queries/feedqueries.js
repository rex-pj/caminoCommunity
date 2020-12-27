import { gql } from "@apollo/client";

export const GET_USER_FEEDS = gql`
  query($criterias: FeedFilterModelInput) {
    userFeeds(criterias: $criterias) {
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
        createdDate
        createdByIdentityId
        address
        feedType
        createdByName
        pictureId
        price
        createdByPhotoCode
      }
    }
  }
`;

export const GET_FEEDS = gql`
  query($criterias: FeedFilterModelInput) {
    feeds(criterias: $criterias) {
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
        createdDate
        createdByIdentityId
        address
        feedType
        createdByName
        pictureId
        price
        createdByPhotoCode
      }
    }
  }
`;
