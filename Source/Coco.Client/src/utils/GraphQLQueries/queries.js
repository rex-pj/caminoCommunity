import { gql } from "@apollo/client";

export const GET_LOGGED_USER = gql`
  query {
    currentUser: loggedUser {
      lastname
      firstname
      email
      displayName
      userIdentityId
    }
    userPhotos {
      photoType
      code
    }
  }
`;

export const GET_USER_INFO = gql`
  query($criterias: FindUserModelInput!) {
    userInfo: fullUserInfo(criterias: $criterias) {
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
  query($criterias: FindUserModelInput!) {
    userIdentityInfo: fullUserInfo(criterias: $criterias) {
      canEdit
      email
      firstname
      lastname
      displayName
      userIdentityId
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
