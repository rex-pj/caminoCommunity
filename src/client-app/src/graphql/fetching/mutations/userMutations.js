import { gql } from "@apollo/client";

export const REFRESH_TOKEN = gql`
  mutation refreshToken {
    refreshToken {
      authenticationToken
      refreshToken
      refreshTokenExpiryTime
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
