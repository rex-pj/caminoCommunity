import { gql } from "@apollo/client";

export const GET_LOGGED_USER = gql`
  fragment LoggedUserInfoParts on FullUserInfo {
    lastname
    firstname
    email
    displayName
    userIdentityId
  }

  query {
    loggedUser {
      ...LoggedUserInfoParts
    }
    userPhotos {
      photoType
      code
    }
  }
`;

export const GET_USER_INFO = gql`
  query($criterias: FindUserModelInput!) {
    fullUserInfo(criterias: $criterias) {
      canEdit
      email
      displayName
      userIdentityId
      address
      birthDate
      countryName
      description
      createdDate
      phoneNumber
    }
    userPhotos(criterias: $criterias) {
      code
      photoType
    }
  }
`;

export const GET_USER_IDENTIFY = gql`
  fragment UserIdentityInfoParts on UserInfo {
    canEdit
    birthDate
    displayName
    email
    firstname
    lastname
    userIdentityId
  }

  query($criterias: FindUserModelInput!) {
    fullUserInfo(criterias: $criterias) {
      ...UserIdentityInfoParts
    }
  }
`;

export const GET_FULL_USER_INFO = gql`
  query($criterias: FindUserModelInput!) {
    fullUserInfo(criterias: $criterias) {
      canEdit
      birthDate
      displayName
      email
      firstname
      lastname
      createdDate
      description
      address
      phoneNumber
      genderLabel
      genderId
      countryName
      countryId
      statusLabel
      userIdentityId
      countrySelections {
        id
        name
      }
      genderSelections {
        id
        text
      }
    }
  }
`;

export const SIGNOUT = gql`
  query signout {
    signout {
      isSucceed
      errors {
        code
        message
      }
    }
  }
`;

export const ACTIVE = gql`
  query($criterias: ActiveUserModelInput!) {
    active(criterias: $criterias) {
      isSucceed
      errors {
        code
        message
      }
    }
  }
`;
