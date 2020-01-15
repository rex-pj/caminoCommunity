import gpl from "graphql-tag";

export const SIGN_UP = gpl`
  mutation Signup($user: RegisterInputType!){
    signup(user: $user){
      isSucceed,
      errors {
        code,
        description
      }
    }
  }
`;

export const SIGNIN = gpl`
  mutation signin($args: SigninInputType!){
    signin(args: $args){
      result {
        userInfo {
          displayName,
          userIdentityId
        },
        authenticationToken,
      },
      isSucceed,
      errors {
        code
        description
      }
    }
  }
`;

export const UPDATE_USER_IDENTIFIER = gpl`
  mutation UpdateUserIdentifier($user: UserIdentifierUpdateInputType!){
    updateUserIdentifier(user: $user){
      isSucceed,
      errors {
        code,
        description
      },
      result {
        lastname,
        firstname,
        displayName
      }
    }
  }
`;

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
  query($criterias: FindUserInputType!){
    fullUserInfo(criterias: $criterias){
      accessMode
      result{
        email
        displayName
        userIdentityId
        address
        birthDate
        countryName
        description
        createdDate
        phoneNumber,
        avatarUrl,
        coverPhotoUrl
      }
    }
  }
`;

export const GET_FULL_USER_INFO = gpl`
  query($criterias: FindUserInputType!){
    fullUserInfo(criterias: $criterias){
      accessMode
      result {
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
          text
        }
      }
    }
  }
`;

export const UPDATE_USER_INFO_PER_ITEM = gpl`
  mutation UpdateUserInfoItem($criterias: UpdatePerItemInputType!){
    updateUserInfoItem(criterias: $criterias){
      isSucceed,
      errors {
        code
        description
      },
      result {
        value,
        propertyName
      }
    }
  }
`;

export const UPDATE_USER_AVATAR = gpl`
  mutation UpdateAvatar($criterias: UpdateUserPhotoInputType!){
    updateAvatar(criterias: $criterias){
      isSucceed,
      errors {
        code
        description
      },
      result {
        photoUrl,
        xAxis,
        yAxis,
        width,
        height,
        contentType,
        canEdit
      }
    }
  }
`;

export const DELETE_USER_AVATAR = gpl`
  mutation DeleteAvatar($criterias: DeleteUserPhotoInputType!){
    deleteAvatar(criterias: $criterias){
      isSucceed,
      errors {
        code
        description
      },
      result {
        canEdit
      }
    }
  }
`;

export const UPDATE_USER_COVER = gpl`
  mutation UpdateUserCover($criterias: UpdateUserPhotoInputType!){
    updateUserCover(criterias: $criterias){
      isSucceed,
      errors {
        code
        description
      },
      result {
        photoUrl,
        xAxis,
        yAxis,
        width,
        height,
        contentType,
        canEdit
      }
    }
  }
`;

export const DELETE_USER_COVER = gpl`
  mutation DeleteCover($criterias: DeleteUserPhotoInputType!){
    deleteCover(criterias: $criterias){
      isSucceed,
      errors {
        code
        description
      },
      result {
        canEdit
      }
    }
  }
`;

export const UPDATE_USER_PASSWORD = gpl`
  mutation UpdatePassword($criterias: UserPasswordUpdateInputType!){
    updatePassword(criterias: $criterias){
      isSucceed,
      errors {
        code,
        description
      },
      result {
        authenticationToken
      },
    }
  }
`;

export const FORGOT_PASSWORD = gpl`
  mutation ForgotPassword($criterias: ForgotPasswordInputType!){
    forgotPassword(criterias: $criterias){
      isSucceed,
      errors {
        code,
        description
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
        description
      }
    }
  }
`;

export const ACTIVE = gpl`
query ($criterias: ActiveUserInputType!){
  active(criterias: $criterias) {
      isSucceed,
      errors {
        code,
        description
      }
    }
  }
`;

export const RESET_PASSWORD = gpl`
mutation ($criterias: ResetPasswordInputType!){
  resetPassword(criterias: $criterias) {
      isSucceed,
      errors {
        code,
        description
      }
    }
  }
`;
