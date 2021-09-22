import { gql } from "@apollo/client";

export const SIGNUP = gql`
  mutation Signup($criterias: SignupModelInput) {
    signup(criterias: $criterias) {
      isSucceed
      errors {
        code
        message
      }
    }
  }
`;

export const LOGIN = gql`
  mutation login($criterias: LoginModelInput!) {
    login(criterias: $criterias) {
      authenticationToken
      refreshToken
      refreshTokenExpiryTime
    }
  }
`;

export const REFRESH_TOKEN = gql`
  mutation refreshToken {
    refreshToken {
      authenticationToken
      refreshToken
      refreshTokenExpiryTime
    }
  }
`;

export const UPDATE_USER_IDENTIFIER = gql`
  mutation UpdateIdentifier($criterias: UserIdentifierUpdateModelInput) {
    updateIdentifier(criterias: $criterias) {
      lastname
      firstname
      displayName
    }
  }
`;

export const PARTIAL_USER_UPDATE = gql`
  mutation PartialUserUpdate($criterias: PartialUpdateRequestModelInput!) {
    partialUserUpdate(criterias: $criterias) {
      propertyName
    }
  }
`;

export const UPDATE_USER_AVATAR = gql`
  mutation UpdateAvatar($criterias: UserPhotoUpdateModelInput!) {
    updateAvatar(criterias: $criterias) {
      isSucceed
      errors {
        code
        message
      }
    }
  }
`;

export const DELETE_USER_AVATAR = gql`
  mutation DeleteAvatar($criterias: PhotoDeleteModelInput!) {
    deleteAvatar(criterias: $criterias) {
      isSucceed
      errors {
        code
        message
      }
    }
  }
`;

export const UPDATE_USER_COVER = gql`
  mutation UpdateCover($criterias: UserPhotoUpdateModelInput!) {
    updateCover(criterias: $criterias) {
      isSucceed
      errors {
        code
        message
      }
    }
  }
`;

export const DELETE_USER_COVER = gql`
  mutation DeleteCover($criterias: PhotoDeleteModelInput!) {
    deleteCover(criterias: $criterias) {
      isSucceed
      errors {
        code
        message
      }
      result
    }
  }
`;

export const UPDATE_USER_PASSWORD = gql`
  mutation UpdatePassword($criterias: UserPasswordUpdateModelInput!) {
    updatePassword(criterias: $criterias) {
      authenticationToken
      isSucceed
    }
  }
`;

export const FORGOT_PASSWORD = gql`
  mutation ForgotPassword($criterias: ForgotPasswordModelInput!) {
    forgotPassword(criterias: $criterias) {
      isSucceed
      errors {
        code
        message
      }
    }
  }
`;

export const RESET_PASSWORD = gql`
  mutation ($criterias: ResetPasswordModelInput!) {
    resetPassword(criterias: $criterias) {
      isSucceed
      errors {
        code
        message
      }
    }
  }
`;

export const GET_SELECT_USERS = gql`
  mutation ($criterias: UserFilterModelInput) {
    selectUsers(criterias: $criterias) {
      id
      text
      isSelected
    }
  }
`;
