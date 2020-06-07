import { gql } from "@apollo/client";

const UserFragment = {
  avatar: gql`
    fragment UserAvatarParts on UserAvatar {
      url
    }
  `,
  cover: gql`
    fragment UserCovertParts on UserCover {
      url
    }
  `,
  countries: gql`
    fragment CountryParts on CountryList {
      CountrySelections {
        id
        name
      }
    }
  `,
  genders: gql`
    fragment GenderParts on GenderList {
      GenderSelections {
        id
        text
      }
    }
  `,
};

export const GET_LOGGED_USER = gql`
  fragment UserInfoParts on UserInfo {
    lastname
    firstname
    email
    displayName
    userIdentityId
  }

  ${UserFragment.avatar}
  ${UserFragment.cover}

  query {
    loggedUser {
      ...UserInfoParts
      ...UserAvatarParts
      ...UserCovertParts
    }
  }
`;

export const GET_USER_INFO = gql`
  ${UserFragment.avatar}
  ${UserFragment.cover}

  fragment UserInfoParts on UserInfo {
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
  query($criterias: FindUserModelInput!) {
    fullUserInfo(criterias: $criterias) {
      ...UserInfoParts
      ...UserAvatarParts
      ...UserCovertParts
    }
  }
`;

export const GET_USER_IDENTIFY = gql`
  fragment UserInfoParts on UserInfo {
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
      ...UserInfoParts
    }
  }
`;

export const GET_FULL_USER_INFO = gql`
  fragment UserInfoParts on UserInfo {
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

  ${UserFragment.cover}
  ${UserFragment.countries}
  ${UserFragment.genders}
  query($criterias: FindUserModelInput!) {
    fullUserInfo(criterias: $criterias) {
      ...UserInfoParts
      ...UserCovertParts
      ...GenderParts
      ...CountryParts
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
