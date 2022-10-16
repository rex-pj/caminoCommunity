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
      id
    }
  }
`;

export const GET_USER_INFO = gql`
  query ($criterias: FindUserModelInput!) {
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
      id
      photoType
    }
  }
`;

export const GET_SUGGESSTION_USERS = gql`
  query ($criterias: UserFilterModelInput) {
    users(criterias: $criterias) {
      collections {
        countryCode
        countryId
        countryName
        description
        displayName
        firstname
        genderLabel
        lastname
        userIdentityId
        avatarId
      }
    }
  }
`;

export const GET_USER_IDENTIFY = gql`
  query ($criterias: FindUserModelInput!) {
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
  query ($criterias: FindUserModelInput!) {
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
    }
    countrySelections {
      id
      text
    }
    genderSelections {
      id
      text
    }
  }
`;

export const ACTIVE_USER = gql`
  query ($criterias: ActiveUserModelInput!) {
    active(criterias: $criterias) {
      isSucceed
      errors {
        code
        message
      }
    }
  }
`;
