import gpl from "graphql-tag";

export const GET_LOGGED_USER = gpl`
  query{
    loggedUser{
      lastname,
      firstname,
      email,
      displayName,
      userIdentityId,
      avatarUrl,
      coverPhotoUrl
    }
  }
`;

export const GET_USER_INFO = gpl`
  query($criterias: FindUserModelInput!){
    fullUserInfo(criterias: $criterias){
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
      avatarUrl
      coverPhotoUrl
    }
  }
`;

export const GET_USER_IDENTIFY = gpl`
  query($criterias: FindUserModelInput!){
    fullUserInfo(criterias: $criterias){
      canEdit
      birthDate
      displayName
      email
      firstname
      lastname
      userIdentityId
    }
  }
`;

export const GET_FULL_USER_INFO = gpl`
  query($criterias: FindUserModelInput!){
    fullUserInfo(criterias: $criterias){
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
      coverPhotoUrl
      genderSelections {
        id
        text
      }
      countrySelections {
        id
        name
      }
    }
  }
`;

export const SIGNOUT = gpl`
query signout{
    signout {
      isSucceed,
      errors {
        code,
        message
      }
    }
  }
`;

export const ACTIVE = gpl`
query ($criterias: ActiveUserModelInput!){
  active(criterias: $criterias) {
      isSucceed,
      errors {
        code,
        message
      }
    }
  }
`;
